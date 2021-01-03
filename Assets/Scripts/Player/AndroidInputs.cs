using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidInputs : MonoBehaviour
{
    // Start is called before the first frame update
    Player player;

    //Movement Inputs
    public int horizontal = 0;
    public int vertical = 0;

    //Pickaxe and gem inputs
    int pickaxe = 0;
    int throwGem = 0;

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
