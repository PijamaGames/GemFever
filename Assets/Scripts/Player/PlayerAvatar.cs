using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    User user;

    [HideInInspector] Material skinMat;
    [SerializeField] Renderer skinRenderer;

    [HideInInspector] Material shirtMat;
    [SerializeField] Renderer shirtRenderer;

    [HideInInspector] Material pantsMat;
    [SerializeField] Renderer pantsRenderer;

    [HideInInspector] Material hatMat;
    [SerializeField] Renderer hatRenderer;

    [HideInInspector] Material faceMat;
    [SerializeField] Renderer faceRenderer;

    private void Awake()
    {

        skinMat = Instantiate(skinRenderer.sharedMaterial);
        skinRenderer.sharedMaterial = skinMat;

        shirtMat = Instantiate(shirtRenderer.sharedMaterial);
        shirtRenderer.sharedMaterial = shirtMat;

        pantsMat = Instantiate(pantsRenderer.sharedMaterial);
        pantsRenderer.sharedMaterial = pantsMat;

        hatMat = Instantiate(hatRenderer.sharedMaterial);
        hatRenderer.sharedMaterial = hatMat;

        faceMat = Instantiate(faceRenderer.sharedMaterial);
        faceRenderer.sharedMaterial = faceMat;

    }

    public void SetUser(User user)
    {
        this.user = user;
    }

    public void UpdateVisuals()
    {
        if (user == null) return;
        skinMat.SetColor("Color_398EEC7D", CustomizeAvatarController.skinColors[user.avatar_skinTone]);
        pantsMat.SetColor("Color_398EEC7D", CustomizeAvatarController.favColors[user.avatar_color]);
        shirtMat.SetColor("Color_398EEC7D", CustomizeAvatarController.favColors[user.avatar_color]);
        hatMat.SetColor("Color_398EEC7D", CustomizeAvatarController.favColors[user.avatar_color]);
    }
}
