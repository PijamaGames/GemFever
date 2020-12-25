using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] GameObject touchControlsButton;

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider effectsSlider;
    [SerializeField] Toggle allowFriendsToggle;
    [SerializeField] Toggle allowInvitationsToggle;

    [SerializeField] Image languageImg;
    [SerializeField] Sprite englishSprite;
    [SerializeField] Sprite spanishSprite;

    private void Start()
    {
        
        if(musicSlider != null) musicSlider.value = GameManager.musicVolume;
        if(effectsSlider != null) effectsSlider.value = GameManager.effectsVolume;
        UpdateLanguageImg();

        if (!GameManager.isHandheld)
        {
            if(touchControlsButton != null) touchControlsButton.SetActive(false);
        }
    }

    public void UpdateLanguageImg()
    {
        languageImg.sprite = GameManager.english ? spanishSprite : englishSprite;
    }
}
