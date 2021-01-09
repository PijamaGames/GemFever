using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public int value = 1;

    [SerializeField] float horizontalSpawnForce = 6f;
    [SerializeField] float verticalSpawnForce = 6f;
    [Space]

    [SerializeField] float knockbackForce = 0f;
    [SerializeField][Range(0f, 1f)] float speedIncrementPerParry = 0.1f;
    [SerializeField] float maxSpeedIncrement = 5f;

    [SerializeField] List<GemTier> tiers;
    [SerializeField] MeshRenderer gemMesh;
    GemTier currentTier;

    Rigidbody rb;
    GemPool gemPool;
    [HideInInspector] public bool isBeingThrown = false;
    /*[HideInInspector]*/ public bool isCharged = false;
    [HideInInspector] public bool isFalling = false;

    float throwSpeedMultiplier = 1f;
    Vector3 playerForward;
    public Player playerOwner = null;

    //Sound
    PersistentAudioSource audioSource;
    [SerializeField] AudioClip gemPickup;
    [SerializeField] AudioClip gemHitPlayer;
    [SerializeField] AudioClip gemHitGem;
    [SerializeField] AudioClip gemHitMap;
    [SerializeField] AudioClip chargedGemHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gemPool = FindObjectOfType<GemPool>();
        audioSource = FindObjectOfType<PersistentAudioSource>();
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

    private void PlaySound(AudioClip clip)
    {
        if(clip != null)
            audioSource.PlayEffect(clip);
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

        //TODO: Cambiarle el material entero no el tint
        gemMesh.material = currentTier.tierMaterial;
    }

    public void ThrowGem(Vector3 playerForward, Vector3 playerPosition, float throwForce, Player playerOwner)
    {
        this.playerForward = playerForward;
        this.playerOwner = playerOwner;

        transform.position = playerPosition;

        rb.useGravity = false;
        rb.AddForce(this.playerForward * throwForce * throwSpeedMultiplier, ForceMode.Impulse);

        Physics.IgnoreCollision(this.GetComponent<Collider>(), playerOwner.GetComponent<Collider>(), true);
        gameObject.layer = LayerMask.NameToLayer("ThrownGem");

        isBeingThrown = true;
    }

    public void ParryGem(Vector3 newDirection, float throwForce, Player newPlayerOwner)
    {
        if (!isCharged) isCharged = true;

        this.playerOwner = newPlayerOwner;

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

            PlaySound(gemPickup);

            if (player.TryAddGemToPouch(this))
                gameObject.SetActive(false);
        }

        //Si está cargada hace que el jugador suelte gemas
        if (isCharged)
        {
            if (collision.gameObject.tag == "Player")
            {
                Debug.Log("Player charged");
                rb.velocity = Vector3.zero;
                isCharged = false;

                StopThrowing();
                Player playerHit = collision.gameObject.GetComponent<Player>();

                playerHit.Knockback(playerForward, knockbackForce);
                playerHit.DropGem(-this.transform.forward);

                PlaySound(chargedGemHit);
            }

            //Pared/Suelo
            else if (collision.gameObject.tag == "Map" || collision.gameObject.tag == "Floor")
            {
                StopThrowing();
                PlaySound(gemHitMap);

                isFalling = true;
            }
        }

        //Todo esto si la gema no está cargada
        else if (isBeingThrown)
        {
            //Si es otro jugador se para y lo aturde
            if (collision.gameObject.tag == "Player")
            {
                //Debug.Log("Player not charged");
                StopThrowing();

                collision.gameObject.GetComponent<Player>().Knockback(playerForward, knockbackForce);

                PlaySound(gemHitPlayer);
            }

            //Choque con otra gema en el aire
            else if (collision.gameObject.tag == "Gem")
            {
                if(collision.gameObject.GetComponent<Gem>().isBeingThrown)
                {
                    StopThrowing();
                    collision.gameObject.GetComponent<Gem>().StopThrowing();
                    PlaySound(gemHitGem);
                }
            }

            //Pared/Suelo
            else if (collision.gameObject.tag == "Map" || collision.gameObject.tag == "Floor")
            {
                PlaySound(gemHitMap);
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
                PlaySound(gemHitMap);
                isFalling = false;
            }

            //Si mientras cae le da a otro jugador le hace daño y se para
            else if(collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<Player>().Knockback(Vector3.zero, 0f);

                PlaySound(gemHitPlayer);

                rb.velocity = Vector3.zero;
                isFalling = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Si tiras una gema a un minecart la guardas y se suma el score al jugador
        if( (isBeingThrown || isCharged) && other.tag == "Minecart")
        {
            playerOwner.AddScore(this.value);
            StopThrowing();
            gemPool.ReturnObjectToPool(this.gameObject);
        }

        //Si le da a otro jugador por la espalda se mete en su bolsa si puede, si no lo aturde
        else if (isBeingThrown && !isCharged && other.tag == "PlayerBack")
        {
            //Debug.Log("Espalda");
            PlayerBack playerBack = other.gameObject.GetComponent<PlayerBack>();

            if (playerBack.TryAddGemToPouch(this))
            {
                PlaySound(gemPickup);
                StopThrowing();
                gameObject.SetActive(false);
            }
            else
            {
                PlaySound(gemHitPlayer);
                playerBack.Knockback(playerForward, knockbackForce);
            }
                
        }
    }

    //TODO Método de reset de la gema
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
    //public Color tierColor;
    public Material tierMaterial; 
}
