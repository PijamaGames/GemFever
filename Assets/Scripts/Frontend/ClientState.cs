using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClientState
{

    virtual public void HandleMessage(ref string msg)
    {

    }

    virtual public void Begin()
    {
        Client.instance.socket.onMessageCallback = this.HandleMessage;
    }

    virtual public void Finish()
    {
        Client.instance.socket.onMessageCallback = null;
    }
}
