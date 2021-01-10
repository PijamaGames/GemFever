using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class CustomizeAvatarController : MonoBehaviour
{
    [SerializeField] private PlayerAvatar playerAvatar;
    [SerializeField] GameObject skinParent;
    [SerializeField] GameObject colorParent;
    [SerializeField] private GameObject body1Panel;
    [SerializeField] private GameObject body2Panel;
    [SerializeField] private GameObject layoutFaces;
    
    [SerializeField] private Button back;
    [SerializeField] private Button next;

    private System.Random rnd = new System.Random();
    
    private Image[] skins;
    private Image[] colors;

    public List<string> randHatList;
    public List<string> randFaceList;

    private void Awake()
    {
        skins = skinParent.GetComponentsInChildren<Image>();
        colors = colorParent.GetComponentsInChildren<Image>();
        

        for (int i=0; i<skins.Length; i++)
        {
            skins[i].color = InicializeAvatarVariables.skinColors[i];
        }
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i].color = InicializeAvatarVariables.characterColors[i].colorShirt;
        }

        playerAvatar.SetUser(Client.user);

        PrepareHatList();
        PrepareFaceList();

        playerAvatar.UpdateVisuals();


        body1Panel.SetActive(Client.user.avatar_bodyType==0);
        body2Panel.SetActive(Client.user.avatar_bodyType == 1);

        if (Client.state == Client.signedUpState)
        {
            next.gameObject.SetActive(true);
            next.onClick.AddListener(ClientSignedUp.SaveInfo);
            back.onClick.RemoveListener(ClientSignedIn.SaveInfo);
            back.gameObject.SetActive(false);
        }
        else
        {
            back.gameObject.SetActive(true);
            next.onClick.RemoveListener(ClientSignedUp.SaveInfo);
            back.onClick.AddListener(ClientSignedIn.SaveInfo);
            next.gameObject.SetActive(false);
        }
    }

    public void SetSelectedSkin(int id)
    {
        Client.user.avatar_skinTone = id;
        playerAvatar.UpdateVisuals();
    }

    public void SetSelectedColor(int id)
    {
        Client.user.avatar_color = id;
        playerAvatar.UpdateVisuals();
    }

    public void Random()
    {
        int randomColor= rnd.Next(InicializeAvatarVariables.characterColors.Length);
        int randomSkin= rnd.Next(InicializeAvatarVariables.skinColors.Length);
        int randomBody= rnd.Next(InicializeAvatarVariables.numBodies);
        int randomFace= rnd.Next(randFaceList.Count);
        int randomHat = rnd.Next(randHatList.Count);

        SetCodeHat(randHatList[randomHat]);
        SetFaceCode(randFaceList[randomFace]);
        ChangeBody(randomBody==1);
        SetSelectedColor(randomColor);
        SetSelectedSkin(randomSkin);
    }

    public void PrepareHatList()
    {
        randHatList =new List<string>(Client.user.items_hats);
        randHatList.RemoveAll((x) => !HatMeshes.hatsMeshes.ContainsKey(x));
    }
    public void PrepareFaceList()
    {
        randFaceList = new List<string>(Client.user.items_faces);
        randFaceList.RemoveAll((x) => !FaceTextures.facesTextures.ContainsKey(x));
    }

    public void ChangeBody(bool body1)
    {
        Client.user.avatar_bodyType = body1 ? 0 : 1;
        InicializeAvatarVariables.body1Selected = body1;

        body1Panel.SetActive(body1); 
        body2Panel.SetActive(!body1);
        playerAvatar.UpdateVisuals();
    }

    private void SetFaceCode(String id)
    {
        Client.user.avatar_face = id;
        playerAvatar.UpdateVisuals();
    }

    public void SetFace(Texture text)
    {
        string id = text.name;
        Client.user.avatar_face = id;
        playerAvatar.UpdateVisuals();
    }

    private void SetCodeHat(string id)
    {
        Client.user.avatar_hat = id;
        playerAvatar.UpdateVisuals();
    }
    public void SetHat(Texture text)
    {
        string id = text.name;
        Client.user.avatar_hat = id;
        playerAvatar.UpdateVisuals();
    }


}
