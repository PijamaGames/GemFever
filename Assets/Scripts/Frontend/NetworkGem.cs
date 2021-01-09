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
    Rigidbody rb;

    bool firstFrameInactive = true;
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
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = GameManager.isClient;
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
        if (gameObject.activeSelf || (!gameObject.activeSelf && firstFrameInactive))
        {
            firstFrameInactive = false;
            info.position = transform.position;
            info.active = gameObject.activeSelf;
            return JsonUtility.ToJson(info);
        }
        else return "";
        
    }

    public override void SetInfo(string json)
    {
        if (GameManager.isHost || json == "") return;
        info = JsonUtility.FromJson<Info>(json);
        Debug.Log("Setting info of gem");
        /*if(info.active != gameObject.activeSelf)
        {*/
            gameObject.SetActive(info.active);
        //}
    }

    private void Update()
    {
        if (GameManager.isClient)
        {
            float realLerp = lerp * Time.deltaTime;
            if (realLerp > 1f) realLerp = 1f;
            transform.position = Vector3.Lerp(transform.position, info.position, realLerp);
        }
    }
}
