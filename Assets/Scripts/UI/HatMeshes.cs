using System.Collections;
using System.Collections.Generic;
using System.Net.Cache;
using UnityEngine;

public class HatMeshes : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer[] hats;

    public static Dictionary<string, SkinnedMeshRenderer> hatsMeshes;
    public static string[] hatsForRandom;
    private static HatMeshes instance;

    void Awake()
    {
        hatsForRandom = new string[hats.Length];
        hatsMeshes = new Dictionary<string, SkinnedMeshRenderer>();
        if (instance == null)
        {
            instance = this;
            int i = 0;
            foreach (var hat in hats)
            {
                hatsMeshes.Add(hat.name, hat);
                hatsForRandom[i] = hat.name;
                i++;
            }

        }

    }
}
