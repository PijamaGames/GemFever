using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] Player player;

    public void PlayWalkSound()
    {
        player.PlayWalkSound();
    }

    public void PlayLadderSound()
    {
        player.PlayLadderSound();
    }
}
