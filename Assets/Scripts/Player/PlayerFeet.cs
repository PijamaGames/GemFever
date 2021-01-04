using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeet : MonoBehaviour
{
    [SerializeField] Player player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ladder")
        {
            Debug.Log("En escalera");
            player.touchingTheGround = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ladder")
        {
            Debug.Log("No en escalera");
            player.touchingTheGround = true;
        }
    }
}
