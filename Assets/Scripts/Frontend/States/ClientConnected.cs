using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientConnected : ClientState
{
    private enum FrontendEvents { SignedIn, SignedUp, WrongData };
    private enum BackendEvents { SignIn, SignUp};

    public delegate void WrongDataHandler(int error);
    public static event WrongDataHandler wrongDataEvent;

    override public void Begin()
    {
        base.Begin();
        SceneLoader.instance.LoadSingInScene();
    }

    public static void SignUp()
    {
        BackendEvents evt = BackendEvents.SignUp;
        var pairs = new KeyValuePair<string, object>[]
        {
            new KeyValuePair<string, object>("evt", UsefulFuncs.PrimitiveToJsonValue((int)evt)),
            new KeyValuePair<string, object>("user", User.ToJson(Client.user)),
        };
        string msg = UsefulFuncs.CombineJsons(pairs);
        Client.instance.socket.SendMessage(msg);
    }

    public static void SignIn()
    {
        BackendEvents evt = BackendEvents.SignIn;
        var pairs = new KeyValuePair<string, object>[]
        {
            new KeyValuePair<string, object>("evt", UsefulFuncs.PrimitiveToJsonValue((int)evt)),
            new KeyValuePair<string, object>("username", UsefulFuncs.PrimitiveToJsonValue(Client.user.id)),
            new KeyValuePair<string, object>("password", UsefulFuncs.PrimitiveToJsonValue(Client.user.password)),
        };
        string msg = UsefulFuncs.CombineJsons(pairs);
        Client.instance.socket.SendMessage(msg);
    }

    private class MsgStructure
    {
        public int evt = 0;
        public int error = -1;
    }

    override public void HandleMessage(ref string msg)
    {
        base.HandleMessage(ref msg);
        var data = JsonUtility.FromJson<MsgStructure>(msg);
        FrontendEvents evt = (FrontendEvents)data.evt;
        Debug.Log("EVENT: " + data.evt);
        switch (evt)
        {
            case FrontendEvents.SignedIn:
                Client.SetState(Client.signedInState);
                break;
            case FrontendEvents.SignedUp:
                Client.SetState(Client.signedUpState);
                break;
            case FrontendEvents.WrongData:
                wrongDataEvent.Invoke(data.error);
                break;
        }
    }

    override public void Finish()
    {
        base.Finish();
    }
}
