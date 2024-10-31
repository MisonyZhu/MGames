#if UNITY_WEBGL
using System;
using UnityEngine;
using UnityWebSocket;
using ErrorEventArgs = UnityWebSocket.ErrorEventArgs;
using Framework;

namespace Game
{
    public class WebConnection : NetConnection
    {
        WebSocket m_WebSocket;

        public override void Connect(string host, int port, Action<bool> callback = null, float timeout = 5)
        {
            base.Connect(host, port, callback, timeout);

            var url = $"ws://{host}:{port}";
            try
            {
                m_WebSocket = new WebSocket(url, true);
                m_WebSocket.OnOpen += OnOpen;
                m_WebSocket.OnError += OnError;
                m_WebSocket.OnClose += OnClose;
                m_WebSocket.OnMessage += OnMessage;

                m_NetState = State.Connecting;
                m_WebSocket.ConnectAsync();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.ToString());
                OnConnectFailed();
            }
        }

        void OnOpen(object sender, OpenEventArgs args)
        {
            if (m_LogEnable) Debug.Log("WebConnection.OnOpen");
            OnConnectSuccess();
        }

        void OnError(object sender, ErrorEventArgs args)
        {
            if (m_LogEnable) Debug.LogError("WebConnection.OnError:" + args.Message);

            switch (m_NetState)
            {
                case State.Connecting:
                    OnConnectFailed();
                    break;
            }
        }

        void OnClose(object sender, CloseEventArgs args)
        {
            if (m_LogEnable) Debug.Log("WebConnection.OnClose");
            OnDisconnect();
        }

        void OnMessage(object sender, MessageEventArgs args)
        {
            m_ReadMemoryStream.Position = 0;
            m_ReadMemoryStream.Write(args.RawData, 0, args.RawData.Length);
            m_ReadMemoryStream.SetLength(args.RawData.Length);
            m_ReadMemoryStream.Position = 0;

            int available = (int)m_ReadMemoryStream.Length;
            while (available > 0)
            {
                var package = NetPackagePool.Get();
                if (package.Read(m_NetReader, available))
                {
                    Recieve(package);
                    available = (int)(m_ReadMemoryStream.Length - m_ReadMemoryStream.Position);
                }
                else
                    break;
            }
        }

        public override void Close()
        {
            base.Close();

            if (m_WebSocket != null)
            {
                m_WebSocket.CloseAsync();
                m_WebSocket = null;
            }
        }

        public override void SendPackage(INetPackage package)
        {
            if (!IsConnected) return;
            m_WriteMemoryStream.Position = 0;
            package.Write(m_NetWriter);
            m_WebSocket.SendAsync(m_WriteMemoryStream.GetBuffer(), (int)m_WriteMemoryStream.Position);
            if (m_LogEnable) Debug.Log($"WebConnection.Send id={package.id} length={package.bodyLength}m_WriteMemoryStream.Position id={(int)m_WriteMemoryStream.Position}");
        }
    }
}
#endif