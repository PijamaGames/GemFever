using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ws://localhost:8080/player/websocket
// wss://gem-fever.herokuapp.com//player/websocket

public class Client : MonoBehaviour
{
    public static User user = null;
    [HideInInspector] public Websocket socket = null;

    private ClientState state = null;
    [HideInInspector] public ClientUnconnected unconnectedState;
    [HideInInspector] public ClientConnected connectedState;

    private static Client instance = null;

    private void Start()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
        socket = GetComponent<Websocket>();

        GetStates();
        SetState(unconnectedState);
    }

    private void GetStates()
    {
        unconnectedState = GetComponentInChildren<ClientUnconnected>();
        unconnectedState.gameObject.SetActive(false);
        connectedState = GetComponentInChildren<ClientConnected>();
        connectedState.gameObject.SetActive(false);
    }

    public void SetState(ClientState newState)
    {
        state?.Finish();
        state = newState;
        state.Begin();
    }
}
