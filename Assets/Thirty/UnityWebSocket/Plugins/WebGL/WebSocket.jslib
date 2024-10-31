var WebSocketLibrary =
{
	$webSocketManager:
	{
		/*
		 * Map of instanceInfos
		 * ws 和 wk 互斥
		 * Instance structure:
		 * {
		 * 	url: string
		 * 	ws: WebSocket
		 * 	wk: Worker
		 * 	rystate:readyState
		 * }
		 */
		instanceInfos: {},

		/* Last instance ID */
		lastId: 0,

		/* Event listeners */
		onOpen: null,
		onMessage: null,
		onError: null,
		onClose: null,
	},

	/**
	 * Set onOpen callback
	 *
	 * @param callback Reference to C# static function
	 */
	WebSocketSetOnOpen: function(callback)
	{
		webSocketManager.onOpen = callback;
	},

	/**
	 * Set onMessage callback
	 *
	 * @param callback Reference to C# static function
	 */
	WebSocketSetOnMessage: function(callback)
	{
		webSocketManager.onMessage = callback;
	},

	/**
	 * Set onMessage callback
	 *
	 * @param callback Reference to C# static function
	 */
	WebSocketSetOnMessageStr: function(callback)
	{
		webSocketManager.onMessageStr = callback;
	},

	/**
	 * Set onError callback
	 *
	 * @param callback Reference to C# static function
	 */
	WebSocketSetOnError: function(callback)
	{
		webSocketManager.onError = callback;
	},

	/**
	 * Set onClose callback
	 *
	 * @param callback Reference to C# static function
	 */
	WebSocketSetOnClose: function(callback)
	{
		webSocketManager.onClose = callback;
	},

	/**
	 * Allocate new WebSocket instance struct
	 *
	 * @param url Server URL
	 */
	WebSocketAllocate: function(url)
	{
		var urlStr = UTF8ToString(url);
		var id = ++webSocketManager.lastId;
		webSocketManager.instanceInfos[id] = {
			url:urlStr,
			ws:null,
			wk:null,
			rystate:0
		};
		return id;
	},

	/**
	 * Remove reference to WebSocket instance
	 *
	 * If socket is not closed function will close it but onClose event will not be emitted because
	 * this function should be invoked by C# WebSocket destructor.
	 *
	 * @param instanceId Instance ID
	 */
	WebSocketFree: function(instanceId)
	{
		var instance = webSocketManager.instanceInfos[instanceId];
		if (!instance) return 0;
		// Close if not closed
		if (instance.ws !== null && instance.ws.readyState < 2)
			instance.ws.close();
		if(instance.wk !== null)
			instance.wk.postMessage({cmd: 'stop'});
		// Remove reference
		delete webSocketManager.instanceInfos[instanceId];
		return 0;
	},
	
	/**
	 * Connect WebSocket to the server
	 *
	 * @param instanceId Instance ID
	 */
	WebSocketConnect: function(instanceId,isWorker)
	{
		var instance = webSocketManager.instanceInfos[instanceId];
		if (!instance) return -1;
		if(isWorker==true){
			if (instance.wk !== null) return -2;
			instance.wk = new Worker("worker.js");
			instance.wk.addEventListener("message" ,function (e)
			{	var method = e.data.method;
				var mdata = e.data;
				if(method == "onopen"){
					if (webSocketManager.onOpen){
						Module.dynCall_vi(webSocketManager.onOpen, instanceId);
					}
				}else if(method == "onmessage"){
					if (webSocketManager.onMessage === null)return;
					if (mdata.msg instanceof ArrayBuffer)
					{
						var dataBuffer = new Uint8Array(mdata.msg);
						var buffer = _malloc(dataBuffer.length);
						HEAPU8.set(dataBuffer, buffer);
						try
						{
							Module.dynCall_viii(webSocketManager.onMessage, instanceId, buffer, dataBuffer.length);
						}
						finally
						{
							_free(buffer);
						}
					}
					else if (mdata.msg instanceof Blob)
					{
						var reader = new FileReader();
						reader.addEventListener("loadend", function()
						{
							var dataBuffer = new Uint8Array(reader.result);
							var buffer = _malloc(dataBuffer.length);
							HEAPU8.set(dataBuffer, buffer);
							try
							{
								Module.dynCall_viii(webSocketManager.onMessage, instanceId, buffer, dataBuffer.length);
							}
							finally
							{
								reader = null;
								_free(buffer);
							}
						});
						reader.readAsArrayBuffer(mdata.msg);
					}
					else if(typeof mdata.msg == 'string')
					{
						var length = lengthBytesUTF8(mdata.msg) + 1;
						var buffer = _malloc(length);
						stringToUTF8(mdata.msg, buffer, length);
						try
						{
							Module.dynCall_vii(webSocketManager.onMessageStr, instanceId, buffer);
						}
						finally
						{
							_free(buffer);
						}
					}
					else
					{
						console.log("[JSLIB Worker WebSocket] not support message type: ", (typeof mdata.msg));
					}
				}else if(method == "onclose"){
					if (webSocketManager.onClose)
					{
						var msg = mdata.reason;
						var length = lengthBytesUTF8(msg) + 1;
						var buffer = _malloc(length);
						stringToUTF8(msg, buffer, length);
						try
						{
							Module.dynCall_viii(webSocketManager.onClose, instanceId, mdata.code, buffer);
						}
						finally
						{
							_free(buffer);
						}
					}
					instance.wk = null;
			   
				}else if(method == "onerror"){
					if(webSocketManager.onError){
						var msg = "Worker WebSocket error.";
						var length = lengthBytesUTF8(msg) + 1;
						var buffer = _malloc(length);
						stringToUTF8(msg, buffer, length);
						try
						{
							Module.dynCall_vii(webSocketManager.onError, instanceId, buffer);
						}
						finally
						{
							_free(buffer);
						}
					}
				}else if(method == "readystate"){
					instance.rystate = ev;
				}
			})
			instance.wk.postMessage({cmd: 'start',msg:instance.url});
		}else{
			if (instance.ws !== null) return -2;
			instance.ws = new WebSocket(instance.url);
			instance.ws.onopen = function()
			{
				if (webSocketManager.onOpen)
					Module.dynCall_vi(webSocketManager.onOpen, instanceId);
			};
	
			instance.ws.onmessage = function(ev)
			{
				if (webSocketManager.onMessage === null)
					return;
	
				if (ev.data instanceof ArrayBuffer)
				{
					var dataBuffer = new Uint8Array(ev.data);
					var buffer = _malloc(dataBuffer.length);
					HEAPU8.set(dataBuffer, buffer);
					try
					{
						Module.dynCall_viii(webSocketManager.onMessage, instanceId, buffer, dataBuffer.length);
					}
					finally
					{
						_free(buffer);
					}
				}
				else if (ev.data instanceof Blob)
				{
					var reader = new FileReader();
					reader.addEventListener("loadend", function()
					{
						var dataBuffer = new Uint8Array(reader.result);
						var buffer = _malloc(dataBuffer.length);
						HEAPU8.set(dataBuffer, buffer);
						try
						{
							Module.dynCall_viii(webSocketManager.onMessage, instanceId, buffer, dataBuffer.length);
						}
						finally
						{
							reader = null;
							_free(buffer);
						}
					});
					reader.readAsArrayBuffer(ev.data);
				}
				else if(typeof ev.data == 'string')
				{
					var length = lengthBytesUTF8(ev.data) + 1;
					var buffer = _malloc(length);
					stringToUTF8(ev.data, buffer, length);
					try
					{
						Module.dynCall_vii(webSocketManager.onMessageStr, instanceId, buffer);
					}
					finally
					{
						_free(buffer);
					}
				}
				else
				{
					console.log("[JSLIB WebSocket] not support message type: ", (typeof ev.data));
				}
			};
	
			instance.ws.onerror = function(ev)
			{
				if (webSocketManager.onError)
				{
					var msg = "WebSocket error.";
					var length = lengthBytesUTF8(msg) + 1;
					var buffer = _malloc(length);
					stringToUTF8(msg, buffer, length);
					try
					{
						Module.dynCall_vii(webSocketManager.onError, instanceId, buffer);
					}
					finally
					{
						_free(buffer);
					}
				}
			};
	
			instance.ws.onclose = function(ev)
			{
				if (webSocketManager.onClose)
				{
					var msg = ev.reason;
					var length = lengthBytesUTF8(msg) + 1;
					var buffer = _malloc(length);
					stringToUTF8(msg, buffer, length);
					try
					{
						Module.dynCall_viii(webSocketManager.onClose, instanceId, ev.code, buffer);
					}
					finally
					{
						_free(buffer);
					}
				}
	
				instance.ws = null;
			};
		}


		return 0;
	},

	/**
	 * Close WebSocket connection
	 *
	 * @param instanceId Instance ID
	 * @param code Close status code
	 * @param reasonPtr Pointer to reason string
	 */
	WebSocketClose: function(instanceId, code, reasonPtr)
	{
		var instance = webSocketManager.instanceInfos[instanceId];
		if (!instance) return -1;
		var reason = ( reasonPtr ? UTF8ToString(reasonPtr) : undefined );
		if(instance.wk){
			try
			{
				instance.wk.postMessage({cmd:'close',code:code,reason:reason});
			}
			catch(err)
			{
				return -7;
			}
		}else{
			if (instance.ws === null) return -3;
			if (instance.ws.readyState === 2) return -4;
			if (instance.ws.readyState === 3) return -5;
			try
			{
				instance.ws.close(code, reason);
			}
			catch(err)
			{
				return -7;
			}
		}
		return 0;
	},

	/**
	 * Send message over WebSocket
	 *
	 * @param instanceId Instance ID
	 * @param bufferPtr Pointer to the message buffer
	 * @param length Length of the message in the buffer
	 */
	WebSocketSend: function(instanceId, bufferPtr, length)
	{
		var instance = webSocketManager.instanceInfos[instanceId];
		if (!instance) return -1;
		var value = HEAPU8.slice(bufferPtr, bufferPtr + length).buffer;
		if(instance.wk){
			instance.wk.postMessage({cmd: 'sendMsg',msg:value});
		}else{
			if (instance.ws === null) return -3;
			if (instance.ws.readyState !== 1) return -6;
			instance.ws.send(value);
		}
		return 0;
	},

	/**
	 * Send message string over WebSocket
	 *
	 * @param instanceId Instance ID
	 * @param stringPtr Pointer to the message string
	 */
	WebSocketSendStr: function(instanceId, stringPtr)
	{
		var instance = webSocketManager.instanceInfos[instanceId];
		if (!instance) return -1;
		var value = UTF8ToString(stringPtr)
		if(instance.wk){
			instance.wk.postMessage({cmd: 'sendMsg',msg:value});
		}else{
			if (instance.ws === null) return -3;
			if (instance.ws.readyState !== 1) return -6;
			instance.ws.send(value);
		}
		return 0;
	},

	/**
	 * Return WebSocket readyState
	 *
	 * @param instanceId Instance ID
	 */
	WebSocketGetState: function(instanceId)
	{
		var instance = webSocketManager.instanceInfos[instanceId];
		if (!instance) return -1;
		if(instance.wk){
			instance.wk.postMessage({cmd:'getReadyState'});
			//worker如何获取WebSocket的readyState
			return instance.rystate;
		}else{
			if (instance.ws === null) return 3;
			return instance.ws.readyState;
		}
	}
};

autoAddDeps(WebSocketLibrary, '$webSocketManager');
mergeInto(LibraryManager.library, WebSocketLibrary);
