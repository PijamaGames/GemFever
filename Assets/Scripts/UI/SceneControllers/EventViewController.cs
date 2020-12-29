using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventViewController : MonoBehaviour
{
    [SerializeField] Bilingual text;

    private void Start()
    {
        text.spanishText = ClientSignedIn.spanishMsg;
        text.englishText = ClientSignedIn.englishMsg;
        text.UpdateLanguage();
    }
}
