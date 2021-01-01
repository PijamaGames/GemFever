using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClientSignedIn : ClientState
{
    private enum FrontendEvents { SignedOut };
    private enum BackendEvents { SignOut, Save };

    public static event Action signedOutEvent;

    public static bool hasEvent = false;
    public static string spanishMsg = "";
    public static string englishMsg = "";

    override public void Begin()
    {
        base.Begin();
        Debug.Log("Signed in");
        Debug.Log("HAS EVENT: " + hasEvent);
        if (hasEvent)
        {
            Debug.Log("SPANISH MSG EVENT: " + spanishMsg);
            Debug.Log("ENGLISH MSG EVENT: " + englishMsg);
            SceneLoader.instance.LoadEventViewScene();
        } else
        {
            SceneLoader.instance.LoadMainMenuScene();
        }
        GameManager.instance.SavePreferences();
    }

    private class MsgStructure
    {
        public int evt = 0;
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
                Debug.Log(00);
                Client.SetState(Client.connectedState);
                Debug.Log(10);
                signedOutEvent?.Invoke();
                Debug.Log(20);
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
