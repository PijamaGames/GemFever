using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider effectsSlider;
    [SerializeField] Toggle allowFriendsToggle;
    [SerializeField] Toggle allowInvitationsToggle;

    [SerializeField] Image languageImg;
    [SerializeField] Sprite englishBtn;
    [SerializeField] Sprite spanishBtn;

    private void Start()
    {
        musicSlider.value = GameManager.musicVolume;
        effectsSlider.value = GameManager.effectsVolume;
        UpdateLanguageImg();
    }

    public void UpdateLanguageImg()
    {
        languageImg.sprite = GameManager.english ? spanishBtn : englishBtn;
    }
}
