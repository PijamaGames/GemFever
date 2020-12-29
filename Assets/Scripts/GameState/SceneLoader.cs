using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public const string menuScene = "MainMenu";
    public const string signInScene = "SignIn";
    public const string signUpScene = "SignUp";
    public const string customizeAvatarScene = "CustomizeAvatar";
    public const string configScene = "Settings";
    public const string creditsScene = "Credits";
    public const string profileScene = "Profile";
    public const string howToPlayScene = "HowToPlay";
    public const string changeNameScene = "ChangeName";
    public const string changePasswordScene = "ChangePassword";
    public const string endGameScene = "EndGame";
    public const string shopScene = "Shop";
    public const string friendsScene = "Friends";
    public const string connectionScene = "Connection";
    public const string hubScene = "HUB";
    public const string hubSettingsScene = "HUBSettings";
    public const string lobbyScene = "Lobby";
    public const string eventViewScene = "EventView";

    //public const string gameScene = "GameScene";


    [SerializeField] int maxPreviousSceneMemory = 10;
    private static List<string> previousScenes = new List<string>();
    //private static string previousScene = "";

    private static string requestedScene = "";
    public static SceneLoader instance;

    private void Awake()
    {
        instance = this;
    }

    public void LoadMainMenuScene()
    {
        LoadScene(menuScene);
    }

    public void LoadEventViewScene()
    {
        LoadScene(eventViewScene);
    }

    public void LoadSingInScene()
    {
        LoadScene(signInScene);
    }

    public void LoadSignUpScene()
    {
        LoadScene(signUpScene);
    }
    public void LoadCustomizeAvatarScene()
    {
        LoadScene(customizeAvatarScene);
    }

    public void LoadConfigScene()
    {
        LoadScene(configScene);
    }

    public void LoadCreditsScene()
    {
        LoadScene(creditsScene);
    }
    public void LoadHowToPlayScene()
    {
        LoadScene(howToPlayScene);
    }
    public void LoadProfileScene()
    {
        LoadScene(profileScene);
    }
    
    public void LoadChangeNameScene()
    {
        LoadScene(changeNameScene);
    }

    public void LoadChangePasswordScene()
    {
        LoadScene(changePasswordScene);
    }
    public void LoadEndGameScene()
    {
        LoadScene(endGameScene);
    }
    public void LoadShopScene()
    {
        LoadScene(shopScene);
    }
    public void LoadFriendsScene()
    {
        LoadScene(friendsScene);
    }
    public void LoadConnectionScene()
    {
        LoadScene(connectionScene);
    }
    public void LoadHubScene()
    {
        LoadScene(hubScene);
    }
    public void LoadHubSettingsScene()
    {
        LoadScene(hubSettingsScene);
    }
    public void LoadLobbyScene()
    {
        LoadScene(lobbyScene);
    }
    /*public static void LoadUsernameScene()
    {
        LoadScene(usernameScene);
    }
    public static void LoadVictoryScene()
    {
        LoadScene(victoryScene);
    }
    public static void LoadLevelsScene()
    {
        LoadScene(levelsScene);
    }
    public static void LoadGameScene()
    {
        LoadScene(gameScene);
    }
    
    public static void LoadRankingScene()
    {
        LoadScene(rankingScene);
    }*/

    public void LoadPreviousScene()
    {
        int previousScenesCount = previousScenes.Count;
        if (previousScenesCount > 0)
        {
            string previousScene = previousScenes[previousScenesCount - 1];
            previousScenes.RemoveAt(previousScenesCount - 1);
            Debug.Log("PREVIOUS SCENE: " + previousScene);
            SceneManager.LoadScene(previousScene, LoadSceneMode.Single);
        } else
        {
            LoadMainMenuScene();
            Debug.Log("There was no previous scene so main menu was loaded");
        }
    }

    public void LoadRequestedScene()
    {
        SceneManager.LoadScene(requestedScene, LoadSceneMode.Single);
    }

    public static string GetCurrentScene()
    {
        return SceneManager.GetActiveScene().name;
    }

    private void LoadScene(string name)
    {
        string currentScene = GetCurrentScene();
        if (name == currentScene) return;
        previousScenes.Add(currentScene);
        if(previousScenes.Count > maxPreviousSceneMemory)
        {
            previousScenes.RemoveAt(0);
        }
        //previousScenes.Push(GetCurrentScene());
        //previousScenes.
        //previousScene = GetCurrentScene();
        /*if(UITransition.instance != null)
        {
            requestedScene = name;
            UITransition.instance.FadeOut();
        } else
        {*/
            SceneManager.LoadScene(name, LoadSceneMode.Single);
        /*}*/
    }

    public void ExitGame()
    {
        Debug.Log("Exit game");
        Application.Quit();
    }
}
