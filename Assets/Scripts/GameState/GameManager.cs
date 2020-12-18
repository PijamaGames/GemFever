using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static float musicVolume = 0.5f;
    public static float effectsVolume = 0.5f;
    public static bool english = false;

    private static bool firstInstance = true;
    private static string saveFilePath;

    private void Awake()
    {
        if (firstInstance)
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            //User.Init();
            /*bool success = DatabaseManager.Connect();
            Debug.Log("CONNECTION TO DATABASE: " + success);*/
            //CheckPreferencesFile();
        }
    }

    private void Start()
    {
        if (firstInstance)
        {
            firstInstance = false;
        }
    }
    //PUBLIC FUNCTIONS

    public static void ChangeUsername(string newUsername)
    {
        Debug.Log("Username changed: " + newUsername);
        User.username = newUsername;
        //username = newUsername;
    }

    public static void OnMusicVolumeChanged(float volume)
    {
        musicVolume = volume;
        AudioRegulator.UpdateAllVolumes();
    }

    public static void OnEffectsVolumeChanged(float volume)
    {
        effectsVolume = volume;
        AudioRegulator.UpdateAllVolumes();
    }

    
    public static void ChangeLanguage()
    {
        english = !english;
        Bilingual.UpdateAll();
    }

    private static void CheckPreferencesFile()
    {
        saveFilePath = Application.persistentDataPath + "/preferences.txt";
        Debug.Log("Persistent save file path: " + saveFilePath);
        if (!File.Exists(saveFilePath))
        {
            //File.Create(saveFilePath);
            SavePreferences();
        } else
        {
            ReadPreferences();
            if(User.username.Trim(' ') != "" && SceneLoader.GetCurrentScene() == SceneLoader.usernameScene)
            {
                //DatabaseManager.TryCreateUser();
                SceneLoader.LoadMainMenuScene();
            }
        }
    }

    private static void ReadPreferences()
    {
        string[] allLines = File.ReadAllLines(saveFilePath);
        if(allLines.Length > 0)
        {
            User.username = allLines[0];
            english = bool.Parse(allLines[2]);
            effectsVolume = float.Parse(allLines[3]);
            musicVolume = float.Parse(allLines[4]);
        }
    }

    public static void SavePreferences()
    {
        List<string> allLines = new List<string>();

        allLines.Add("" + User.username);
        allLines.Add("" + english);
        allLines.Add("" + effectsVolume);
        allLines.Add("" + musicVolume);

        File.WriteAllLines(saveFilePath, allLines);
    }
}
