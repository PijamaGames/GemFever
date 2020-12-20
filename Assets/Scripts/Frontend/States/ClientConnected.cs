using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientConnected : ClientState
{
    private enum FrontendEvents { SignedIn };
    private enum BackendEvents { SignIn, SignUp};

    override public void Begin()
    {
        base.Begin();
        SceneLoader.instance.LoadSingInScene();


        SignUp();
    }

    public void SignUp()
    {
        Client.user = new User();
        Client.user.id = "Pedro";
        Client.user.password = "la vida es dura";
        BackendEvents evt = BackendEvents.SignUp;
        var pairs = new KeyValuePair<string, object>[]
        {
            new KeyValuePair<string, object>("event", UsefulFuncs.PrimitiveToJsonValue((int)evt)),
            new KeyValuePair<string, object>("user", User.ToJson(Client.user)),
        };
        string msg = UsefulFuncs.CombineJsons(pairs);
        client.socket.SendMessage(msg);
    }

    override public void Finish()
    {
        base.Finish();
    }
}
