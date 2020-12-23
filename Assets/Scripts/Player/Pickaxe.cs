using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pickaxe : MonoBehaviour
{
    [SerializeField] Player playerOwner;
    [Space]

    BoxCollider boxCollider;
    Animator animator;

    [SerializeField] float hitCooldown = 0.5f;
    [SerializeField] float knockbackForce = 50f;
    [SerializeField] float gemParryForce = 50f;

    bool hitOnCooldown = false;
    float pickaxeInput = 0f;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;

        animator = GetComponent<Animator>();
    }

    public void PickaxeInput(InputAction.CallbackContext context)
    {
        if (!context.performed || !gameObject.scene.IsValid()) return;

        pickaxeInput = context.ReadValue<float>();
        Debug.Log("Pickaxe input: " + pickaxeInput);

        if(!hitOnCooldown && pickaxeInput == 1)
        {
            hitOnCooldown = true;
            PickaxeHitAnimation();
        }
    }

    void PickaxeHitAnimation()
    {
        //Ejecutar animación de Hit
        //Esa animación llama en los frames deseados a Start y End Hit
        
        animator.SetBool("Hit", true);
    }

    public void StartPickaxeHit()
    {
        animator.SetBool("Hit", false);
        boxCollider.enabled = true;
        gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public void EndPickaxeHit()
    {
        boxCollider.enabled = false;
        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;

        StartCoroutine(HitCooldown());
    }

    IEnumerator HitCooldown()
    {
        yield return new WaitForSecondsRealtime(hitCooldown);
        hitOnCooldown = false;
        animator.ResetTrigger("Hit");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Player playerHit = other.GetComponent<Player>();

            if (playerHit == playerOwner || playerHit.isInvulnerable) return;

            //Aplicar la fuerza hacia right si estás trepando una escalera
            playerHit.Knockback(playerOwner.transform.forward, knockbackForce);
            playerHit.DropGem(playerOwner.transform.forward);
        }

        else if (other.tag == "Gem")
        {
            Gem gem = other.GetComponent<Gem>();
            if(gem.isBeingThrown || gem.isFalling)
            {
                Debug.Log("Parry");
                gem.isCharged = true;
                gem.ParryGem(playerOwner.transform.forward, gemParryForce, playerOwner);
            }
        }
    }
}
