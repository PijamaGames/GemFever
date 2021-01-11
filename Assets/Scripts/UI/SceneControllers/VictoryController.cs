using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryController : MonoBehaviour
{
    [SerializeField] Button playAgainBtn;
    [SerializeField] Button exitBtn;

    private void Start()
    {
        playAgainBtn.gameObject.SetActive(GameManager.isHost || GameManager.isLocalGame);
        playAgainBtn.onClick.AddListener(() => PlayAgain());
        exitBtn.onClick.AddListener(() => GoToMainMenu());
    }

    private void GoToMainMenu()
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

    private void PlayAgain()
    {
        var players = FindObjectsOfType<Player>();

        foreach (var player in players)
        {
            player.UnFreeze();
            player.Reset();
        }

        if (GameManager.isLocalGame)
        {
            SceneLoader.instance.LoadHubScene();
        }
        else if (GameManager.isHost)
        {
            PlayerSpawnerManager.firstTimeInHub = false;
            ClientInRoom.GoToHUBScene();
        }
    }
}
