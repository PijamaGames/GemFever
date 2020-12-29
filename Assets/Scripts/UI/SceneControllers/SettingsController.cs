using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] GameObject touchControlsButton;

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider effectsSlider;
    [SerializeField] Toggle allowRequestsToggle;
    [SerializeField] Toggle allowInvitationsToggle;

    [SerializeField] Image languageImg;
    [SerializeField] Sprite englishSprite;
    [SerializeField] Sprite spanishSprite;

    [SerializeField] Button exitSettingsBtn;


    private void Start()
    {
        
        if(musicSlider != null) musicSlider.value = GameManager.musicVolume;
        if(effectsSlider != null) effectsSlider.value = GameManager.effectsVolume;
        UpdateLanguageImg();

        if (!GameManager.isHandheld)
        {
            if(touchControlsButton != null) touchControlsButton.SetActive(false);
        }

        if (exitSettingsBtn)
        {
            exitSettingsBtn.onClick.AddListener(() => ClientSignedIn.SaveInfo());
        }

        if(Client.user != null)
        {
            if (allowInvitationsToggle != null)
            {
                allowInvitationsToggle.isOn = Client.user.allowInvitations;
            }
            if (allowRequestsToggle != null)
            {
                allowRequestsToggle.isOn = Client.user.allowRequests;
            }
        }
        
    }

    public void UpdateLanguageImg()
    {
        languageImg.sprite = GameManager.english ? spanishSprite : englishSprite;
    }
}
