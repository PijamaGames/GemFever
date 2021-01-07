using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeButtonSkinTone : MonoBehaviour
{
    private Image face;
    void Start()
    {
        face = GetComponent<Image>();
        SetFacesButtonsColor();
    }

    private void SetFacesButtonsColor()
    {
        face.color = InicializeAvatarVariables.skinColors[Client.user.avatar_skinTone];
    }
}
