using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientInRoom : ClientState
{
    private enum FrontendEvents { Error, GetInfo, Exit, AddPlayer, RemovePlayer, Spawn, ChangeScene };
    private enum BackendEvents { Exit, SendObjects, Spawn, ChangeScene };
    public static int error;

    public static List<string> queuedMessages = new List<string>();

    public static Dictionary<string, Player> players = new Dictionary<string, Player>();
    private static Dictionary<string, UserInfo> waitingDict = new Dictionary<string, UserInfo>();

    static bool exitRequested = false;

    static int lastMs = -1;

    override public void Begin()
    {
        base.Begin();
        exitRequested = false;
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
        if (exitRequested) return;
        exitRequested = true;
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

    public static void Spawn()
    {
        BackendEvents evt = BackendEvents.Spawn;
        var pairs = new KeyValuePair<string, object>[]
        {
            new KeyValuePair<string, object>("evt", UsefulFuncs.PrimitiveToJsonValue((int)evt)),
            new KeyValuePair<string, object>("id", UsefulFuncs.PrimitiveToJsonValue(Client.user.id)),
        };
        string msg = UsefulFuncs.CombineJsons(pairs);
        if (Client.instance != null && Client.instance.socket != null)
            Client.instance.socket.SendMessage(msg);
    }

    public static void GoToGameScene()
    {
        ChangeScene(SceneLoader.gameScene, true);
    }

    public static void GoToVictoryScene()
    {
        ChangeScene(SceneLoader.victoryScene, true);
    }

    public static void GoToHUBScene()
    {
        ChangeScene(SceneLoader.hubScene);
    }

    private static void ChangeScene(string scene, bool playing = false)
    {
        if (!GameManager.isHost) return;
        BackendEvents evt = BackendEvents.Spawn;
        var pairs = new KeyValuePair<string, object>[]
        {
            new KeyValuePair<string, object>("evt", UsefulFuncs.PrimitiveToJsonValue((int)evt)),
            new KeyValuePair<string, object>("id", UsefulFuncs.PrimitiveToJsonValue(scene)),
            new KeyValuePair<string, object>("playing", UsefulFuncs.PrimitiveToJsonValue(playing)),
        };
        string msg = UsefulFuncs.CombineJsons(pairs);
        if (Client.instance != null && Client.instance.socket != null)
            Client.instance.socket.SendMessage(msg);
        SceneLoader.instance.LoadScene(scene);
    }

    public class MsgStructure
    {
        public int evt = 0;
        public int error = -1;
        public bool roomEvt = true;
        public bool isHost = false;
        public bool isClient = false;
        public string id = "";
        public bool spawned = false;

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
                waitingDict.Add(info.id, info);
                if (data.spawned) goto case FrontendEvents.Spawn;
                break;
            case FrontendEvents.Spawn:
                Debug.Log("Spawning " + data.id);
                UserInfo userInfo;
                if(waitingDict.TryGetValue(data.id, out userInfo))
                {
                    waitingDict.Remove(data.id);
                    PlayerJoiner.queuedUsers.Enqueue(userInfo);
                }
                break;
            case FrontendEvents.RemovePlayer:
                //TODO: sacar de waiting dict
                if (waitingDict.ContainsKey(data.id))
                {
                    waitingDict.Remove(data.id);
                }
                Player player;
                if(players.TryGetValue(data.id, out player))
                {
                    GameObject.Destroy(player.gameObject);
                    players.Remove(data.id);
                }
                break;
            case FrontendEvents.GetInfo:
                NetworkObj.BasicStructure structure;
                ObjsStructure objsStructure = JsonUtility.FromJson<ObjsStructure>(msg);
                if (objsStructure.ms < lastMs && GameManager.isClient)
                {
                    Debug.Log("Ignoring info msg");
                    return;
                }
                lastMs = objsStructure.ms;
                NetworkObj obj;
                foreach (var str in objsStructure.objs)
                {
                    structure = JsonUtility.FromJson<NetworkObj.BasicStructure>(str);
                    if(NetworkObj.objsDict.TryGetValue(structure.key, out obj))
                    {
                        obj.SetInfo(str);
                    }
                }
                break;
            case FrontendEvents.ChangeScene:
                if (GameManager.isClient)
                {
                    SceneLoader.instance.LoadScene(data.id);
                }
                break;
        }
    }

    class ObjsStructure
    {
        public int evt;
        public int ms;
        public string[] objs;
    }

    public void SendNetworkObjs()
    {
        if (NetworkObj.allObjs.Count == 0) return;
        ObjsStructure objs = new ObjsStructure();
        //objs.ns = Time.
        objs.ms = (int)(Client.ms + 0.5d);
        List<string> allInfo = new List<string>();
        string info;
        foreach(var obj in NetworkObj.allObjs)
        {
            info = obj.CollectInfo();
            if(info != "" && info != null)
            {
                allInfo.Add(info);
            }
        }
        objs.objs = allInfo.ToArray();
        BackendEvents evt = BackendEvents.SendObjects;
        objs.evt = (int)evt;
        string json = JsonUtility.ToJson(objs);

        if (Client.instance != null && Client.instance.socket != null)
            Client.instance.socket.SendMessage(json);
    }

    override public void Finish()
    {
        base.Finish();
        foreach(var pair in players)
        {
            GameObject.Destroy(pair.Value.gameObject);
        }
        players.Clear();
        waitingDict.Clear();
    }
}
