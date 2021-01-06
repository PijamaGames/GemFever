using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientInRoom : ClientState
{
    private enum FrontendEvents { Error, Exit, AddPlayer, RemovePlayer };
    private enum BackendEvents { Exit };
    public static int error;

    public static List<string> queuedMessages = new List<string>();

    public static Dictionary<string, Player> players = new Dictionary<string, Player>();

    override public void Begin()
    {
        base.Begin();
        SceneLoader.instance.LoadHubScene();
        Debug.Log("In room");
        string message;
        foreach(var m in queuedMessages)
        {
            message = m;
            HandleMessage(ref message);
        }
        queuedMessages.Clear();
    }

    public static void Exit()
    {
        BackendEvents evt = BackendEvents.Exit;
        var pairs = new KeyValuePair<string, object>[]
        {
            new KeyValuePair<string, object>("evt", UsefulFuncs.PrimitiveToJsonValue((int)evt)),
        };
        string msg = UsefulFuncs.CombineJsons(pairs);
        if (Client.instance != null && Client.instance.socket != null)
            Client.instance.socket.SendMessage(msg);
        GameManager.instance.BlockUI();
    }

    public class MsgStructure
    {
        public int evt = 0;
        public int error = -1;
        public bool roomEvt = true;
        public bool isHost = false;
        public bool isClient = false;
        public string id = "";

        public int avatar_bodyType = -1;
        public int avatar_skinTone = -1;
        public int avatar_color = -1;
        public string avatar_face = "";
        public string avatar_hat = "";
        public string avatar_frame = "";
    }

    override public void HandleMessage(ref string msg)
    {
        base.HandleMessage(ref msg);
        MsgStructure data = JsonUtility.FromJson<MsgStructure>(msg);
        FrontendEvents evt = (FrontendEvents)data.evt;
        Debug.Log("EVENT: " + evt);
        switch (evt)
        {
            case FrontendEvents.Error:
                error = data.error;
                Client.SetState(Client.signedInState);
                break;
            case FrontendEvents.Exit:
                Client.SetState(Client.signedInState);
                break;
            case FrontendEvents.AddPlayer:
                var info = new UserInfo();
                info.id = data.id;
                info.isHost = data.isHost;
                info.isClient = data.isClient;
                info.bodyType = data.avatar_bodyType;
                info.skinTone = data.avatar_skinTone;
                info.color = data.avatar_color;
                info.face = data.avatar_face;
                info.hat = data.avatar_hat;
                info.frame = data.avatar_frame;
                PlayerJoiner.queuedUsers.Enqueue(info);
                break;
            case FrontendEvents.RemovePlayer:

                break;
        }
    }

    override public void Finish()
    {
        base.Finish();
        players.Clear();
    }
}
