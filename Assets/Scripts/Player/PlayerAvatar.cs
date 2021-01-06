using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    User user;
    UserInfo userInfo;
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

    [HideInInspector] Material hairMat;
    [SerializeField] SkinnedMeshRenderer hairRenderer;

    /*private int randomColor;
    private int randomSkin;
    private int randomFace;
    private int randomHat;*/
    bool materialsInitiated = false;

    private void Awake()
    {
        if (!materialsInitiated)
        {
            InitMaterials();
        }
        
    }

    private void InitMaterials()
    {
        materialsInitiated = true;

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

        hairMat = Instantiate(hairRenderer.sharedMaterial);
        hairRenderer.sharedMaterial = hairMat;
    }

    public void SetUser(User user)
    {
        this.user = user;
        userInfo = new UserInfo();
        userInfo.id = user.id;
        userInfo.isHost = false;
        userInfo.isClient = false;
        userInfo.bodyType = user.avatar_bodyType;
        userInfo.skinTone = user.avatar_skinTone;
        userInfo.color = user.avatar_color;
        userInfo.face = user.avatar_face;
        userInfo.hat = user.avatar_hat;
        userInfo.frame = user.avatar_frame;
        //UpdateVisuals();
    }

    public void SetUserInfo(UserInfo info)
    {
        userInfo = info;
        UpdateVisuals();
    }

    private void Random(out int randomColor,out int randomSkin,out int randomFace,out int randomHat)
    {
        randomColor = rnd.Next(CustomizeAvatarController.characterColors.Length);
        randomSkin = rnd.Next(CustomizeAvatarController.skinColors.Length);
        randomFace= rnd.Next(FaceTextures.facesTextures.Count);
        randomHat= rnd.Next(HatMeshes.hatsMeshes.Count);
    }

    public void UpdateVisuals()
    {
        if (user != null) SetUser(user);
        int skinId;
        int colorId;
        string faceId;
        string hatId;

        if (userInfo == null)
        {
            int randomSkin, randomColor, randomFace, randomHat;
            Random(out randomColor, out randomSkin, out randomFace, out randomHat);
            skinId = randomSkin;
            colorId = randomColor;
            faceId = FaceTextures.facesForRandom[randomFace];
            hatId = HatMeshes.hatsForRandom[randomHat];
        }
        else
        {
            skinId = userInfo.skinTone;
            colorId = userInfo.color;

            if (!HatMeshes.hatsMeshes.ContainsKey(userInfo.hat))
                hatId = HatMeshes.hatsForRandom[0];
            else
                hatId = userInfo.hat;

            if (!FaceTextures.facesTextures.ContainsKey(userInfo.face))
                faceId=FaceTextures.facesForRandom[0];
            else    
                faceId = userInfo.face;
        }

        if (!materialsInitiated) InitMaterials();

        shirtRenderer.sharedMesh = CustomizeAvatarController.shirt;
        pantsRenderer.sharedMesh = CustomizeAvatarController.pants;
        hairRenderer.sharedMesh = CustomizeAvatarController.hair;
        hatRenderer.sharedMesh = HatMeshes.hatsMeshes[hatId].sharedMesh;

        faceMat.SetTexture("_BaseMap", FaceTextures.facesTextures[faceId]);
        skinMat.SetColor("Color_398EEC7D", CustomizeAvatarController.skinColors[skinId]);
        pantsMat.SetColor("Color_398EEC7D", CustomizeAvatarController.characterColors[colorId].colorPants);
        shirtMat.SetColor("Color_398EEC7D", CustomizeAvatarController.characterColors[colorId].colorShirt);
        scarfMat.SetColor("Color_398EEC7D", CustomizeAvatarController.characterColors[colorId].colorScarf);
        //hatMat.SetTexture("Texture2D_E0F6099E", HatMeshes.hatsMeshes[hatId].sharedMaterial.mainTexture);

    }
}
