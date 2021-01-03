using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTextures : MonoBehaviour
{
    [SerializeField] private Texture[] faces;

    public static Texture[] facesTextures;
    private static FaceTextures instance;

    void Awake()
    {
        facesTextures=new Texture[faces.Length];
        if (instance == null)
        {
            instance = this;
            for (int i = 0; i < faces.Length; i++)
            {
                facesTextures[i] = faces[i];
            }
        }
        
    }
    
}
