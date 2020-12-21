using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSignedIn : ClientState
{
    private enum FrontendEvents { };
    private enum BackendEvents { };

    override public void Begin()
    {
        base.Begin();
        Debug.Log("Signed in");
    }

    public override void HandleMessage(ref string msg)
    {
        base.HandleMessage(ref msg);
    }

    override public void Finish()
    {
        base.Finish();
    }
}
