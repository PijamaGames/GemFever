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
    [HideInInspector] public static ClientInRoom inRoomState;

    public static Client instance = null;

    [SerializeField] float syncRate = 10f;

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
        StartCoroutine(UpdateNetworkObjsCoroutine());
    }

    private void CreateStates()
    {
        unconnectedState = new ClientUnconnected();
        connectedState = new ClientConnected();
        signedUpState = new ClientSignedUp();
        signedInState = new ClientSignedIn();
        inRoomState = new ClientInRoom();
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

    IEnumerator UpdateNetworkObjsCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f / syncRate);
            if (state == inRoomState)
            {
                inRoomState.SendNetworkObjs();
            }
        }
    }
}
