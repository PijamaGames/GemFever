using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HatButton : MonoBehaviour
{
    private Button btn;
    private RawImage hatImg;
    private PersistentAudioSource persistentAudioSource;
    [SerializeField] private AudioClip buttonSound;

    void Awake()
    {
        btn = GetComponent<Button>();
        hatImg = GetComponent<RawImage>();
        persistentAudioSource = FindObjectOfType<PersistentAudioSource>();
        btn.onClick.AddListener(()=>persistentAudioSource.PlayEffect(buttonSound));
    }

    public void UpdateVisuals(SkinnedMeshRenderer mesh, CustomizeAvatarController customizeAvatarController)
    {
        
        hatImg.texture=HatTextures.hatsTextures[mesh.name];
        btn.onClick.AddListener(() =>
        {
            customizeAvatarController.SetHat(hatImg.texture);
        });
    }


}
