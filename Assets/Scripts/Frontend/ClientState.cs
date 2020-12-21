using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClientState : MonoBehaviour
{
    protected static Client client = null;

    [SerializeField] UnityEvent onBegin;
    [SerializeField] UnityEvent onFinish;

    private void Awake()
    {
        if (client == null) client = GetComponentInParent<Client>();
    }

    virtual public void HandleMessage(ref string msg)
    {

    }

    virtual public void Begin()
    {
        if(client == null) client = GetComponentInParent<Client>();
        gameObject.SetActive(true);
        onBegin.Invoke();
        client.socket.onMessageCallback = this.HandleMessage;
        
    }
    virtual public void Finish()
    {
        onFinish.Invoke();
        client.socket.onMessageCallback = null;


        gameObject.SetActive(false);
    }
}
