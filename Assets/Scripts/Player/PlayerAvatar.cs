using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    User user;
    private System.Random rnd = new System.Random();

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

    private int randomColor;
    private int randomSkin;

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

    private void Random()
    {
        randomColor = rnd.Next(CustomizeAvatarController.favColors.Length);
        randomSkin = rnd.Next(CustomizeAvatarController.skinColors.Length);
    }

    public void UpdateVisuals()
    {
        int skinId;
        int colorId;

        if (user == null)
        {
            Random();
            skinId = randomSkin;
            colorId = randomColor;
        }
        else
        {
            skinId = user.avatar_skinTone;
            colorId = user.avatar_color;
        }
            
        skinMat.SetColor("Color_398EEC7D", CustomizeAvatarController.skinColors[skinId]);
        pantsMat.SetColor("Color_398EEC7D", CustomizeAvatarController.favColors[colorId]);
        shirtMat.SetColor("Color_398EEC7D", CustomizeAvatarController.favColors[colorId]);
        hatMat.SetColor("Color_398EEC7D", CustomizeAvatarController.favColors[colorId]);
    }
}
