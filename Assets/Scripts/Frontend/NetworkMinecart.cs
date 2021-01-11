using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMinecart : NetworkObj
{
    public class Info
    {
        public string key;
        public string t; //text
    }

    Info info;
    Minecart minecart;

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
        minecart = GetComponent<Minecart>();
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
        info.t = minecart.comboString;
        return JsonUtility.ToJson(info);
    }

    public override void SetInfo(string json)
    {
        if (GameManager.isHost || json == "") return;
        info = JsonUtility.FromJson<Info>(json);
        minecart.SetComboText(info.t);
    }
}
