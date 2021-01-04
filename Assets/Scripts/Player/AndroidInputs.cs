using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidInputs : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject mobileDPAD, mobileButtons;
    [SerializeField] Player playerPrefab;
    Player spawnedPlayer = null;
    
    //Movement Inputs
    [HideInInspector] public int horizontal = 0;
    [HideInInspector] public int vertical = 0;

    //Pickaxe and gem inputs
    int pickaxe = 0;
    int throwGem = 0;

    private void Start()
    {
        if(GameManager.isHandheld)
        {
            mobileDPAD.SetActive(true);
            mobileButtons.SetActive(true);
        }
    }

    //Crear jugador al pulsar algo
    public void SpawnPlayer()
    {
        if (spawnedPlayer == null && PlayerSpawnerManager.isInHub)
        {
            Debug.Log("PlayerSpawned");
            spawnedPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        }
    }

    //Setters
    public void PressUp() { vertical = 1; }

    public void PressDown() { vertical = -1; }

    public void PressLeft() { horizontal = -1; }

    public void PressRight() { horizontal = 1; }

    public void PressPickaxe() { pickaxe = 1; }

    public void PressGemThrow() { throwGem = 1; }

    //Getters
    public Vector2 GetMovementInput() { return new Vector2(horizontal, vertical); }

    public int GetPickaxeInput() { return pickaxe; }

    public int GetThrowGemInput() { return throwGem; }

    //Reset Methods
    public void ResetMovementInputs()
    {
        horizontal = 0;
        vertical = 0;
    }

    public void ResetPickaxeInput() { pickaxe = 0; }

    public void ResetGemThrowInput() { throwGem = 0; }
}
