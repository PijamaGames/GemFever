using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateHatButtons : MonoBehaviour
{
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Transform hatsContainer;
    [SerializeField] private CustomizeAvatarController customizeAvatarController;
    HatButton[] hatButtons;

    private int maxHats;
    void Start()
    {
        customizeAvatarController = FindObjectOfType<CustomizeAvatarController>();
        
        maxHats = Client.user.items_hats.Length;
        hatButtons = new HatButton[maxHats];
        for (int i = 0; i < maxHats; i++)
        {
            string hatName=Client.user.items_hats[i];
            if (HatMeshes.hatsMeshes.ContainsKey(hatName))
            {
                hatButtons[i] = Instantiate(buttonPrefab, hatsContainer).GetComponent<HatButton>();
                hatButtons[i].UpdateVisuals(HatMeshes.hatsMeshes[hatName], customizeAvatarController);
            }
            
        }
    }

}
