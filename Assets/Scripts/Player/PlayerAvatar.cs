using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    User user;
    private System.Random rnd = new System.Random();

    [HideInInspector] Material skinMat;
    [SerializeField] SkinnedMeshRenderer skinRenderer;

    [HideInInspector] Material shirtMat;
    [SerializeField] SkinnedMeshRenderer shirtRenderer;

    [HideInInspector] Material pantsMat;
    [SerializeField] SkinnedMeshRenderer pantsRenderer;

    [HideInInspector] Material hatMat;
    [SerializeField] SkinnedMeshRenderer hatRenderer;

    [HideInInspector] Material faceMat;
    [SerializeField] SkinnedMeshRenderer faceRenderer;
    
    [HideInInspector] Material scarfMat;
    [SerializeField] MeshRenderer scarfRenderer;

    private int randomColor;
    private int randomSkin;
    private int randomFace;
    private int randomHat;

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
        
        scarfMat = Instantiate(scarfRenderer.sharedMaterial);
        scarfRenderer.sharedMaterial = scarfMat;

        
    }

    public void SetUser(User user)
    {
        this.user = user;
    }

    private void Random()
    {
        randomColor = rnd.Next(CustomizeAvatarController.characterColors.Length);
        randomSkin = rnd.Next(CustomizeAvatarController.skinColors.Length);
        randomFace= rnd.Next(FaceTextures.facesTextures.Count);
        randomHat= rnd.Next(HatMeshes.hatsMeshes.Count);
    }

    public void UpdateVisuals()
    {
        int skinId;
        int colorId;
        string faceId;
        string hatId;

        if (user == null)
        {
            Random();
            skinId = randomSkin;
            colorId = randomColor;
            faceId = FaceTextures.facesForRandom[randomFace];
            hatId = HatMeshes.hatsForRandom[randomHat];
        }
        else
        {
            skinId = user.avatar_skinTone;
            colorId = user.avatar_color;

            if (user.avatar_hat == "")
                hatId = HatMeshes.hatsForRandom[0];
            else
                hatId = user.avatar_hat;

            if (user.avatar_face=="")
                faceId=FaceTextures.facesForRandom[0]; 
            else    
                faceId = user.avatar_face;
        }

        shirtRenderer.sharedMesh = CustomizeAvatarController.shirt;
        pantsRenderer.sharedMesh = CustomizeAvatarController.pants;
        //hatRenderer.sharedMesh = HatMeshes.hatsMeshes[hatId].sharedMesh;

        faceMat.SetTexture("_BaseMap", FaceTextures.facesTextures[faceId]);
        skinMat.SetColor("Color_398EEC7D", CustomizeAvatarController.skinColors[skinId]);
        pantsMat.SetColor("Color_398EEC7D", CustomizeAvatarController.characterColors[colorId].colorPants);
        shirtMat.SetColor("Color_398EEC7D", CustomizeAvatarController.characterColors[colorId].colorShirt);
        scarfMat.SetColor("Color_398EEC7D", CustomizeAvatarController.characterColors[colorId].colorScarf);
        //hatMat.SetTexture("_BaseMap", HatMeshes.hatsMeshes[hatId].material.mainTexture);

    }
}
