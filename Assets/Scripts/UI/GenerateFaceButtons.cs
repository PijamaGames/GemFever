using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFaceButtons : MonoBehaviour
{
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Transform facesContainer;
    [SerializeField] private CustomizeAvatarController customizeAvatarController;
    FaceButton[] faceButtons;

    private int maxFaces;
    void Start()
    {
        customizeAvatarController = FindObjectOfType<CustomizeAvatarController>();
        
        maxFaces = Client.user.items_faces.Length;
        faceButtons = new FaceButton[maxFaces];
        for (int i = 0; i < maxFaces; i++)
        {
            string faceName=Client.user.items_faces[i];
            faceButtons[i] = Instantiate(buttonPrefab, facesContainer).GetComponent<FaceButton>();
            faceButtons[i].UpdateVisuals(FaceTextures.facesTextures[faceName], customizeAvatarController);
        }
    }

}
