using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientConnected : ClientState
{
    override public void Begin()
    {
        base.Begin();
        SceneLoader.instance.LoadSingInScene();
    }

    override public void Finish()
    {
        base.Finish();
    }
}
