using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject errorPanel;
    [SerializeField] private PlayerAvatar playerAvatar;
    [SerializeField] Bilingual welcomeText;
    [SerializeField] Button exitButton;
    [SerializeField] private Image onlyPC;

    private Button local;

    private void Start()
    {
        if(Client.user != null)
        {
            errorPanel.SetActive(ClientInRoom.error >= 0);
            ClientInRoom.error = -1;
            welcomeText.spanishText = "¡Bienvenido/a " + Client.user.id + "!";
            welcomeText.englishText = "Welcome " + Client.user.id + "!";
            welcomeText.UpdateLanguage();
            playerAvatar.SetUser(Client.user);
            playerAvatar.UpdateVisuals();
            local = onlyPC.GetComponentInParent<Button>();


            onlyPC.gameObject.SetActive(GameManager.isHandheld);
            local.interactable=!GameManager.isHandheld;
            
            


        }
        //ClientSignedIn.signedOutEvent += ()=>
        exitButton.onClick.AddListener(() => ClientSignedIn.TrySignOut());
    }
}
