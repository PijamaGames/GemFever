using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceButton : MonoBehaviour
{
    private Button btn;
    private RawImage image;
    private Image face;
    
    void Awake()
    {
        btn = GetComponent<Button>();
        face = GetComponent<Image>();
        image = btn.GetComponentInChildren<RawImage>();

        
    }

    public void UpdateVisuals(Texture text, CustomizeAvatarController customizeAvatarController)
    {
        image.texture=text;
        btn.onClick.AddListener(() =>
        {
            customizeAvatarController.SetFace(text);
        });
        face.color = InicializeAvatarVariables.skinColors[Client.user.avatar_skinTone];
    }

    private void Update()
    {
        if(face) face.color = InicializeAvatarVariables.skinColors[Client.user.avatar_skinTone];
    }


}
