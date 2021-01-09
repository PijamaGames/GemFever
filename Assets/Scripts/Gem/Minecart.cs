using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minecart : MonoBehaviour
{
    PersistentAudioSource audioSource;
    [SerializeField] AudioClip gemIntoMinecart;
    [SerializeField] AudioClip playerIntoMinecart;

    private void Start()
    {
        audioSource = FindObjectOfType<PersistentAudioSource>();
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
        }
    }
}
