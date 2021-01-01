using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{

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

    private void Start()
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

    public void UpdateVisuals(User user)
    {
        skinMat.SetColor("Color_398EEC7D", CustomizeAvatarController.skinColors[user.avatar_skinTone]);
    }


}
