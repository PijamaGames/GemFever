using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.Events;
using System.Net.WebSockets;
using System;
using System.Threading;
using System.Text;

public class Websocket : MonoBehaviour
{
    [SerializeField] int urlIndex = 0;
    [SerializeField][TextArea] string[] urlList;
    [SerializeField] bool debugMessages = false;
    [SerializeField] bool debugState = false;
    [SerializeField] bool initOnAwake = false;
    //[SerializeField] bool tryReconnect = true; //will be true in build
    [SerializeField] float keepAliveInterval = 25f;
    [SerializeField] int messageSizeBytes = 4096;
    private bool connected = false;

    [Header("EVENTS")]
    [SerializeField] UnityEvent OnOpenEvent;
    [HideInInspector] public delegate void OnOpenCallback();
    [HideInInspector] public OnOpenCallback onOpenCallback = null;

    [SerializeField] UnityEvent OnCloseEvent;
    [HideInInspector] public delegate void OnCloseCallback();
    [HideInInspector] public OnCloseCallback onCloseCallback = null;

    [SerializeField] UnityEvent OnErrorEvent;
    [HideInInspector] public delegate void OnErrorCallback(string err);
    [HideInInspector] public OnErrorCallback onErrorCallback = null;

    [HideInInspector] public delegate void OnMessageCallback(ref string msg);
    [HideInInspector] public OnMessageCallback onMessageCallback;

    [DllImport("__Internal")]
    private static extern void InitWS(string invoker, string url);

    [DllImport("__Internal")]
    private static extern void SendWSMessage(string jsonMsg);

    private bool isWebGLPlatform = false;

    bool waitingMsg = false;

    //SOCKET VARS
    ClientWebSocket socket = null;

    private void Awake()
    {
/*#if !UNITY_EDITOR
        tryReconnect = true;
#endif*/

#if (!UNITY_EDITOR && UNITY_WEBGL)
        isWebGLPlatform = true;
#endif
        if (initOnAwake) Init();

    }

    private void OnDestroy()
    {
        if(!isWebGLPlatform && connected && socket != null)
        {
            socket.Abort();
            socket.Dispose();
        }
    }

    private void Update()
    {
        if(socket != null)
            CheckState();
    }

    private void CheckState()
    {
        if (isWebGLPlatform) return;
        if (!connected && socket.State == WebSocketState.Open)
        {
            Open();
        }
        if (connected && socket.State == WebSocketState.Closed || socket.State == WebSocketState.CloseReceived)
        {
            socket.Abort();
            Close();
        }
        if(connected && !waitingMsg)
        {
            WaitForMessage();
        }
        if (debugState)
        {
            Debug.Log("[SOCKET] State: " + socket.State);
        }
    }

    public async void Init()
    {
        if (connected) return;
        if (!Application.isPlaying) return;
        string url = urlList[urlIndex];
        Debug.Log("[SOCKET] CONNECTING TO: " + url);
        if (isWebGLPlatform)
        {
            InitWS(gameObject.name, url);
        }
        else
        {
            socket = new ClientWebSocket();
            try
            {
                await socket.ConnectAsync(new Uri(url), CancellationToken.None);
            } catch
            {
                Error("connecting socket");
                //socket.Abort();
                //socket.Dispose();
                /*if (tryReconnect && Application.isPlaying)
                {
                    Init();
                }*/
            }
            
        }
    }

    public void SetMessageHandler(OnMessageCallback func)
    {
        onMessageCallback = func;
    }

    public void SendObj(object obj)
    {
        string json = JsonUtility.ToJson(obj);
        SendMessage(json);
    }

    public new void SendMessage(string json)
    {
        if (!connected) return;
        bool sent = false;
        if (isWebGLPlatform)
        {
            SendWSMessage(json);
            sent = true;
        }
        else if (socket.State == WebSocketState.Open)
        {
            try
            {
                ArraySegment<Byte> bytes = new ArraySegment<byte>(Encoding.UTF8.GetBytes(json));
                socket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
                sent = true;
            }
            catch
            {
                Error("sending message");
            }
            
        }
        if (debugMessages && sent)
        {
            Debug.Log("[SOCKET] sent message: " + json);
        }
    }
    private async void WaitForMessage()
    {
        waitingMsg = true;
        ArraySegment<Byte> bytes = new ArraySegment<byte>(new byte[messageSizeBytes]);
        try
        {
            bool endOfMessage = false;
            string msg = "";
            while (!endOfMessage)
            {
                WebSocketReceiveResult result = await socket.ReceiveAsync(bytes, CancellationToken.None);
                byte[] msgBytes = bytes.Array;
                string json = Encoding.UTF8.GetString(msgBytes, 0, result.Count);
                msg += json;
                endOfMessage = result.EndOfMessage;
            }
            ReceiveMessage(msg);
        } catch
        {
            Debug.Log("[SOCKET] Error waiting message...");
        }
        
        waitingMsg = false;
    }

    void Open()
    {
        if (!Application.isPlaying) return;
        Debug.Log("[SOCKET] Opened");
        connected = true;
        OnOpenEvent.Invoke();
        onOpenCallback?.Invoke();
        //StopCoroutine("KeepAliveCoroutine");
        StartCoroutine(KeepAliveCoroutine());
    }

    void Close()
    {
        if (!Application.isPlaying) return;
        Debug.Log("[SOCKET] Closed");
        connected = false;
        OnCloseEvent.Invoke();
        onCloseCallback?.Invoke();
    }

    void Error(string err = "")
    {
        if (!Application.isPlaying) return;
        Debug.Log("[SOCKET] Error: " + err);
        connected = false;
        OnErrorEvent.Invoke();
        onErrorCallback?.Invoke(err);
    }

    void ReceiveMessage(string msg)
    {
        if (!Application.isPlaying) return;
        if (debugMessages) Debug.Log("[SOCKET] received message: " + msg);
        onMessageCallback?.Invoke(ref msg);
    }

    IEnumerator KeepAliveCoroutine()
    {
        while(socket.State == WebSocketState.Open)
        {
            SendMessage("{\"evt\":\"-1\"}");
            yield return new WaitForSeconds(keepAliveInterval);
        }
    }
}
