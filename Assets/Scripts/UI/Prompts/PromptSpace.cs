using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PromptSpace : MonoBehaviour
{
    [SerializeField] UnityEvent onPressed;
    [SerializeField] string spanishText;
    [SerializeField] string englishText;
    [SerializeField] Sprite sprite;
    [SerializeField] bool hostOnly = false;
    [SerializeField] bool requiresMoreThanOnePlayerInHub = false;
    HashSet<Prompt> promptTargets = new HashSet<Prompt>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("Player prompt");

            var userInfo = other.gameObject.GetComponent<PlayerAvatar>().userInfo;

            if (!GameManager.isLocalGame)
            {
                if (hostOnly && !(GameManager.isHost && userInfo.isHost)) return;
                if (requiresMoreThanOnePlayerInHub && ClientInRoom.players.Count <= 1) return;

                if (userInfo.id != Client.user.id) return;
            }

            //TODO: Detectar tipo de input para cargar el sprite adecuado
            Prompt prompt = other.gameObject.GetComponentInChildren<Prompt>();
            promptTargets.Add(prompt);
            Button btn = PromptsManager.RequestPrompt();
            if(btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => onPressed.Invoke());
                prompt.SetButton(btn);
                Bilingual b = prompt.btn.GetComponentInChildren<Bilingual>();
                b.spanishText = spanishText;
                b.englishText = englishText;
                b.UpdateLanguage();
                Image img = prompt.btn.GetComponentsInChildren<Image>()[1];
                img.enabled = sprite != null;
                img.sprite = sprite;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            var userInfo = other.gameObject.GetComponent<PlayerAvatar>().userInfo;

            if (!GameManager.isLocalGame)
            {
                if (hostOnly && !(GameManager.isHost && userInfo.isHost)) return;
                if (requiresMoreThanOnePlayerInHub && ClientInRoom.players.Count <= 1) return;

                if (userInfo.id != Client.user.id) return;
            }

            Prompt prompt = other.gameObject.GetComponentInChildren<Prompt>();
            promptTargets.Remove(prompt);
            if(prompt.btn != null)
            {
                PromptsManager.ReleasePrompt(prompt.btn);
            }
            prompt.btn = null;
        }
        
    }
}
