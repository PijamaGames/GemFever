using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PromptSpace : MonoBehaviour
{
    [SerializeField] UnityEvent onPressed;
    [SerializeField] string spanishText;
    [SerializeField] string englishText;

    [SerializeField] Sprite mobileSprite;
    [SerializeField] Sprite gamepadSprite;
    [SerializeField] Sprite keyboardPlayer1Sprite;
    [SerializeField] Sprite keyboardPlayer2Sprite;

    [SerializeField] bool hostOnly = false;
    [SerializeField] bool requiresMoreThanOnePlayerInHub = false;
    HashSet<Prompt> promptTargets = new HashSet<Prompt>();

    private void OnTriggerEnter(Collider other)
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

                if (GameManager.isHandheld)
                {
                    img.enabled = mobileSprite != null;

                    if(mobileSprite != null)
                        img.sprite = mobileSprite;
                }
                else
                {
                    PlayerInput playerInput = other.GetComponent<PlayerInput>();

                    switch (playerInput.currentControlScheme)
                    {
                        case "Gamepad":
                            img.enabled = gamepadSprite != null;
                            img.sprite = gamepadSprite;
                            break;
                        case "Keyboard&Mouse":
                            img.enabled = keyboardPlayer1Sprite != null;
                            img.sprite = keyboardPlayer1Sprite;
                            break;
                        case "VirtualKeyboard":
                            img.enabled = keyboardPlayer2Sprite != null;
                            img.sprite = keyboardPlayer2Sprite;
                            break;
                    }
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag.Equals("Player"))
        {
            Player player = other.GetComponent<Player>();

            var userInfo = other.gameObject.GetComponent<PlayerAvatar>().userInfo;

            if (!GameManager.isLocalGame)
            {
                if (hostOnly && !(GameManager.isHost && userInfo.isHost)) return;
                if (requiresMoreThanOnePlayerInHub && ClientInRoom.players.Count <= 1) return;

                if (userInfo.id != Client.user.id) return;
            }

            if (player.promptInput)
            {
                player.promptInput = false;
                onPressed?.Invoke();
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
