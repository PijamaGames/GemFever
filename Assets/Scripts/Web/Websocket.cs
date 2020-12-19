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
    [SerializeField] bool debugMessages = true;
    [SerializeField] bool initOnStart = true;
    [SerializeField] int messageSizeBytes = 4096;
    private bool connected = false;

    [Header("EVENTS")]
    [SerializeField] UnityEvent OnOpenEvent;
    [HideInInspector] public delegate void OnOpenCallback();
    [HideInInspector] public OnOpenCallback onOpenCallback =() => Debug.Log("[SOCKET] opened");

    [SerializeField] UnityEvent OnCloseEvent;
    [HideInInspector] public delegate void OnCloseCallback();
    [HideInInspector] public OnCloseCallback onCloseCallback = () => Debug.Log("[SOCKET] closed");

    [SerializeField] UnityEvent OnErrorEvent;
    [HideInInspector] public delegate void OnErrorCallback();
    [HideInInspector] public OnErrorCallback onErrorCallback = ()=>Debug.Log("[SOCKET] error");

    [HideInInspector] public delegate void OnMessageCallback(string err);
    [HideInInspector] public OnMessageCallback onMessageCallback;

    [DllImport("__Internal")]
    private static extern void InitWS(string invoker, string url);

    [DllImport("__Internal")]
    private static extern void SendWSMessage(string jsonMsg);

    private bool isWebGLPlatform = false;

    bool waitingMsg = false;

    //SOCKET VARS
    ClientWebSocket socket = null;

    private void Start()
    {
#if (!UNITY_EDITOR && UNITY_WEBGL)
        isWebGLPlatform = true;
#endif
        if (initOnStart) Init();

    }

    private void OnDestroy()
    {
        if(!isWebGLPlatform && connected)
        {
            socket.Abort();
            socket.Dispose();
        }
    }

    private void Update()
    {
        CheckState();
    }

    private void CheckState()
    {
        if (isWebGLPlatform) return;
        if (!connected && socket.State == WebSocketState.Open)
        {
            Open();
        }
        if (connected && socket.State == WebSocketState.Closed)
        {
            Close();
        }
        if(connected && !waitingMsg)
        {
            WaitForMessage();
        }
    }

    public async void Init()
    {
        if (connected) return;
        Debug.Log("TRYING TO INIT SOCKET");
        string url = urlList[urlIndex];
        if (isWebGLPlatform)
        {
            Debug.Log("IS WEB GL PLATFORM");
            InitWS(gameObject.name, url);
        }
        else
        {
            socket = new ClientWebSocket();
            await socket.ConnectAsync(new Uri(url), CancellationToken.None);
        }
    }

    public void SetMessageHandler(OnMessageCallback func)
    {
        onMessageCallback = func;
    }

    public void SendObj(object o)
    {
        string json = JsonUtility.ToJson(o);
        SendMessage(json);
    }

    private new void SendMessage(string json)
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
            ArraySegment<Byte> bytes = new ArraySegment<byte>(Encoding.UTF8.GetBytes(json));
            socket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
            sent = true;
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
                Debug.Log("COUNT: " + result.Count);
                byte[] msgBytes = bytes.Array;
                string json = Encoding.UTF8.GetString(msgBytes, 0, result.Count);
                msg += json;
                endOfMessage = result.EndOfMessage;
            }
            ReceiveMessage(msg);
        } catch
        {
            Debug.Log("[SOCKET] Error waiting message");
        }
        
        waitingMsg = false;
    }

    void Open()
    {
        connected = true;
        OnOpenEvent.Invoke();
        onOpenCallback();
        //SendObj(new Vector3(1, 2, 3));
    }

    void Close()
    {
        connected = false;
        OnCloseEvent.Invoke();
        onCloseCallback();
    }

    void Error()
    {
        connected = false;
        OnErrorEvent.Invoke();
        onErrorCallback();
    }

    void ReceiveMessage(string msg)
    {
        if (debugMessages) Debug.Log("[SOCKET] received message: " + msg);
        if(onMessageCallback != null) onMessageCallback(msg);
    }
}
