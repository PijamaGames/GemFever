using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceButton : MonoBehaviour
{
    private Button btn;
    private Texture image;
    [SerializeField] private CustomizeAvatarController customizeAvatarController;
    
    void Start()
    {
        btn = GetComponent<Button>();
        image = btn.GetComponentInChildren<Texture>();
    }

    public void UpdateVisuals(Texture text)
    {
        image = text;
        btn.onClick.AddListener(() =>
        {
            customizeAvatarController.SetFace(text); 

        });
    }

}
