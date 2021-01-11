using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInfo
{
    public bool isHost;
    public bool isClient;
    public string id;
    public int bodyType;
    public int skinTone;
    public int color;
    public string face;
    public string hat;
    public string frame;
}

public class PlayerJoiner : MonoBehaviour
{
    Transform networkPlayersParent;
    [SerializeField] GameObject playerPrefab;

    private void Awake()
    {
        networkPlayersParent = GameObject.FindGameObjectWithTag("NetworkPlayers").transform;
    }

    public static Queue<UserInfo> queuedUsers = new Queue<UserInfo>();

    public Player SpawnPlayerWithEmptyControlScheme()
    {
        GameObject spawnedPlayer = Instantiate(playerPrefab, networkPlayersParent);
        PlayerInput playerInput = spawnedPlayer.GetComponent<PlayerInput>();
        playerInput.SwitchCurrentControlScheme("EmptyControlScheme");
        Player player = spawnedPlayer.GetComponent<Player>();
        return player;
    }

    private void Update()
    {
        bool hasValues = false;
        foreach(var info in queuedUsers)
        {
            hasValues = true;
            Player p = SpawnPlayerWithEmptyControlScheme();
            p.SetUserInfo(info);
        }
        if(hasValues)
            queuedUsers.Clear();
    }

    private void OnDestroy()
    {
        queuedUsers.Clear();
    }
}
