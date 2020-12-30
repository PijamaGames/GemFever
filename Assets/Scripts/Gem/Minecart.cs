using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minecart : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip gemIntoMinecart;
    [SerializeField] AudioClip playerIntoMinecart;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PlaySound(AudioClip clip)
    {
        if(!audioSource.isPlaying)
        {
            audioSource.clip = clip;
            audioSource.PlayOneShot(clip);
        }
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
