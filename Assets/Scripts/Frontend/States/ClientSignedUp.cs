using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSignedUp : ClientState
{
    private enum FrontendEvents { SignedIn };
    private enum BackendEvents {SignIn, Save };

    override public void Begin()
    {
        base.Begin();
        Debug.Log("Signed up");
        SceneLoader.instance.LoadCustomizeAvatarScene();
    }

    private class MsgStructure
    {
        public int evt;
        public string user;
        public bool hasEvent;
        public string spanishMsg;
        public string englishMsg;
    }

    public override void HandleMessage(ref string msg)
    {
        base.HandleMessage(ref msg);
        var data = JsonUtility.FromJson<MsgStructure>(msg);
        FrontendEvents evt = (FrontendEvents)data.evt;
        Debug.Log("EVENT: " + data.evt);
        User user = JsonUtility.FromJson<User>(data.user);
        switch (evt)
        {
            case FrontendEvents.SignedIn:
                Client.user = user;
                ClientSignedIn.hasEvent = data.hasEvent;
                ClientSignedIn.spanishMsg = data.spanishMsg;
                ClientSignedIn.englishMsg = data.englishMsg;
                Client.SetState(Client.signedInState);
                break;
        }
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

    public static void SignIn()
    {
        BackendEvents evt = BackendEvents.SignIn;
        var pairs = new KeyValuePair<string, object>[]
        {
            new KeyValuePair<string, object>("evt", UsefulFuncs.PrimitiveToJsonValue((int)evt)),
        };
        string msg = UsefulFuncs.CombineJsons(pairs);
        Client.instance.socket.SendMessage(msg);
    }

    override public void Finish()
    {
        base.Finish();
    }
}
