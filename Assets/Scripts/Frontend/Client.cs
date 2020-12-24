using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ws://localhost:8080/player/websocket
// wss://gem-fever.herokuapp.com//player/websocket

public class Client : MonoBehaviour
{
    public static User user = null;
    [HideInInspector] public Websocket socket = null;

    [HideInInspector] public static ClientState state = null;
    [HideInInspector] public static ClientUnconnected unconnectedState;
    [HideInInspector] public static ClientConnected connectedState;
    [HideInInspector] public static ClientSignedUp signedUpState;
    [HideInInspector] public static ClientSignedIn signedInState;

    public static Client instance = null;

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Debug.Log("START FRONTEND");
        instance = this;
        socket = GetComponent<Websocket>();
        socket.onCloseCallback = () => SetState(unconnectedState);
        socket.onErrorCallback += (err) => SetState(unconnectedState);
        CreateStates();
        SetState(unconnectedState);
    }

    private void CreateStates()
    {
        unconnectedState = new ClientUnconnected();
        connectedState = new ClientConnected();
        signedUpState = new ClientSignedUp();
        signedInState = new ClientSignedIn();
    }

    public static void SetState(ClientState newState)
    {
        state?.Finish();
        state = newState;
        state.Begin();
    }

    public static bool IsCurrentState(ClientState _state)
    {
        return state == _state;
    }

}
