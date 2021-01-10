using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkGem : NetworkObj
{
    [SerializeField] float maxDistance = 1f;
    [SerializeField] float lerp = 11f;

    public class Info
    {
        public string key;
        public Vector3 pos;
        public bool active;
        public int tierId;
    }

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
            info.pos = transform.position;
            info.active = gameObject.activeSelf;
            info.tierId = gem.tierIndex;
            return JsonUtility.ToJson(info);
        }
        return "";
        
        //else return "";
        
    }

    public override void SetInfo(string json)
    {
        if (GameManager.isHost || json == "") return;
        info = JsonUtility.FromJson<Info>(json);
        if(info.active != gameObject.activeSelf)
        {
            gem.UpdateGemTier(info.tierId);
            gameObject.SetActive(info.active);
        }
    }

    private void Update()
    {
        if (GameManager.isClient)
        {
            float realLerp = lerp * Time.deltaTime;
            if (realLerp > 1f) realLerp = 1f;
            float dist = Vector3.Distance(transform.position, info.pos);
            if (dist > maxDistance) transform.position = info.pos;
            else transform.position = Vector3.Lerp(transform.position, info.pos, realLerp);
        }
    }
}
