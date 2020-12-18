using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bilingual : MonoBehaviour
{
    TextMeshProUGUI text = null;
    [HideInInspector] public string spanishText = "";
    [SerializeField][TextArea] public string englishText = "";

    private void Start()
    {
        if (text == null)
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
        }
        spanishText = text.text;
        UpdateLanguage();
    }

    public void UpdateLanguage()
    {
        if(text == null)
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
        }
        text.text = GameManager.english ? englishText : spanishText;
    }

    public static void UpdateAll()
    {
        var bilinguals = FindObjectsOfType<Bilingual>();
        foreach(var b in bilinguals)
        {
            b.UpdateLanguage();
        }
    }
}
