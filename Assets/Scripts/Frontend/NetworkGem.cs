using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkGem : NetworkObj
{
    [SerializeField] float maxDistance = 1f;
    [SerializeField] float lerp = 11f;



    public class Info
    {
        public string key; //key
        public Vector3 p; //position
        public bool a; //activate
        public bool d; //deactivate
        public int t; //tierId
    }

    bool lastActive = false;
    bool lastInactive = false;

    Info info;
    Rigidbody rb;
    Gem gem;

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
        gem = GetComponent<Gem>();
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
            if (gameObject.activeSelf) firstFrameInactive = true;
            else firstFrameInactive = false;
            info.p = transform.position;
            info.a = gameObject.activeSelf && !lastActive;
            info.d = !gameObject.activeSelf && !lastInactive;
            lastActive = gameObject.activeSelf;
            lastInactive = !gameObject.activeSelf;
            info.t = gem.tierIndex;
            return JsonUtility.ToJson(info);
        }
        return "";
        
        //else return "";
        
    }

    public override void SetInfo(string json)
    {
        if (GameManager.isHost || json == "") return;
        info = JsonUtility.FromJson<Info>(json);
        gem.UpdateGemTier(info.t);
        if (info.a || info.d)
        {
            if (info.a)
            {
                Debug.Log("activate gem");
                gameObject.SetActive(true);
            }
            if (info.d)
            {
                Debug.Log("deactivate gem");
                gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (GameManager.isClient)
        {
            float realLerp = lerp * Time.deltaTime;
            if (realLerp > 1f) realLerp = 1f;
            float dist = Vector3.Distance(transform.position, info.p);
            if (dist > maxDistance) transform.position = info.p;
            else transform.position = Vector3.Lerp(transform.position, info.p, realLerp);
        }
    }
}
