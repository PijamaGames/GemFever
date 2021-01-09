using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkGem : NetworkObj
{
    [SerializeField] float lerp = 11f;

    public class Info
    {
        public string key;
        public Vector3 position;
        public bool active;
    }

    Info info;

    private void Start()
    {
        if (GameManager.isLocalGame)
        {
            Destroy(this);
            return;
        }
        info = new Info();
        info.key = gameObject.name;
        allObjs.Add(this);
        objsDict.Add(info.key, this);
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
        info.position = transform.position;
        info.active = gameObject.activeSelf;
        return JsonUtility.ToJson(info);
    }

    public override void SetInfo(string json)
    {
        if (GameManager.isHost || json == "") return;
        info = JsonUtility.FromJson<Info>(json);
        if(info.active != gameObject.activeSelf)
        {
            gameObject.SetActive(info.active);
        }
    }

    private void Update()
    {
        if(GameManager.isClient)
            transform.position = Vector3.Lerp(transform.position, info.position, lerp * Time.deltaTime);
    }
}
