using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<GameObject> levels = new List<GameObject>();
    [SerializeField] List<AudioClip> levelMusic=new List<AudioClip>();
    [SerializeField] private PersistentAudioSource persistentAudioSource;

    private System.Random rnd = new System.Random();
    private int randomMusicId;

    private void Awake()
    {
        foreach (var level in levels)
            level.SetActive(false);

        levels[GameManager.levelId].SetActive(true);

        randomMusicId = rnd.Next(levelMusic.Count);
        persistentAudioSource.PlayMusic(levelMusic[randomMusicId]);
    }
}
