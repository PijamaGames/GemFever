using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PromptSpace : MonoBehaviour
{
    [SerializeField] UnityEvent onPressed;
    [SerializeField] string text;
    [SerializeField] Sprite sprite;
    HashSet<Prompt> promptTargets = new HashSet<Prompt>();

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.isLocalGame)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                Prompt prompt = other.gameObject.GetComponentInChildren<Prompt>();
                promptTargets.Add(prompt);
                Button btn = PromptsManager.RequestPrompt();
                if(btn != null)
                {
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() => onPressed.Invoke());
                    prompt.SetButton(btn);
                    prompt.btn.GetComponentInChildren<TextMeshProUGUI>().text = text;
                    Image img = prompt.btn.GetComponentsInChildren<Image>()[1];
                    img.enabled = sprite != null;
                    img.sprite = sprite;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameManager.isLocalGame)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
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
}
