using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientInRoom : ClientState
{
    private enum FrontendEvents { Error, Exit };
    private enum BackendEvents { Exit };

    public static int error;

    override public void Begin()
    {
        base.Begin();
        SceneLoader.instance.LoadHubScene();
        Client.user = null;
        Debug.Log("In room");
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

    private class MsgStructure
    {
        public int evt = 0;
        public int error = -1;
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
        }
    }

    override public void Finish()
    {
        base.Finish();
    }
}
