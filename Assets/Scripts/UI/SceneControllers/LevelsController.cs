using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsController : MonoBehaviour
{
    private bool firstClick = true;
    [SerializeField] private Button next;
    private System.Random rnd = new System.Random();
    private int numLevels = 5;
    private ButtonOutline outline;
    private bool firstRandom = true;

    private int levelSelected = 0;

    void Start()
    {
        next.gameObject.SetActive(false);
        outline = FindObjectOfType<ButtonOutline>();
        
    }

    public void LevelSelected(int id)
    {
        if (firstClick)
        {
            firstClick = false;
            next.gameObject.SetActive(true);

        }
        levelSelected = id;
    }

    public void RandomLevel()
    {
        levelSelected = rnd.Next(numLevels);
        outline.ChangeOutline(outline.buttons[levelSelected]);
        if (firstRandom)
        {
            firstRandom = false;
            next.gameObject.SetActive(true);

        }
    }

    public void NextScene()
    {
        GameManager.levelId = levelSelected;
        ClientSignedIn.CreateRoom();
    }
}
