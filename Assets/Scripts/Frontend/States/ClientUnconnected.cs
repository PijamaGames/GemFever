using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientUnconnected : ClientState
{
    override public void Begin()
    {
        base.Begin();
        client.socket.onOpenCallback = () => client.SetState(client.connectedState);
        client.socket.Init();
    }

    override public void Finish()
    {
        base.Finish();
        client.socket.onOpenCallback = null;
    }
}
