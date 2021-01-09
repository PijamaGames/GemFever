using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatTextures : MonoBehaviour
{
    [SerializeField] private Texture[] hats;

    public static Dictionary<string,Texture> hatsTextures;
    private static HatTextures instance;

    void Awake()
    {
        hatsTextures = new Dictionary<string, Texture>();
        if (instance == null)
        {
            instance = this;
            int i = 0;
            foreach (var hat in hats)
            {
                hatsTextures.Add(hat.name, hat);
                i++;
            }

        }
        
    }
    
}
