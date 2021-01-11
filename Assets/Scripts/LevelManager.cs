using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<GameObject> levels = new List<GameObject>();ç

    private void Awake()
    {
        foreach (var level in levels)
            level.SetActive(false);

        levels[GameManager.levelId].SetActive(true);
    }
}
