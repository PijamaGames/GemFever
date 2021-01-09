using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFaceButtons : MonoBehaviour
{
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Transform facesContainer;
    FaceButton[] faceButtons;
    private int maxFaces;
    void Start()
    {
        maxFaces = Client.user.avatar_face.Length;
        faceButtons = new FaceButton[maxFaces];
        for (int i = 0; i < maxFaces; i++)
        {
            faceButtons[i] = Instantiate(buttonPrefab, facesContainer).GetComponent<FaceButton>();
            faceButtons[i].UpdateVisuals(FaceTextures.facesTextures[FaceTextures.facesForRandom[i]]);
        }
    }

}
