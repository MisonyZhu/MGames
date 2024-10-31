using System;
using System.IO;
using System.Net;

namespace Framework
{
    public abstract class NetConnection
    {
        public enum State
        {
            Closed,
            Connecting,
            Connected,
        }

        protected State m_NetState;
        protected bool m_LogEnable;

        protected MemoryStream m_ReadMemoryStream;
        protected MemoryStream m_WriteMemoryStream;
        protected BinaryReader m_NetReader;
        protected BinaryWriter m_NetWriter;

        public bool IsConnected => NetState == State.Connected;

        public INetPackagePool NetPackagePool { get;  set; } = new NetPackagePool<DefaultNetPackage>();

        public INetReceiver NetReceiver { get; set; }
        public State NetState
        {
            get { return m_NetState; }
            set { m_NetState = value; }
        }
        
        // 每帧最大接收消息数目
        public int MaxReceiveCountPerFrame
        {
            get;
            set;
        } = 100;

        // 连接断开回调
        public event Action onDisconnect;

        ~NetConnection()
        {
            Close();
        }

        protected static IPAddress GetAddress(string host, int port)
        {
            IPAddress[] ipAddrs = null;
            try
            {
                ipAddrs = Dns.GetHostAddresses(host);
            }
            catch (Exception e)
            {
                Log.LogError(e.ToString());
                return null;
            }

            if (ipAddrs == null || ipAddrs.Length == 0)
            {
                return null;
            }

            return ipAddrs[0];
        }

        public virtual void Update()
        {
            
        }

        #region Init

        protected virtual void InitNet()
        {
            m_NetState = State.Closed;
            m_ReadMemoryStream = new MemoryStream();
            m_WriteMemoryStream = new MemoryStream();
            m_NetReader = new BinaryReader(m_ReadMemoryStream);
            m_NetWriter = new BinaryWriter(m_WriteMemoryStream);
        }

        #endregion

        #region Connect

        private Action<bool> m_ConnectCallback;

        public virtual void Connect(string ip, int port, Action<bool> call = null, float timeOut = 5)
        {
            if (m_LogEnable)
            {
                Log.LogMsg($"Net Connect:{ip}:{port}");
            }

            m_ConnectCallback = call;
        }

        public virtual void Connect(string ip, int port, uint conv, Action<bool> call = null, float timeOut = 5)
        {
            if (m_LogEnable)
            {
                Log.LogMsg($"Net Connect:{ip}:{port}");
            }

            m_ConnectCallback = call;
        }
        
        protected virtual void OnConnectFailed()
        {
            Close();
            if (m_ConnectCallback != null)
            {
                m_ConnectCallback(false);
                m_ConnectCallback = null;
            }
        }

        protected virtual void OnConnectSuccess()
        {
            InitNet();
            if (m_ConnectCallback != null)
            {
                m_ConnectCallback(true);
                m_ConnectCallback = null;
            }
        }

        protected virtual void OnDisconnect()
        {
            if (IsConnected)
            {
                Close();
                onDisconnect?.Invoke();
            }
        }

        #endregion

        #region Send

        public virtual void Send(int id, byte[] data)
        {
            if (IsConnected)
            {
                var package = NetPackagePool.Get();
                package.id = id;
                package.body = data;
                package.bodyLength = data == null ? data.Length : 0;
                SendPackage(package);
            }
        }

        public abstract void SendPackage(INetPackage package);


        #endregion

        #region  Receiver

        public virtual void Recieve(INetPackage package)
        {
            if (m_LogEnable)
            {
                Log.LogMsg($"Net Receive {package.id} : {package.bodyLength}");
            }
            try
            {
                NetReceiver?.OnReceive(package);
            }
            catch (Exception e)
            {
                Log.LogError($"Net Receive  Error: {package.id} : {e}");
            }
            BufferPool.Recycle(package.body);
            NetPackagePool.Recycle(package);
        }

        #endregion

        #region  Close

        public virtual void Close()
        {
            m_NetState = State.Closed;

            if (m_NetReader != null)
            {
                m_NetReader.Close();
                m_NetReader = null;
            }

            if (m_ReadMemoryStream != null)
            {
                m_ReadMemoryStream.Close();
                m_ReadMemoryStream = null;
            }

            if (m_NetWriter != null)
            {
                m_NetWriter.Close();
                m_NetWriter = null;
            }

            if (m_WriteMemoryStream != null)
            {
                m_WriteMemoryStream.Close();
                m_WriteMemoryStream = null;
            }
        }

        #endregion
    }
}