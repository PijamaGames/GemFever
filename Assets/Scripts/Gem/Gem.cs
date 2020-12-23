using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] int value = 1;

    [SerializeField] float horizontalSpawnForce = 6f;
    [SerializeField] float verticalSpawnForce = 6f;
    [Space]

    [SerializeField] float knockbackForce = 0f;
    [SerializeField][Range(0f, 1f)] float speedIncrementPerParry = 0.1f;
    [SerializeField] float maxSpeedIncrement = 5f;

    [SerializeField] List<GemTier> tiers;
    GemTier currentTier;

    Rigidbody rb;
    [HideInInspector] public bool isBeingThrown = false;
    /*[HideInInspector]*/ public bool isCharged = false;
    [HideInInspector] public bool isFalling = false;

    float throwSpeedMultiplier = 1f;
    Vector3 playerForward;
    public Player playerOwner = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentTier = tiers[0];
        SpawnnForce();
    }

    private void SpawnnForce()
    {
        Vector3 force = Vector3.zero;

        force.x = Random.Range(-horizontalSpawnForce, horizontalSpawnForce);
        force.y = Random.Range(-verticalSpawnForce, verticalSpawnForce);

        rb.AddForce(force, ForceMode.Impulse);
    }

    public void DropForce(Vector3 dropDirection, float dropForce)
    {
        rb.AddForce(dropDirection * dropForce, ForceMode.Impulse);
    }

    public IEnumerator IgnoreCollisionsForSomeTime(Collider toIgnore, float timeToIgnore)
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), toIgnore, true);
        yield return new WaitForSecondsRealtime(timeToIgnore);
        Physics.IgnoreCollision(GetComponent<Collider>(), toIgnore, false);
    }

    public void UpdateGemValue(int newValue)
    {
        value = newValue;

        bool currentTierFound = false;

        GemTier nextTier;

        for(int i = tiers.IndexOf(currentTier); i < tiers.Count; i++)
        {
            if(!currentTierFound)
            {
                if (i == tiers.Count - 1)
                {
                    currentTierFound = true;
                }
                else
                {
                    nextTier = tiers[i + 1];
                    if(value >= nextTier.minValueForThisTier)
                    {
                        currentTier = nextTier;
                    }
                }
            }          
        }

        gameObject.GetComponent<MeshRenderer>().material.color = currentTier.tierColor;
        //Actualizar modelo de la gema
    }

    public void ThrowGem(Vector3 playerForward, Vector3 playerPosition, float throwForce, Player playerOwner)
    {
        this.playerForward = playerForward;
        this.playerOwner = playerOwner;

        transform.position = playerPosition;

        rb.AddForce(this.playerForward * throwForce * throwSpeedMultiplier, ForceMode.Impulse);
        rb.useGravity = false;

        Physics.IgnoreCollision(this.GetComponent<Collider>(), playerOwner.GetComponent<Collider>(), true);
        gameObject.layer = LayerMask.NameToLayer("ThrownGem");

        isBeingThrown = true;
    }

    public void ParryGem(Vector3 newDirection, float throwForce)
    {
        if (!isCharged) isCharged = true;

        throwSpeedMultiplier += speedIncrementPerParry;

        if (throwSpeedMultiplier > maxSpeedIncrement)
            throwSpeedMultiplier = maxSpeedIncrement;

        rb.velocity = Vector3.zero;

        rb.AddForce(newDirection * throwForce * throwSpeedMultiplier, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isBeingThrown && collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player.isStunned) return;

            if (player.TryAddGemToPouch(this))
                gameObject.SetActive(false);
        }

        //Si está cargada hace que el jugador suelte gemas
        if (isCharged)
        {
            if (collision.gameObject.tag == "Player")
            {
                //Debug.Log("Player charged");
                rb.velocity = Vector3.zero;
                isCharged = false;

                StopThrowing();
                Player playerHit = collision.gameObject.GetComponent<Player>();

                playerHit.Knockback(playerForward, knockbackForce);
                playerHit.DropGem(-this.transform.forward);
            }
        }

        //Todo esto si la gema no está cargada
        //Meterse al minecart
        else if (isBeingThrown)
        {
            //Si es otro jugador se para y lo aturde
            if (collision.gameObject.tag == "Player")
            {
                //Debug.Log("Player not charged");
                StopThrowing();

                collision.gameObject.GetComponent<Player>().Knockback(playerForward, knockbackForce);
            }

            //Choque con otra gema en el aire
            else if (collision.gameObject.tag == "Gem")
            {
                if(collision.gameObject.GetComponent<Gem>().isBeingThrown)
                {
                    StopThrowing();
                    collision.gameObject.GetComponent<Gem>().StopThrowing();
                }
            }

            //Pared/Suelo
            else if (collision.gameObject.tag == "Map")
            {
                StopThrowing();
                isFalling = true;
            }
        }

        else if(isFalling)
        {
            //Si mientras cae toca el suelo se para
            if(collision.gameObject.tag == "Map")
            {
                rb.velocity = Vector3.zero;
                isFalling = false;
            }

            //Si mientras cae le da a otro jugador le hace daño y se para
            else if(collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<Player>().Knockback(Vector3.zero, 0f);

                rb.velocity = Vector3.zero;
                isFalling = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Si le da a otro jugador por la espalda se mete en su bolsa si puede, si no lo aturde
        if (isBeingThrown && other.tag == "PlayerBack")
        {
            //Debug.Log("Espalda");
            PlayerBack playerBack = other.gameObject.GetComponent<PlayerBack>();

            if (playerBack.TryAddGemToPouch(this))
            {
                StopThrowing();
                gameObject.SetActive(false);
            }
            else
                playerBack.Knockback(playerForward, knockbackForce);
        }
    }

    private void StopThrowing()
    {
        rb.velocity = Vector3.zero;

        rb.useGravity = true;
        isBeingThrown = false;
        isCharged = false;

        throwSpeedMultiplier = 1f;

        gameObject.layer = LayerMask.NameToLayer("Gem");

        if (playerOwner != null)
        {
            Physics.IgnoreCollision(this.GetComponent<Collider>(), playerOwner.GetComponent<Collider>(), false);
            playerOwner = null;
        }
    }
}

[System.Serializable]
public class GemTier 
{ 
    [Tooltip("Valor mínimo que debe tener la gema para entrar en este tier")]public int minValueForThisTier; 
    public Color tierColor; 
    public Mesh tierMesh; 
}
