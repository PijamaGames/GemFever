using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrompFunctions : MonoBehaviour
{
    public void ExitHub()
    {
        ClientInRoom.Exit();
    }

    public void EnterLevel()
    {
        Player[] players = FindObjectsOfType<Player>();

        foreach(Player player in players)
        {
            player.Reset();
        }

        ClientInRoom.GoToGameScene();
    }
}
