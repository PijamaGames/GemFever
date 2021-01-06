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
    [SerializeField] private Mesh pants1;
    [SerializeField] private Mesh pants2;
    [SerializeField] private Mesh shirt1;
    [SerializeField] private Mesh shirt2;
    [SerializeField] private Mesh hair1;
    [SerializeField] private Mesh hair2;
    [SerializeField] private Button back;
    [SerializeField] private Button next;

    private System.Random rnd = new System.Random();
    private Image[] faces;
    private Image[] skins;
    private Image[] colors;

    private void Awake()
    {
        skins = skinParent.GetComponentsInChildren<Image>();
        colors = colorParent.GetComponentsInChildren<Image>();
        faces= layoutFaces.GetComponentsInChildren<Image>();
        

        for (int i=0; i<skins.Length; i++)
        {
            skins[i].color = InicializeAvatarVariables.skinColors[i];
        }
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i].color = InicializeAvatarVariables.characterColors[i].colorShirt;
        }

        playerAvatar.SetUser(Client.user);
        InicializeAvatarVariables.shirts[0] = shirt1;
        InicializeAvatarVariables.shirts[1] = shirt2;
        InicializeAvatarVariables.pants[0] = pants1;
        InicializeAvatarVariables.pants[1] = pants2;
        InicializeAvatarVariables.hairs[0] = hair1;
        InicializeAvatarVariables.hairs[1] = hair2;
        playerAvatar.UpdateVisuals();
        SetFacesButtonsColor(0);

        body1Panel.SetActive(Client.user.avatar_bodyType==0);
        body2Panel.SetActive(Client.user.avatar_bodyType == 1);

        if (Client.state == Client.signedUpState)
        {
            next.gameObject.SetActive(true);
            next.onClick.AddListener(ClientSignedUp.SaveInfo);
            back.onClick.RemoveListener(ClientSignedIn.SaveInfo);
        }
        else
        {
            next.onClick.RemoveListener(ClientSignedUp.SaveInfo);
            back.onClick.AddListener(ClientSignedIn.SaveInfo);
            next.gameObject.SetActive(false);
        }
    }

    public void SetSelectedSkin(int id)
    {
        Client.user.avatar_skinTone = id;
        playerAvatar.UpdateVisuals();
        SetFacesButtonsColor(id);
    }

    private void SetFacesButtonsColor(int id)
    {
        foreach (var f in faces)
        {
            f.color = InicializeAvatarVariables.skinColors[id];
        }
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
        int randomFace= rnd.Next(FaceTextures.facesTextures.Count);
        int randomHat = rnd.Next(HatMeshes.hatsMeshes.Count);

        SetHat(HatMeshes.hatsForRandom[randomHat]);
        SetFace(FaceTextures.facesForRandom[randomFace]);
        ChangeBody(randomBody==1);
        SetSelectedColor(randomColor);
        SetSelectedSkin(randomSkin);
    }

    public void ChangeBody(bool body1)
    {
        Client.user.avatar_bodyType = body1 ? 0 : 1;
        InicializeAvatarVariables.body1Selected = body1;

        body1Panel.SetActive(body1); 
        body2Panel.SetActive(!body1);
        playerAvatar.UpdateVisuals();
    }

    public void SetFace(string id)
    {
        Client.user.avatar_face = id;
        playerAvatar.UpdateVisuals();
    }

    public void SetHat(string id)
    {
        Client.user.avatar_hat = id;
        playerAvatar.UpdateVisuals();
    }
    

}
