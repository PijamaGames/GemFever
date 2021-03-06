﻿using System.Collections;
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
    public bool pickaxeReset = false;
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

        androidInputs = FindObjectOfType<AndroidInputs>();

        EnableCollisions(false);
    }

    public void PickaxeInput(InputAction.CallbackContext context)
    {
        if (GameManager.isHandheld) return;

        if (playerOwner.freeze) return;

        if (!context.performed || !gameObject.scene.IsValid()) return;

        //Online game
        if (!GameManager.isLocalGame)
        {
            //Caso de las máquinas de clientes
            if (!GameManager.isHost)
            {
                //Manda input por red
                playerOwner.networkPlayer.inputInfo.pickaxeInput = context.ReadValue<float>();
            }

            else //Máquina host y jugador host
            {
                pickaxeInput = context.ReadValue<float>();

                PickaxeHit();
            }
        }
        //Offline game
        else
        {
            pickaxeInput = context.ReadValue<float>();

            PickaxeHit();
        }
    }

    private void PickaxeHit()
    {
        if (!hitOnCooldown && pickaxeInput == 1)
        {
            if (GameManager.isHandheld)
                androidInputs.ResetPickaxeInput();

            hitOnCooldown = true;

            pickaxeReset = true;

            PlaySound(pickaxeSwing);

            playerOwner.StartPickaxeAnimation();
            StartCoroutine(HitCooldown());
        }
    }

    private void Update()
    {
        MobileInputs();

        //Máquina del host, pero jugadores clientes
        if (!GameManager.isLocalGame)
        {
            if (GameManager.isHost && playerOwner.userInfo.isClient)
            {
                //Recibir input por red
                pickaxeInput = playerOwner.networkPlayer.inputInfo.pickaxeInput;
                PickaxeHit();
            }
        }
    }

    public void ResetPickaxe()
    {
        if (GameManager.isHandheld) androidInputs = null;

        pickaxeInput = 0f;

        hitOnCooldown = false;
        pickaxeReset = false;

        EnableCollisions(false);
    }

    private void MobileInputs()
    {
        if (GameManager.isHandheld)
        {
            if (androidInputs == null)
                androidInputs = FindObjectOfType<AndroidInputs>();

            //Online game
            if (!GameManager.isLocalGame)
            {
                //Caso de las máquinas del host
                if (!GameManager.isHost)
                {
                    //Manda input por red
                    playerOwner.networkPlayer.inputInfo.pickaxeInput = androidInputs.GetPickaxeInput();
                    PickaxeHit();
                }

                else //Máquina host y jugador host
                {
                    pickaxeInput = androidInputs.GetPickaxeInput();
                    PickaxeHit();
                }
            }
            //Local game
            else
            {
                pickaxeInput = androidInputs.GetPickaxeInput();
                PickaxeHit();
            }
        }
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
        pickaxeReset = false;
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

        if(!pickaxeReset)
            EndPickaxeHit();
    }

    private void PlaySound(AudioClip clip)
    {
        if(clip != null)
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
