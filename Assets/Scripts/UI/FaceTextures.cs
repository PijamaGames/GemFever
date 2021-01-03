using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTextures : MonoBehaviour
{
    [SerializeField] private Texture[] faces;

    public static Dictionary<string,Texture> facesTextures;
    public static string[] facesForRandom;
    private static FaceTextures instance;

    void Awake()
    {
        facesForRandom=new string[faces.Length];
        facesTextures=new Dictionary<string, Texture>();
        if (instance == null)
        {
            instance = this;
            int i = 0;
            foreach (var face in faces)
            {
                facesTextures.Add(face.name, face);
                facesForRandom[i] = face.name;
                i++;
            }

        }
        
    }
    
}
