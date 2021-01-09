using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryController : MonoBehaviour
{
    public void GoToMainMenu()
    {
        if (GameManager.isLocalGame)
        {
            SceneLoader.instance.LoadMainMenuScene();
            var players = FindObjectsOfType<Player>();
            foreach(var player in players)
            {
                Destroy(player.gameObject);
            }
        } else
        {
            ClientInRoom.Exit();
        }
    }

    

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
