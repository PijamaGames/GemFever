using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClientSignedIn : ClientState
{
    private enum FrontendEvents { SignedOut, Error, InRoom, GetRooms };
    private enum BackendEvents { SignOut, Save, CreateRoom, JoinRoom, RequestRooms };

    public static event Action signedOutEvent;
    public static event Action<List<PlayerRoomInfo>> getRoomsEvent;

    public static bool hasEvent = false;
    public static string spanishMsg = "";
    public static string englishMsg = "";

    public static int error = -1;
    public static bool goToLobby = false;

    override public void Begin()
    {
        base.Begin();
        Debug.Log("Signed in");
        Debug.Log("HAS EVENT: " + hasEvent);
        if (hasEvent)
        {
            hasEvent = false;
            Debug.Log("SPANISH MSG EVENT: " + spanishMsg);
            Debug.Log("ENGLISH MSG EVENT: " + englishMsg);
            SceneLoader.instance.LoadEventViewScene();
        } else if (goToLobby)
        {
            goToLobby = false;
            SceneLoader.instance.LoadLobbyScene();
        } else
        {
            SceneLoader.instance.LoadMainMenuScene();
        }
        GameManager.instance.SavePreferences();
    }

    private class MsgStructure
    {
        public int evt = 0;
        public bool isHost = false;
        public bool isClient = false;
        public int error = -1;
        public string[] players;
    }

    public override void HandleMessage(ref string msg)
    {
        base.HandleMessage(ref msg);
        MsgStructure data = JsonUtility.FromJson<MsgStructure>(msg);
        FrontendEvents evt = (FrontendEvents)data.evt;
        Debug.Log("EVENT: " + evt);
        switch (evt)
        {
            case FrontendEvents.SignedOut:
                Client.SetState(Client.connectedState);
                signedOutEvent?.Invoke();
                break;
            case FrontendEvents.Error:
                error = data.error;
                GameManager.instance.ReleaseUI();
                Debug.Log("SignedIn error: " + data.error);
                //TODO: DISPLAY ERROR IN LOBBY
                break;
            case FrontendEvents.InRoom:
                Client.SetState(Client.inRoomState);
                break;
            case FrontendEvents.GetRooms:
                var roomInfos = new List<PlayerRoomInfo>();
                PlayerRoomInfo info;
                foreach(string json in data.players)
                {
                    info = JsonUtility.FromJson<PlayerRoomInfo>(json);
                    Debug.Log("Room info: " + json);
                    if(info != null)
                    {
                        roomInfos.Add(info);
                    }
                }
                getRoomsEvent?.Invoke(roomInfos);
                break;
        }
    }

    public static void RequestRoomInfos()
    {
        BackendEvents evt = BackendEvents.RequestRooms;
        var pairs = new KeyValuePair<string, object>[]
        {
            new KeyValuePair<string, object>("evt", UsefulFuncs.PrimitiveToJsonValue((int)evt)),
        };
        string msg = UsefulFuncs.CombineJsons(pairs);

        if (Client.instance != null && Client.instance.socket != null)
            Client.instance.socket.SendMessage(msg);
    }

    public static void CreateRoom()
    {
        BackendEvents evt = BackendEvents.CreateRoom;
        var pairs = new KeyValuePair<string, object>[]
        {
            new KeyValuePair<string, object>("evt", UsefulFuncs.PrimitiveToJsonValue((int)evt)),
        };
        string msg = UsefulFuncs.CombineJsons(pairs);

        if (Client.instance != null && Client.instance.socket != null)
            Client.instance.socket.SendMessage(msg);
    }

    public static void JoinRoom(string hostName)
    {
        BackendEvents evt = BackendEvents.JoinRoom;
        var pairs = new KeyValuePair<string, object>[]
        {
            new KeyValuePair<string, object>("evt", UsefulFuncs.PrimitiveToJsonValue((int)evt)),
            new KeyValuePair<string, object>("host", UsefulFuncs.PrimitiveToJsonValue(hostName)),

        };
        string msg = UsefulFuncs.CombineJsons(pairs);

        if (Client.instance != null && Client.instance.socket != null)
            Client.instance.socket.SendMessage(msg);

    }

    public static void SaveInfo()
    {
        BackendEvents evt = BackendEvents.Save;
        var pairs = new KeyValuePair<string, object>[]
        {
            new KeyValuePair<string, object>("evt", UsefulFuncs.PrimitiveToJsonValue((int)evt)),
            new KeyValuePair<string, object>("user", JsonUtility.ToJson(Client.user)),
        };
        string msg = UsefulFuncs.CombineJsons(pairs);

        if (Client.instance != null && Client.instance.socket != null)
            Client.instance.socket.SendMessage(msg);
    }

    public static void TrySignOut()
    {
        BackendEvents evt = BackendEvents.SignOut;
        var pairs = new KeyValuePair<string, object>[]
        {
            new KeyValuePair<string, object>("evt", UsefulFuncs.PrimitiveToJsonValue((int)evt)),
        };
        string msg = UsefulFuncs.CombineJsons(pairs);

        if (Client.instance != null && Client.instance.socket != null)
            Client.instance.socket.SendMessage(msg);
    }

    override public void Finish()
    {
        base.Finish();
    }
}
