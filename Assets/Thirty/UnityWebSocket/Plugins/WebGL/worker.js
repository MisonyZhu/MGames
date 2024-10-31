var ws;
self.addEventListener('message', function (e) {
    var data = e.data;
    switch (data.cmd) 
    {
        case 'start':
            createWebSocket(data.msg);
            break;
        case 'stop':
            cleanSocket();
            break;
        case 'close':
            closeSocket(data.code,data.reason);
            break;
        case 'sendMsg':
            if(ws && 1 == ws.readyState){
                ws.send(data.msg)
            }else{
                console.log("worker ws---------------连接已经断开！");
            }   
            break;
        case 'getReadyState':
            getReadyState();
            break;
        default:
            self.postMessage('worker Unknown command: ' + data.msg);
    };
  }, false);

  function createWebSocket(ws_url){
    if(ws&&1 != ws.readyState)
    {
        cleanSocket();
    }
    ws = new WebSocket(ws_url);
    //ws.binaryType= 'arraybuffer';
    ws.onopen = function(e) {
        self.postMessage({method: 'onopen',msg:e.data,code:e.code});
        console.log("worker postMessage---------------onopen！");
    }
    ws.onmessage = function(e) {
        self.postMessage({method: 'onmessage',msg:e.data,code:e.code} );
        console.log("worker postMessage---------------onmessage！");
    }
    ws.onclose = function(e) {
        self.postMessage({method: 'onclose',msg:e.data,code:e.code,reason:"reason"});
        console.log("worker postMessage---------------onclose！");
    }
    ws.onerror = function(e) {
        self.postMessage({method: 'onerror'});
        console.log("worker postMessage---------------onerror！");
    }
}
function getReadyState(){
    var readystate = 0;
    if(ws){
        readystate = ws.readyState;
    }
    self.postMessage({method: 'readystate',msg:readystate});
}
function closeSocket(code,reason) {
    if(ws){
        try {
            ws.close();
            console.log("worker ws---------------closeSocket|"+code+"|"+reason);
        } catch (e) {
            console.log("worker ws.close----------------出错"+e);
        }
    }
}

function cleanSocket() {
    if(ws){
        try {
            ws.close();
            console.log("worker ws---------------cleanSocket");
        } catch (e) {
            console.log("worker ws.close----------------出错"+e);
        }
        ws.onopen = null;
        ws.onmessage = null;
        ws.onclose = null;
        ws.onerror = null;
        ws = null;
    }
}