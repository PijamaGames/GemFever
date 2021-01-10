using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkOre : NetworkObj
{
    public class Info
    {
        public string key;
        public bool depleted;
        public int tierId;
    }

    Info info;
    Ore ore;

    bool init = false;

    private void Start()
    {
        if (!init) Init();
    }

    public void Init()
    {
        if (init) return;
        init = true;
        if (GameManager.isLocalGame)
        {
            Destroy(this);
            return;
        }
        info = new Info();
        info.key = gameObject.name;
        allObjs.Add(this);
        objsDict.Add(info.key, this);
        ore = GetComponent<Ore>();
    }

    private void OnDestroy()
    {
        if (!GameManager.isLocalGame)
        {
            allObjs.Remove(this);
            if (info != null)
                objsDict.Remove(info.key);
        }
    }

    public override string CollectInfo()
    {
        if (GameManager.isClient) return "";
        info.depleted = ore.depleted;
        info.tierId = ore.oreTierIndex;
        return JsonUtility.ToJson(info);
    }

    public override void SetInfo(string json)
    {
        if (GameManager.isHost || json == "") return;
        info = JsonUtility.FromJson<Info>(json);
        ore.UpdateColorInOre(info.tierId);
        ore.ShowGemMeshes(!info.depleted);
    }
}
