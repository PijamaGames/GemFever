using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
    public static float musicVolume = 0.5f;
    public static float effectsVolume = 0.5f;
    public static bool english = false;
    public static string username = "";

    public static int levelId=0;

    private static bool firstInstance = true;
    public static GameManager instance = null;

    public static bool isLocalGame = true;

    public static bool isHost = false;
    public static bool isClient = false;

    //private static string saveFilePath;

    [DllImport("__Internal")]
    private static extern bool IsHandheld();

    public bool debugMobile = false;
    public static bool isHandheld = false;

    [SerializeField] float blockedUIAlpha = 0.9f;

    private void Awake()
    {
        if (firstInstance)
        {
            instance = this;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            ReadPreferences();

#if (UNITY_WEBGL && !UNITY_EDITOR)
            isHandheld = IsHandheld();
#else
            isHandheld = debugMobile;
#endif


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
    public void SetIsLocalGame(bool isLocal)
    {
        isLocalGame = isLocal;
    }
    public void BlockUI()
    {
        CanvasGroup group = FindObjectOfType<CanvasGroup>();
        if(group != null)
        {
            group.blocksRaycasts = false;
            group.alpha = blockedUIAlpha;
        }
    }

    public void ReleaseUI()
    {
        CanvasGroup group = FindObjectOfType<CanvasGroup>();
        group.blocksRaycasts = true;
        group.alpha = 1f;
    }

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

    public void OnAllowInvitationsChanged(bool allow)
    {
        if (Client.user != null)
        {
            Client.user.allowInvitations = allow;
        }
    }

    public void OnAllowRequestsChanged(bool allow)
    {
        if(Client.user != null)
        {
            Client.user.allowRequests = allow;
        }
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
