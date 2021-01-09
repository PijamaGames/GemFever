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
        info = new Info();
        allObjs.Add(this);
        objsDict.Add(gameObject.name, this);
    }

    private void OnDestroy()
    {
        allObjs.Remove(this);
        objsDict.Remove(info.key);
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
        if (GameManager.isHost) return;
        info = JsonUtility.FromJson<Info>(json);
        if(info.active != gameObject.activeSelf)
        {
            gameObject.SetActive(info.active);
        }
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, info.position, lerp * Time.deltaTime);
    }
}
