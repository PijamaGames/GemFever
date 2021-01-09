using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkAudio : NetworkObj
{
    public static List<string> allEffects = new List<string>();

    PersistentAudioSource audioManager;

    public class Info
    {
        public string key = "audio";
        public string[] effects;
    }

    Info info;

    private void Start()
    {
        info = new Info();
        audioManager = GetComponent<PersistentAudioSource>();
        allObjs.Add(this);
        objsDict.Add(info.key, this);
    }

    private void OnDestroy()
    {
        allEffects.Clear();
        allObjs.Remove(this);
        objsDict.Remove(info.key);
    }

    public override string CollectInfo()
    {
        if (GameManager.isClient) return "";
        info.effects = allEffects.ToArray();
        allEffects.Clear();
        return JsonUtility.ToJson(info);
    }

    public override void SetInfo(string json)
    {
        if (GameManager.isHost) return;
        info = JsonUtility.FromJson<Info>(json);
        foreach(var effect in info.effects)
        {
            audioManager.PlayEffect(clipName: effect);
        }
    }
}
