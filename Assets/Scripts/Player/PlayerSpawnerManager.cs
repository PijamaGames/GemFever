using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnerManager : MonoBehaviour
{
    [SerializeField] Transform networkPlayersParent;

    public static bool isInHub = false;

    [SerializeField] bool hasJoined = false;
    private bool anyInputDone = false;
    private bool kicking = false;

    [SerializeField] bool _isInHub;
    [SerializeField] float timeToKickOut = 10f;

    [SerializeField] List<GameObject> playerSpawnLocations = new List<GameObject>();
    [SerializeField] int maxPlayers = 4;
    int currentPlayers = 0;

    private void Start()
    {
        isInHub = _isInHub;

        if(!GameManager.isLocalGame)
            StartCoroutine(KickCountdown());
    }

    IEnumerator KickCountdown()
    {
        yield return new WaitForSecondsRealtime(timeToKickOut);
        if (!anyInputDone && GameManager.isClient)
        {
            Debug.Log("kicking");
            kicking = true;
            ClientInRoom.Exit();
        }
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        if (currentPlayers >= maxPlayers)
        {
            Destroy(playerInput.gameObject);
            return;
        }

        if(!GameManager.isLocalGame)
        {
            //Online Game
            if (kicking)
            {
                Destroy(playerInput.gameObject);
                return;
            }
            Player playerComp = SpawnPlayerAtLocation(playerInput.gameObject);
            if (!hasJoined && playerInput.transform.parent != networkPlayersParent)
            {
                anyInputDone = true;
                hasJoined = true;
                UserInfo userInfo = new UserInfo();
                User user = Client.user;
                userInfo.id = user.id;
                userInfo.isHost = GameManager.isHost;
                userInfo.isClient = GameManager.isClient;
                userInfo.bodyType = user.avatar_bodyType;
                userInfo.skinTone = user.avatar_skinTone;
                userInfo.color = user.avatar_color;
                userInfo.face = user.avatar_face;
                userInfo.hat = user.avatar_hat;
                userInfo.frame = user.avatar_frame;
                playerComp.SetUserInfo(userInfo);
                ClientInRoom.Spawn();
            }
            else if(playerInput.transform.parent != networkPlayersParent)
                Destroy(playerInput.gameObject);
        }
        //Local Game (Random skin)
        else
        {
            anyInputDone = true;
            //playerInput.GetComponent<PlayerAvatar>().UpdateVisuals();     
            SpawnPlayerAtLocation(playerInput.gameObject);
        }
    }

    private Player SpawnPlayerAtLocation(GameObject player)
    {
        Vector3 availableLocation = playerSpawnLocations[currentPlayers].transform.position;

        currentPlayers++;
        Player comp = player.GetComponent<Player>();
        comp.playerNumber = currentPlayers;

        player.transform.position = availableLocation;
        return comp;
    }
}
