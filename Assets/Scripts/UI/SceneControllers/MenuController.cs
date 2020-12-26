using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] Bilingual welcomeText;
    [SerializeField] Button exitButton;

    private void Start()
    {
        if(Client.user != null)
        {
            welcomeText.spanishText = "¡Bienvenido " + Client.user.id + "!";
            welcomeText.englishText = "Welcome " + Client.user.id + "!";
            welcomeText.UpdateLanguage();
        }
        //ClientSignedIn.signedOutEvent += ()=>
        exitButton.onClick.AddListener(() => ClientSignedIn.TrySignOut());
    }
}
