using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBack : MonoBehaviour
{
    [SerializeField] Player playerOwner;

    public void Knockback(Vector3 knockbackDirection, float knockbackForce)
    {
        playerOwner.Knockback(knockbackDirection, knockbackForce);
    }

    public bool TryAddGemToPouch(Gem gem)
    {
        return playerOwner.TryAddGemToPouch(gem);
    }
}
