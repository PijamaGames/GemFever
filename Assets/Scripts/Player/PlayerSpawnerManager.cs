using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnerManager : MonoBehaviour
{
    Transform networkPlayersParent;

    public static bool isInHub = false;

    public static bool firstTimeInHub = true;

    [SerializeField] bool hasJoined = false;
    private bool anyInputDone = false;
    private bool kicking = false;

    [SerializeField] bool _isInHub;
    [SerializeField] float timeToKickOut = 10f;

    [SerializeField] List<GameObject> playerSpawnLocations = new List<GameObject>();
    [SerializeField] List<Transform> hubSpawnLocations = new List<Transform>();
    [SerializeField] int maxPlayers = 4;
    [SerializeField] GameObject joinButtons;
    int currentJoinedPlayers = 0;

    private void Start()
    {
        if(joinButtons != null)
        {
            if (GameManager.isHandheld)
                joinButtons.SetActive(false);
            else if(firstTimeInHub)
                joinButtons.SetActive(true);
            else
                joinButtons.SetActive(false);
        }

        networkPlayersParent = GameObject.FindGameObjectWithTag("NetworkPlayers").transform;
        isInHub = _isInHub;

        if(!GameManager.isLocalGame && isInHub)
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
        if (currentJoinedPlayers >= maxPlayers)
        {
            Destroy(playerInput.gameObject);
            return;
        }

        Player player = playerInput.GetComponent<Player>();

        if (isInHub && !player.initialized)
        {
            player.initialized = true;

            if (!GameManager.isLocalGame)
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

                    if(joinButtons != null)
                        joinButtons.SetActive(false);

                    ClientInRoom.Spawn();
                }
                else if (playerInput.transform.parent != networkPlayersParent)
                    Destroy(playerInput.gameObject);
            }
            //Local Game (Random skin)
            else
            {
                anyInputDone = true;
                playerInput.GetComponent<PlayerAvatar>().UpdateVisuals();     
                SpawnPlayerAtLocation(playerInput.gameObject);
            }
        }
        else
            SpawnPlayerAtLocation(player.gameObject);
    }

    private Player SpawnPlayerAtLocation(GameObject player)
    {
        //TODO CAMBIAR PUNTOS DE SPAWN SEGÚN EL NIVEL
        Transform[] spawnLocations;

        if (!_isInHub)
        {
            GameObject aux = playerSpawnLocations[GameManager.levelId];
            spawnLocations = aux.GetComponentsInChildren<Transform>();
            Debug.Log(" ");
        }
            
        else
            spawnLocations = hubSpawnLocations.ToArray();

        Vector3 availableLocation;

        if (!_isInHub)
            //Ignorar componente del padre
            availableLocation = spawnLocations[currentJoinedPlayers + 1].transform.position;
        else
            availableLocation = spawnLocations[currentJoinedPlayers].transform.position;

        currentJoinedPlayers++;
        Player comp = player.GetComponent<Player>();
        comp.playerNumber = currentJoinedPlayers;

        player.transform.position = availableLocation;
        //player.transform.SetParent(null);
        return comp;
    }
}
