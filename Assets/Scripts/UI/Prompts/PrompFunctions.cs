using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrompFunctions : MonoBehaviour
{
    public void ExitHub()
    {
        //Online game
        if(!GameManager.isLocalGame)
        {
            ClientInRoom.Exit();
        }
        //Local game
        else
        {
            Player[] players = FindObjectsOfType<Player>();

            foreach (Player player in players)
            {
                Destroy(player.gameObject);
            }

            SceneLoader.instance.LoadMainMenuScene();
        }
            
    }

    public void EnterLevel()
    {
        Player[] players = FindObjectsOfType<Player>();

        foreach(Player player in players)
        {
            player.Reset();
        }

        //Online game
        if(!GameManager.isLocalGame)
        {
            ClientInRoom.GoToGameScene();
        }
        //Local game
        else
        {
            SceneLoader.instance.LoadGameScene();
        }

        
    }
}
