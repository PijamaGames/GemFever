using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : NetworkObj
{
    public class Info
    {
        public string key = "";
        public int x = 0;
        public int y = 0;
        public int z = 0;
        public int rx = 0;
        public int ry = 0;
        public int rz = 0;
        public string a = ""; //animation
        public float s = 1f; //animation speed
        public int g = 0; //gems
        public int sc = 0; //score
    }

    public class InputInfo
    {
        public string key = "";
        public Vector2 joystick = Vector2.zero;
        public float pickaxeInput = 0f;
        public float throwGemInput = 0f;
    }

    [SerializeField] float maxDistance = 1f;
    [SerializeField] float lerp = 3f;

    [HideInInspector] public Info info;
    [HideInInspector] public InputInfo inputInfo;

    PlayerAvatar playerAvatar;
    private UserInfo userInfo;
    Rigidbody rb;

    Animator anim;
    Player player;

    Vector3 targetPos = Vector3.zero;

    void Start()
    {
        if (GameManager.isLocalGame)
        {
            Destroy(this);
            return;
        }
        rb = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
        anim = GetComponentInChildren<Animator>();
        info = new Info();
        inputInfo = new InputInfo();
        allObjs.Add(this);
        playerAvatar = GetComponent<PlayerAvatar>();
        userInfo = playerAvatar.userInfo;
        rb.isKinematic = GameManager.isClient;
        objsDict.Add(playerAvatar.userInfo.id, this);
        info.key = playerAvatar.userInfo.id;
        inputInfo.key = playerAvatar.userInfo.id;
    }

    public override string CollectInfo()
    {
        string json = "";
        if (GameManager.isHost)
        {
            json = JsonUtility.ToJson(info);
        }
        else if (GameManager.isClient && userInfo.id == Client.user.id)
        {
            json = JsonUtility.ToJson(inputInfo);
        }
        return json;
    }

    public override void SetInfo(string json)
    {
        if(json != "")
        {
            if (GameManager.isHost)
            {
                inputInfo = JsonUtility.FromJson<InputInfo>(json);
            } else if (GameManager.isClient)
            {
                Info lastInfo = info;
                info = JsonUtility.FromJson<Info>(json);
                if(lastInfo.a != info.a)
                {
                    anim.Play(info.a);
                }
                targetPos = new Vector3(info.x, info.y, info.z) * 0.01f;
            }
        }
    }

    private void OnDestroy()
    {
        if (!GameManager.isLocalGame)
        {
            allObjs.Remove(this);
            objsDict.Remove(playerAvatar.userInfo.id);
        }
    }

    void Update()
    {
        /*if(userInfo == null)
        {
            userInfo = playerAvatar.userInfo;
        }*/
        if(userInfo != null)
        {
            if (GameManager.isHost)
            {
                //info.p = transform.position;
                info.x = Mathf.RoundToInt(transform.position.x * 100f);
                info.y = Mathf.RoundToInt(transform.position.y * 100f);
                info.z = Mathf.RoundToInt(transform.position.z * 100f);

                var animClipInfo = anim.GetCurrentAnimatorClipInfo(0);
                if(animClipInfo.Length > 0)
                {
                    info.a = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                }
                info.g = player.currentPouchSize;
                info.sc = player.score;
            }
            else if (GameManager.isClient)
            {
                //TODO: SET INPUT INFO
            }
        }
        if (GameManager.isClient)
        {
            LerpPosition();
        }
    }

    private void LerpPosition()
    {
        float realLerp = lerp * Time.deltaTime;
        if (realLerp > 1f) realLerp = 1f;
        float dist = Vector3.Distance(transform.position, targetPos);
        if (dist > maxDistance) transform.position = targetPos;
        else transform.position = Vector3.Lerp(transform.position, targetPos, realLerp);
    }
}
