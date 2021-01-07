﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : NetworkObj
{
    public class Info
    {
        public string key = "";
        public Vector3 position = Vector3.zero;
        public string animation = "";
        public int gems = 0;
        public int score = 0;
    }

    public class InputInfo
    {
        public string key = "";
    }

    [SerializeField] float lerp = 3f;

    [HideInInspector] public Info info;
    [HideInInspector] public InputInfo inputInfo;

    PlayerAvatar playerAvatar;
    private UserInfo userInfo;
    Rigidbody rb;

    Animator anim;
    Player player;

    void Start()
    {
        player = GetComponent<Player>();
        anim = GetComponent<Animator>();
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
        else if (GameManager.isClient)
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
                if(lastInfo.animation != info.animation)
                {
                    anim.Play(info.animation);
                }
            }
        }
    }

    private void OnDestroy()
    {
        allObjs.Remove(this);
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
                info.position = transform.position;
                info.animation = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                info.gems = player.currentPouchSize;
                info.score = player.score;
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
        transform.position = Vector3.Lerp(transform.position, info.position, lerp * Time.deltaTime);
    }
}