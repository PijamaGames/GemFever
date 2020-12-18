using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public static string username = "";
    public static List<float> bestTimes;

    public static void Init()
    {
        int size = LevelGenerator.levels.Count;
        bestTimes = new List<float>(size);
        for (int i = 0; i < size; i++) bestTimes.Add(999.9f);
    }

    public static void SaveHighScore(float score)
    {
        int level = Level.levelSelected-1;
        if(score < bestTimes[level])
        {
            bestTimes[level] = score;
            DatabaseManager.SaveUser();
        }
    }
}
