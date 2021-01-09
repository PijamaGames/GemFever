using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentAudioSource : MonoBehaviour
{

    private const int poolSize = 20;
    private static AudioSource[] audioSources = new AudioSource[poolSize];
    private static Stack<AudioSource> avaibleAudioSources = new Stack<AudioSource>();
    private static HashSet<AudioSource> inUseAudioSources = new HashSet<AudioSource>();
    private static GameObject pool;
    [SerializeField] private AudioClip[] allClips;
    static Dictionary<string, AudioClip> allClipsDict = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if(pool == null)
        {
            foreach (var clip in allClips) allClipsDict.Add(clip.name, clip);
            pool = new GameObject("AudioPool");
            pool.transform.position = Camera.main.transform.position;
            GameObject obj;
            for (int i = 0; i < poolSize; i++)
            {
                obj = new GameObject("AudioSource" + i);
                obj.transform.parent = pool.transform;
                audioSources[i] = obj.AddComponent(typeof(AudioSource)) as AudioSource;
                obj.AddComponent(typeof(AudioRegulator));
                audioSources[i].enabled = false;
                avaibleAudioSources.Push(audioSources[i]);
            }
            DontDestroyOnLoad(pool);
        } else
        {
            Debug.Log("AudioPool already present");
        }
    }

    private void Update()
    {
        List<AudioSource> audiosToStop = new List<AudioSource>();
        foreach(AudioSource source in inUseAudioSources)
        {
            if (!source.isPlaying)
            {
                audiosToStop.Add(source);
            }
        }
        for(int i = 0; i < audiosToStop.Count; i++)
        {
            StopSource(audiosToStop[i]);
        }
        audiosToStop.Clear();
    }

    private void StopSource(AudioSource source)
    {
        source.enabled = false;
        inUseAudioSources.Remove(source);
        avaibleAudioSources.Push(source);
    }

    public void StopClip(AudioClip clip)
    {
        List<AudioSource> audiosToStop = new List<AudioSource>();
        foreach (AudioSource source in inUseAudioSources)
        {
            if(source && source.clip == clip)
            {
                audiosToStop.Add(source);
                //StopSource(source);
            }
        }
        for (int i = 0; i < audiosToStop.Count; i++)
        {
            StopSource(audiosToStop[i]);
        }
        audiosToStop.Clear();
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        bool alreadyPlaying = false;
        foreach(AudioSource source in inUseAudioSources)
        {
            if(source && source.clip == clip)
            {
                alreadyPlaying = true;
            }
        }

        if (!alreadyPlaying)
        {
            TryPlayClip(clip, true, true);
        }
    }

    public void PlayEffect(string clipName)
    {
        AudioClip clip;
        if (allClipsDict.TryGetValue(clipName, out clip))
            PlayEffect(clip);
    }

    public void PlayEffect(AudioClip clip)
    {
        if (clip != null)
            TryPlayClip(clip, false, false);

        if (GameManager.isHost)
        {
            NetworkAudio.allEffects.Add(clip.name);
        }
    }

    private void TryPlayClip(AudioClip clip, bool isMusic, bool loop = false)
    {
        if (avaibleAudioSources.Count > 0)
        {
            var source = avaibleAudioSources.Pop();
            if (!source) return;
            inUseAudioSources.Add(source);
            source.enabled = true;
            source.clip = clip;
            source.loop = loop;
            source.Play();
            var regulator = source.GetComponent<AudioRegulator>();
            regulator.isMusic = isMusic;
            regulator.UpdateVolume();
        }
    }
}
