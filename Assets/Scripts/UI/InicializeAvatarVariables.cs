using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InicializeAvatarVariables:MonoBehaviour
{
    private Image[] skins;
    private Image[] colors;

    public static Mesh[] pants;
    public static Mesh[] shirts;
    public static Mesh[] hairs;

    public static Color[] skinColors = { new Color(1f, 0.8789797f, 0.5707547f), new Color(255 / 255f, 216 / 255f, 177 / 255f), new Color(170 / 255f, 127 / 255f, 82 / 255f), new Color(125 / 255f, 83 / 255f, 42 / 255f), new Color(75 / 255f, 44 / 255f, 13 / 255f) };

    private System.Random rnd = new System.Random();
    public static bool body1Selected;

    private Image[] faces;
    public static int numBodies = 2;


    private void Awake()
    {
        shirts = new Mesh[numBodies];
        hairs = new Mesh[numBodies];
        pants = new Mesh[numBodies];
    }

}
