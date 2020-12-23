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

    [SerializeField] List<GemTier> tiers;
    GemTier currentTier;

    Rigidbody rb;
    bool isBeingThrown = false;
    bool isCharged = false;
    bool isFalling = false;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (!isBeingThrown && collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player.isStunned) return;

            if (player.TryAddGemToPouch(this))
                gameObject.SetActive(false);
        }

        //Todo esto si la gema no está cargada
        //Meterse al minecart
        if (isBeingThrown)
        {
            //Si es otro jugador se para y lo aturde
            if (collision.gameObject.tag == "Player")
            {
                Debug.Log("Player");
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
            Debug.Log("Espalda");
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
