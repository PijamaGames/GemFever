using System.Collections;
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

    public void UpdatePlayerUI(int playerNumber, int score)
    {
        playerScores[playerNumber - 1].text = "P" + playerNumber + ": " + score;
    }

    public void ActivatePlayerUI(int playerNumber)
    {
        playerScores[playerNumber - 1].enabled = true;
        playerScores[playerNumber - 1].text = "P" + playerNumber + ": 0";
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
            SceneLoader.instance.LoadVictoryScene();
        }
        else StartCoroutine(Timer());
    }
}
