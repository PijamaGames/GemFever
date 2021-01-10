using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Minecart : MonoBehaviour
{
    PersistentAudioSource audioSource;
    [SerializeField] AudioClip gemIntoMinecart;
    [SerializeField] AudioClip playerIntoMinecart;

    [SerializeField] TextMeshProUGUI comboText;
    [SerializeField] float textFadeTime = 2f;

    //Network parameters
    public int comboNumber = 0;
    public bool textActive = false;

    private void Start()
    {
        audioSource = FindObjectOfType<PersistentAudioSource>();
        comboText.enabled = false;
    }

    private void PlaySound(AudioClip clip)
    {
        if(clip != null)
            audioSource.PlayEffect(clip);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlaySound(playerIntoMinecart);
        }
        else if(other.tag == "Gem")
        {
            PlaySound(gemIntoMinecart);

            if(GameManager.isLocalGame || GameManager.isHost)
            {
                comboText.enabled = true;
                comboNumber = 1;
                comboText.text = "x " + comboNumber;

                StopCoroutine(FadeText());
                StartCoroutine(FadeText());
            }
        }
    }

    private IEnumerator FadeText()
    {
        textActive = true;

        yield return new WaitForSecondsRealtime(textFadeTime);
        comboText.enabled = false;

        textActive = false;
    }

    public void PlayerComboText(int combo)
    {
        if (GameManager.isLocalGame || GameManager.isHost)
        {
            comboNumber = combo;

            comboText.enabled = true;
            comboText.text = "x " + comboNumber.ToString();

            StopCoroutine(FadeText());
            StartCoroutine(FadeText());
        }
    }
}
