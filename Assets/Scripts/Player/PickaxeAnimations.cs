using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeAnimations : MonoBehaviour
{
    [SerializeField] Pickaxe pickaxe;
   
    public void StartPickaxeHit()
    {
        pickaxe.StartPickaxeHit();
    }

    public void EndPickaxeHit()
    {
        pickaxe.EndPickaxeHit();
    }
}
