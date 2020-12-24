using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnerManager : MonoBehaviour
{
    [SerializeField] List<GameObject> playerSpawnLocations = new List<GameObject>();
    [SerializeField] int maxPlayers = 4;
    int currentPlayers = 0;

    void OnPlayerJoined(PlayerInput playerInput)
    {
        if (currentPlayers >= maxPlayers) return;

        SpawnPlayerAtLocation(playerInput.gameObject);
    }

    void SpawnPlayerAtLocation(GameObject player)
    {
        Vector3 availableLocation = playerSpawnLocations[currentPlayers].transform.position;

        currentPlayers++;
        player.GetComponent<Player>().playerNumber = currentPlayers;

        player.transform.position = availableLocation;
    }
}
