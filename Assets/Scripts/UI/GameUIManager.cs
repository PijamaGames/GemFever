﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] List<TMP_Text> playerScores = new List<TMP_Text>();
    [SerializeField] TMP_Text timer;
    [Space]

    [SerializeField] float goalTime = 120f;
    float currentSeconds = 0f;

    private void Awake()
    {
        foreach(TMP_Text text in playerScores)
        {
            text.enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartTimer();
    }

    public void StartTimer()
    {
        StartCoroutine(Timer());
    }

    void UpdateTimer()
    {
        int minutes = 0;
        int seconds = 0;

        int timeRemaining = (int) (goalTime - currentSeconds);

        minutes = timeRemaining / 60;
        seconds = timeRemaining - 60 * minutes;

        string minutesString = "";
        string secondsString = "";

        minutesString = minutes.ToString();

        if (seconds < 10) secondsString = "0" + seconds;
        else secondsString = seconds.ToString();

        timer.text = minutesString + ":" + secondsString;
    }

    public void UpdatePlayerUI(int playerNumber, int score, string playerName)
    {
        if (!GameManager.isLocalGame)
            playerScores[playerNumber - 1].text = playerName + "\n" + score;
        else
        {
            playerScores[playerNumber - 1].text = playerName + "" + playerNumber + ": " + score;
        }
    }

    public void ActivatePlayerUI(int playerNumber, string playerName)
    {
        playerScores[playerNumber - 1].enabled = true;

        if(!GameManager.isLocalGame)
            playerScores[playerNumber - 1].text = playerName + "\n" + "0";
        else
        {
            playerScores[playerNumber - 1].text = playerName + "" + playerNumber + " :0";
        }

        //TODO cambiar lo de P número al nombre de usuario y debajo las gemas que tiene
    }

    IEnumerator Timer()
    {
        yield return new WaitForSecondsRealtime(1f);
        currentSeconds++;
        UpdateTimer();
        CheckTimeUp();
    }

    void CheckTimeUp()
    {
        if (currentSeconds >= goalTime)
        {
            var players = FindObjectsOfType<Player>();
            foreach (var player in players)
                player.Freeze();

            //Online game
            if(!GameManager.isLocalGame)
            {

                ClientInRoom.GoToVictoryScene();
            }
            //Local Game
            else
            {
                SceneLoader.instance.LoadVictoryScene();
            }            
        }
        else StartCoroutine(Timer());
    }
}
