using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnerManager : MonoBehaviour
{
    public static bool isInHub = false;

    private bool hasJoined = false;
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

        if(!GameManager.isLocalGame || true)
            StartCoroutine(KickCountdown());
    }

    IEnumerator KickCountdown()
    {
        yield return new WaitForSecondsRealtime(timeToKickOut);
        if (!anyInputDone)
        {
            kicking = true;
            ClientInRoom.Exit();
        }
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        if (currentPlayers >= maxPlayers) return;

        if(!GameManager.isLocalGame)
        {
            //Online Game
            if (kicking)
            {
                Destroy(playerInput.gameObject);
                return;
            }

            if (!hasJoined)
            {
                hasJoined = true;
                anyInputDone = true;
                //TODO Avisar al server de que se quiere spawnear el jugador y hacer que lo cree el playerJoiner
                Player playerComp = SpawnPlayerAtLocation(playerInput.gameObject);
                UserInfo userInfo = new UserInfo();
                User user = Client.user;
                userInfo.id = user.id;
                userInfo.isHost = false;
                userInfo.isClient = false;
                userInfo.bodyType = user.avatar_bodyType;
                userInfo.skinTone = user.avatar_skinTone;
                userInfo.color = user.avatar_color;
                userInfo.face = user.avatar_face;
                userInfo.hat = user.avatar_hat;
                userInfo.frame = user.avatar_frame;
                playerComp.SetUserInfo(userInfo);
            }
            else
                Destroy(playerInput.gameObject);
        }
        //Local Game
        else
        {
            anyInputDone = true;
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
