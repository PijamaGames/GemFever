using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoiner : MonoBehaviour
{
    [SerializeField] Player playerPrefab;
    Player spawnedPlayer;

    public void SpawnPlayerWithEmptyControlScheme()
    {
        spawnedPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity);

        spawnedPlayer.GetComponent<PlayerInput>().SwitchCurrentControlScheme("EmpyControlScheme");
    }
}
