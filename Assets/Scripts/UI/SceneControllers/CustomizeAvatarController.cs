using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UI;
using System;

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

    private Image[] skins;
    private Image[] colors;

    public static Mesh pants;
    public static Mesh shirt;

    public static Color[] skinColors={ new Color(1f, 0.8789797f, 0.5707547f), new Color(255/255f,216/255f,177/255f), new Color(170/255f,127/255f,82/255f),new Color(125/255f,83/255f,42/255f), new Color(75/255f,44/255f,13/255f)};
    public static Color[] favColors={Color.black, Color.blue, Color.cyan, Color.gray, Color.green, Color.magenta};

    private System.Random rnd =new System.Random();
    public static bool body1Selected;

    private Image[] faces;

    public static Texture faceTexture;

    private void Start()
    {
        skins = skinParent.GetComponentsInChildren<Image>();
        colors = colorParent.GetComponentsInChildren<Image>();
        faces= layoutFaces.GetComponentsInChildren<Image>();

        for (int i=0; i<skins.Length; i++)
        {
            skins[i].color = skinColors[i];
        }
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i].color = favColors[i];
        }

        playerAvatar.SetUser(Client.user);
        shirt = shirt1;
        pants = pants1;
        playerAvatar.UpdateVisuals();
        SetFacesButtonsColor(0);

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
            f.color = skinColors[id];
        }
    }

    public void SetSelectedColor(int id)
    {
        Client.user.avatar_color = id;
        playerAvatar.UpdateVisuals();
    }

    public void NextScene()
    {
        ClientSignedIn.SaveInfo();
    }

    public void Random()
    {
        int randomColor= rnd.Next(favColors.Length);
        int randomSkin= rnd.Next(skinColors.Length);
        int randomBody= rnd.Next(3);

        ChangeBody(randomBody==1);
        SetSelectedColor(randomColor);
        SetSelectedSkin(randomSkin);
    }

    public void ChangeBody(bool body1)
    {
        body1Selected = body1;

        shirt = body1 ? shirt1 : shirt2;
        pants = body1 ? pants1 : pants2;

        body1Panel.SetActive(body1); 
        body2Panel.SetActive(!body1);
        playerAvatar.UpdateVisuals();
    }

    public void SetFace(Texture text)
    {
        faceTexture=text;
        playerAvatar.UpdateVisuals();
    }

}
