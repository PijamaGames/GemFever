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

    private void Start()
    {
        musicSlider.value = GameManager.musicVolume;
        effectsSlider.value = GameManager.effectsVolume;

    }
}
