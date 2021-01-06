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
    [SerializeField] private Mesh hair1;
    [SerializeField] private Mesh hair2;

    private Image[] skins;
    private Image[] colors;

    public static Mesh pants;
    public static Mesh shirt;
    public static Mesh hair;

    public static Color[] skinColors={ new Color(1f, 0.8789797f, 0.5707547f), new Color(255/255f,216/255f,177/255f), new Color(170/255f,127/255f,82/255f),new Color(125/255f,83/255f,42/255f), new Color(75/255f,44/255f,13/255f)};

    private System.Random rnd =new System.Random();
    public static bool body1Selected;

    private Image[] faces;
    public static int numBodies=2;


    public static CharacterColors[] characterColors;

    private void Awake()
    {
        skins = skinParent.GetComponentsInChildren<Image>();
        colors = colorParent.GetComponentsInChildren<Image>();
        faces= layoutFaces.GetComponentsInChildren<Image>();

        SetColors();

        for (int i=0; i<skins.Length; i++)
        {
            skins[i].color = skinColors[i];
        }
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i].color = characterColors[i].colorShirt;
        }

        playerAvatar.SetUser(Client.user);
        shirt = shirt1;
        pants = pants1;
        hair = hair1;
        playerAvatar.UpdateVisuals();
        SetFacesButtonsColor(0);
    }

    private void SetColors()
    {
        characterColors = new CharacterColors[colors.Length];
        characterColors[0] = new CharacterColors(new Color(0.5882f, 0.733f, 1f), new Color(0.8196f, 0.69f, 0.4706f), new Color(0.3608f, 0.498f, 0.7451f));
        characterColors[1] = new CharacterColors(new Color(0.9176f, 0.4392f, 0.5255f), new Color(0.3804f, 0.6588f, 0.7686f), new Color(0.7176f, 0.1373f, 0.2392f));
        characterColors[2] = new CharacterColors(new Color(0.498f, 0.8275f, 0.5686f), new Color(0.4745f, 0.3255f, 0.2627f), new Color(0.2f, 0.5373f, 0.2745f));
        characterColors[3] = new CharacterColors(new Color(1.0f, 0.9412f, 0.5843f), new Color(0.3569f, 0.2941f, 0.498f), new Color(0.9176f, 0.8078f, 0.2392f));
        characterColors[4] = new CharacterColors(new Color(0.6902f, 0.5216f, 0.9961f), new Color(0.2118f, 0.1647f, 0.498f), new Color(0.5216f, 0.2863f, 0.9569f));
        characterColors[5] = new CharacterColors(new Color(1.0f, 0.8471f, 0.9804f), new Color(0.1882f, 0.4471f, 0.2471f), new Color(0.9725f, 0.6157f, 0.9333f));
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
        int randomColor= rnd.Next(characterColors.Length);
        int randomSkin= rnd.Next(skinColors.Length);
        int randomBody= rnd.Next(numBodies);
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
        Client.user.avatar_bodyType = body1 ? 1 : 2;
        body1Selected = body1;

        shirt = body1 ? shirt1 : shirt2;
        pants = body1 ? pants1 : pants2;
        hair = body1 ? hair1 : hair2;

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
