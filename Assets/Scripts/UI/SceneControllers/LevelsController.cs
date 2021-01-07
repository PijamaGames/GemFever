using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsController : MonoBehaviour
{
    private bool firstClick = true;
    [SerializeField] private Button next;

    private int levelSelected = 0;

    void Start()
    {
        next.gameObject.SetActive(false);
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

    public void NextScene()
    {
        GameManager.levelId = levelSelected;
        ClientSignedIn.CreateRoom();
    }
}
