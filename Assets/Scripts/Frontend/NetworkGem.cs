using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkGem : NetworkObj
{
    [SerializeField] float prediction = 0.5f;
    [SerializeField] float maxDistance = 1f;
    [SerializeField] float lerp = 11f;

    public class Info
    {
        public string key; //key
        public int x; //position
        public int y;
        public int z;
        public int t; //tierId
        public bool a; //active
    }

    Info info;
    Rigidbody rb;
    Gem gem;

    bool firstFrameInactive = true;
    bool init = false;

    Vector3 targetPos = Vector3.zero;

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
        /*if (gameObject.activeSelf || (!gameObject.activeSelf && firstFrameInactive))
        {
            if (gameObject.activeSelf) firstFrameInactive = true;
            else firstFrameInactive = false;*/
            Vector3 pos = transform.position + rb.velocity * prediction;
            info.x = Mathf.RoundToInt(pos.x*100f);
            info.y = Mathf.RoundToInt(pos.y*100f);
            info.z = Mathf.RoundToInt(pos.z*100f);
            info.t = gem.tierIndex;
            info.a = gameObject.activeSelf;
            return JsonUtility.ToJson(info);
        //}
        //return "";
        
        //else return "";
        
    }

    public override void SetInfo(string json)
    {
        if (GameManager.isHost || json == "") return;
        info = JsonUtility.FromJson<Info>(json);
        gem.UpdateGemTier(info.t);
        if(gameObject.activeSelf != info.a)
        {
            gameObject.SetActive(info.a);
        }
        targetPos = new Vector3(info.x, info.y, info.z)*0.01f;
    }

    private void Update()
    {
        if (GameManager.isClient)
        {
            float realLerp = lerp * Time.deltaTime;
            if (realLerp > 1f) realLerp = 1f;
            float dist = Vector3.Distance(transform.position, targetPos);
            if (dist > maxDistance) transform.position = targetPos;
            else transform.position = Vector3.Lerp(transform.position, targetPos, realLerp);
        }
    }
}
