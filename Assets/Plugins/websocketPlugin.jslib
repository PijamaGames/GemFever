mergeInto(LibraryManager.library, {
  socket: null,
  InitWS__deps:['socket'],
  InitWS: function(invoker, url){
    invoker = Pointer_stringify(invoker);
    url = Pointer_stringify(url);
    console.log("[WEBSOCKET] INIT Invoker: " + invoker + " URL: " + url);
    _socket = new WebSocket(url);
    _socket.onopen = function(){
      console.log("[WEBSOCKET] OPEN");
      gameInstance.SendMessage(invoker, "Open");
    };
    _socket.onclose = function(){
      console.log("[WEBSOCKET] CLOSE");
      gameInstance.SendMessage(invoker, "Close");
    };
    _socket.onerror = function(evt){
      console.log("[WEBSOCKET] ERROR");
      gameInstance.SendMessage(invoker, "Error", "")
    };
    _socket.onmessage = function(msg){
      gameInstance.SendMessage(invoker, "ReceiveMessage", msg.data);
    };
  },
  SendWSMessage__deps:['socket'],
  SendWSMessage: function(jsonMsg){
    jsonMsg = Pointer_stringify(jsonMsg);
    _socket.send(jsonMsg);
  },
});
