using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InicializeAvatarVariables:MonoBehaviour
{
    public static Mesh[] pants;
    public static Mesh[] shirts;
    public static Mesh[] hairs;
    public static Color[] skinColors = { new Color(1f, 0.8789797f, 0.5707547f), new Color(255 / 255f, 216 / 255f, 177 / 255f), new Color(170 / 255f, 127 / 255f, 82 / 255f), new Color(125 / 255f, 83 / 255f, 42 / 255f), new Color(75 / 255f, 44 / 255f, 13 / 255f) };
    public static bool body1Selected=true;
    public static int numBodies = 2;
    private int numColors = 6;
    public static CharacterColors[] characterColors;

    [SerializeField] private Mesh pants1;
    [SerializeField] private Mesh pants2;
    [SerializeField] private Mesh shirt1;
    [SerializeField] private Mesh shirt2;
    [SerializeField] private Mesh hair1;
    [SerializeField] private Mesh hair2;

    public static List<string> randHatList;
    public static List<string> randFaceList;

    private void Awake()
    {
        SetColors();
        shirts = new Mesh[numBodies];
        hairs = new Mesh[numBodies];
        pants = new Mesh[numBodies];

        shirts[0] = shirt1;
        shirts[1] = shirt2;
        pants[0] = pants1;
        pants[1] = pants2;
        hairs[0] = hair1;
        hairs[1] = hair2;
    }
    public static void PrepareHatList()
    {
        randHatList = new List<string>(Client.user.items_hats);
        randHatList.RemoveAll((x) => !HatMeshes.hatsMeshes.ContainsKey(x));
    }
    public static void PrepareFaceList()
    {
        randFaceList = new List<string>(Client.user.items_faces);
        randFaceList.RemoveAll((x) => !FaceTextures.facesTextures.ContainsKey(x));
    }
    private void SetColors()
    {
        characterColors = new CharacterColors[numColors];
        characterColors[0] = new CharacterColors(new Color(0.5882f, 0.733f, 1f), new Color(0.8196f, 0.69f, 0.4706f), new Color(0.3608f, 0.498f, 0.7451f));
        characterColors[1] = new CharacterColors(new Color(0.9176f, 0.4392f, 0.5255f), new Color(0.3804f, 0.6588f, 0.7686f), new Color(0.7176f, 0.1373f, 0.2392f));
        characterColors[2] = new CharacterColors(new Color(0.498f, 0.8275f, 0.5686f), new Color(0.4745f, 0.3255f, 0.2627f), new Color(0.2f, 0.5373f, 0.2745f));
        characterColors[3] = new CharacterColors(new Color(1.0f, 0.9412f, 0.5843f), new Color(0.3569f, 0.2941f, 0.498f), new Color(0.9176f, 0.8078f, 0.2392f));
        characterColors[4] = new CharacterColors(new Color(0.6902f, 0.5216f, 0.9961f), new Color(0.2118f, 0.1647f, 0.498f), new Color(0.5216f, 0.2863f, 0.9569f));
        characterColors[5] = new CharacterColors(new Color(1.0f, 0.8471f, 0.9804f), new Color(0.1882f, 0.4471f, 0.2471f), new Color(0.9725f, 0.6157f, 0.9333f));
    }

}
