using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClientState : MonoBehaviour
{
    protected Client client = null;

    [SerializeField] UnityEvent onBegin;
    [SerializeField] UnityEvent onFinish;

    private void Awake()
    {
        if (client == null) client = GetComponentInParent<Client>();
    }

    public void HandleMessage(ref string msg)
    {

    }

    virtual public void Begin()
    {
        if(client == null) client = GetComponentInParent<Client>();
        gameObject.SetActive(true);
        onBegin.Invoke();
        client.socket.onMessageCallback = HandleMessage;
    }
    virtual public void Finish()
    {
        onFinish.Invoke();
        client.socket.onMessageCallback = null;


        gameObject.SetActive(false);
    }
}
