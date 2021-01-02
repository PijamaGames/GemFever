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
    private Image[] skins;
    private Image[] colors;

    public static Color[] skinColors={ new Color(1f, 0.8789797f, 0.5707547f), new Color(255/255f,216/255f,177/255f), new Color(170/255f,127/255f,82/255f),new Color(125/255f,83/255f,42/255f), new Color(75/255f,44/255f,13/255f)};
    public static Color[] favColors={Color.black, Color.blue, Color.cyan, Color.gray, Color.green, Color.magenta};

    private System.Random rnd =new System.Random();

    private void Start()
    {
        skins = skinParent.GetComponentsInChildren<Image>();
        colors = colorParent.GetComponentsInChildren<Image>();

        for (int i=0; i<skins.Length; i++)
        {
            skins[i].color = skinColors[i];
        }
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i].color = favColors[i];
        }

        playerAvatar.SetUser(Client.user);
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

    public void NextScene()
    {
        ClientSignedIn.SaveInfo();
    }

    public void Random()
    {
        int randomColor= rnd.Next(favColors.Length);
        int randomSkin= rnd.Next(skinColors.Length);

        SetSelectedColor(randomColor);
        SetSelectedSkin(randomSkin);
    }

}
