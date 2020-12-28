using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemAnimations : MonoBehaviour
{
    [SerializeField] Player player;

    public void EndGemAnimation()
    {
        Debug.Log("EndGemAnimation");

        player.EndGemAnimation();
    }
}
