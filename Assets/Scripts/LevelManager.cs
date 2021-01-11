using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<GameObject> levels = new List<GameObject>();
    [SerializeField] int debugID = 0;
    private void Awake()
    {
        foreach (var level in levels)
            level.SetActive(false);

        levels[/*GameManager.levelId*/ debugID].SetActive(true);
    }
}
