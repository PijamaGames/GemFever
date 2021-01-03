using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pickaxe : MonoBehaviour
{
    [SerializeField] Player playerOwner;
    [Space]

    BoxCollider [] boxColliders;

    [SerializeField] float hitCooldown = 0.5f;
    [SerializeField] float knockbackForce = 50f;
    [SerializeField] float gemParryForce = 50f;

    public bool hitOnCooldown = false;
    float pickaxeInput = 0f;

    AndroidInputs androidInputs;

    //Sound
    PersistentAudioSource audioSource;
    [SerializeField] AudioClip pickaxeSwing;
    [SerializeField] AudioClip pickaxeHitPlayer;
    [SerializeField] AudioClip parrySound;

    // Start is called before the first frame update
    void Start()
    {
        boxColliders = GetComponents<BoxCollider>();
        audioSource = FindObjectOfType<PersistentAudioSource>();

        //Si es el jugador local
        androidInputs = FindObjectOfType<AndroidInputs>();

        EnableCollisions(false);
    }

    public void PickaxeInput(InputAction.CallbackContext context)
    {
        if (!context.performed || !gameObject.scene.IsValid()) return;

        pickaxeInput = context.ReadValue<float>();

        PickaxeHit();
    }

    private void PickaxeHit()
    {
        
        if (!hitOnCooldown && pickaxeInput == 1)
        {
            //TODO solo si usa movil
            androidInputs.ResetPickaxeInput();

            hitOnCooldown = true;

            PlaySound(pickaxeSwing);

            playerOwner.StartPickaxeAnimation();
            StartCoroutine(HitCooldown());
        }
    }

    private void Update()
    {
        //TODO Si usa móvil
        pickaxeInput = androidInputs.GetPickaxeInput();
        PickaxeHit();

    }

    public void StartPickaxeHit()
    {
        EnableCollisions(true);
        //gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public void EndPickaxeHit()
    {
        EnableCollisions(false);
        //gameObject.GetComponent<MeshRenderer>().material.color = Color.red;

        //hitOnCooldown = false;

        //terminar animación
        playerOwner.EndPickaxeAnimation();
    }

    private void EnableCollisions(bool enabled)
    {
        foreach (BoxCollider boxCollider in boxColliders)
            boxCollider.enabled = enabled;
    }

    IEnumerator HitCooldown()
    {
        yield return new WaitForSecondsRealtime(hitCooldown);
        hitOnCooldown = false;
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.PlayEffect(clip);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Player playerHit = other.GetComponent<Player>();

            if (playerHit == playerOwner || playerHit.isInvulnerable) return;

            PlaySound(pickaxeHitPlayer);

            //Aplicar la fuerza hacia right si estás trepando una escalera
            playerHit.Knockback(playerOwner.transform.forward, knockbackForce);
            playerHit.DropGem(playerOwner.transform.forward);
        }

        else if (other.tag == "Gem")
        {
            Gem gem = other.GetComponent<Gem>();
            if(gem.isBeingThrown || gem.isFalling)
            {
                PlaySound(parrySound);
                gem.isCharged = true;
                gem.ParryGem(playerOwner.transform.forward, gemParryForce, playerOwner);
            }
        }
    }
}
