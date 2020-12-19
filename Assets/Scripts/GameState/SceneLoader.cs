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
    //public const string victoryScene = "Victory"; 
    //public const string gameScene = "GameScene";
    //public const string usernameScene = "Username";
    //public const string rankingScene = "Ranking";
    public const string howToPlayScene = "HowToPlay";
    //public const string levelsScene = "Levels";


    private static string previousScene = "";

    private static string requestedScene = "";

    public void LoadMainMenuScene()
    {
        LoadScene(menuScene);
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
        if (!previousScene.Equals(""))
        {
            LoadScene(previousScene);
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
        previousScene = GetCurrentScene();
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
