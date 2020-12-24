using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientUnconnected : ClientState
{

    override public void Begin()
    {
        base.Begin();
        Client.instance.socket.onOpenCallback = () => Client.SetState(Client.connectedState);
        Client.instance.socket.Init();
        SceneLoader.instance.LoadConnectionScene();
    }

    public override void HandleMessage(ref string msg)
    {
        base.HandleMessage(ref msg);
    }

    override public void Finish()
    {
        base.Finish();
        Client.instance.socket.onOpenCallback = null;
    }
}
