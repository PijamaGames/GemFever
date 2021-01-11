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

    int comboNumber = 0;

    //Network parameters
    public string comboString = "";

    private void Start()
    {
        audioSource = FindObjectOfType<PersistentAudioSource>();
        comboText.text = "";

        comboString = comboText.text;
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

            if (GameManager.isLocalGame || GameManager.isHost)
                ComboText(1);
        }
    }

    private IEnumerator FadeText()
    {
        yield return new WaitForSecondsRealtime(textFadeTime);
        comboText.text = "";

        comboString = comboText.text;
    }

    public void ComboText(int combo)
    {
        comboNumber = combo;

        comboText.text = "x " + comboNumber.ToString();

        comboString = comboText.text;

        StopCoroutine(FadeText());
        StartCoroutine(FadeText());
    }

    public void SetComboText(string text)
    {
        comboText.text = text;

        comboString = comboText.text;
    }
}
