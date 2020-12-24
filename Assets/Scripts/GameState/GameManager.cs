using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static float musicVolume = 0.5f;
    public static float effectsVolume = 0.5f;
    public static bool english = false;
    public static string username = "";

    private static bool firstInstance = true;
    public static GameManager instance = null;
    //private static string saveFilePath;

    private void Awake()
    {
        if (firstInstance)
        {
            instance = this;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            ReadPreferences();
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

    public void OnMusicVolumeChanged(float volume)
    {
        musicVolume = volume;
        AudioRegulator.UpdateAllVolumes();
    }

    public void OnEffectsVolumeChanged(float volume)
    {
        effectsVolume = volume;
        AudioRegulator.UpdateAllVolumes();
    }

    
    public void ChangeLanguage()
    {
        english = !english;
        Bilingual.UpdateAll();
    }

    private void ReadPreferences()
    {
        if (PlayerPrefs.HasKey("id")) username = PlayerPrefs.GetString("id");
        if (PlayerPrefs.HasKey("musicVolume")) musicVolume = PlayerPrefs.GetFloat("musicVolume");
        if (PlayerPrefs.HasKey("effectsVolume")) effectsVolume = PlayerPrefs.GetFloat("effectsVolume");
        if (PlayerPrefs.HasKey("english")) english = bool.Parse(PlayerPrefs.GetString("english"));
    }

    public void SavePreferences()
    {
        PlayerPrefs.SetString("id", Client.user != null ? Client.user.id : "");
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("effectsVolume", effectsVolume);
        PlayerPrefs.SetString("english", english.ToString());
    }
}
