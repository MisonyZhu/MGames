using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Framework;
using UnityEngine;

namespace Game
{
    public class TcpConnection : NetConnection
    {
        TcpClient m_Tcp;

        public int sendTimeout = 30000;
        public bool disconnectOnSendException = false;

        // 检查连接状态时间间隔
        public float checkConnectInterval
        {
            get;
            set;
        } = 1;

        public override void Update()
        {
            CheckConnect();
            UpdateReceive();
            UpdateEvents();
        }

        #region connect
        public override async void Connect(string host, int port, Action<bool> callback = null, float timeout = 5)
        {
            base.Connect(host, port, callback, timeout);

            var ipAddr = GetAddress(host, port);
            if (ipAddr == null)
            {
                OnConnectFailed();
                return;
            }

            try
            {
                m_Tcp = new TcpClient(ipAddr.AddressFamily);
                m_NetState = State.Connecting;
                var connectTask = m_Tcp.ConnectAsync(ipAddr, port);
                var timeoutTask = Task.Delay((int)(timeout * 1000));
                await Task.WhenAny(connectTask, timeoutTask);
            }
            catch (Exception e)
            {
                if (m_LogEnable) Debug.LogException(e);
                OnConnectFailed();
                return;
            }

            if (m_Tcp != null)
            {
                if (m_Tcp.Connected)
                {
                    OnConnectSuccess();
                }
                else
                {
                    OnConnectFailed();
                }
            }
        }

        protected override void InitNet()
        {
            base.InitNet();

            m_Tcp.SendTimeout = sendTimeout;
            m_Tcp.NoDelay = true;

            StartSendThread();
            StartReceiveThread();
        }

        public override void Close()
        {
            base.Close();

            if (m_Tcp != null)
            {
                m_Tcp.Close();
                m_Tcp = null;
            }

            StopSendThread();
            StopReceiveThread();
        }

        float m_LastCheck = 0;
        void CheckConnect()
        {
            if (m_NetState != State.Connected)
                return;

            if (Time.unscaledTime - m_LastCheck > checkConnectInterval)
            {
                m_LastCheck = Time.unscaledTime;
                if (!m_Tcp.Connected)
                {
                    if (m_LogEnable)
                    {
                        Debug.Log("TcpConnection.CheckConnect:not connected");
                    }
                    OnDisconnect();
                }
#if !UNITY_EDITOR
                else if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    if (m_LogEnable)
                    {
                        Debug.Log("TcpConnection.CheckConnect:internet not reachable");
                    }
                    OnDisconnect();
                }
#endif
            }
        }
        #endregion

        #region send
        volatile bool m_SendThreadRunning = false;
        Thread m_SendThread;
        object m_SendQueueLock = new object();
        List<INetPackage> m_SendQueue = new List<INetPackage>();
        List<INetPackage> m_SendQueueThread = new List<INetPackage>();
        NetSemaphore m_SendSemaphore = new NetSemaphore();

        void StartSendThread()
        {
            m_SendThreadRunning = true;
            if (m_SendThread == null)
            {
                m_SendThread = new Thread(SendThread);
                m_SendThread.Start();
            }
        }

        void StopSendThread()
        {
            m_SendThreadRunning = false;

            lock (m_SendQueueLock)
            {
                m_SendQueue.Clear();
                m_SendQueueThread.Clear();
            }

            if (m_SendThread != null)
            {
                m_SendSemaphore.ProduceResrouce();
                m_SendThread.Join();
                m_SendThread = null;
            }
        }

        void SendThread()
        {
            while (m_SendThreadRunning)
            {
                m_SendSemaphore.WaitResource();
                if (m_SendQueue.Count > 0)
                {
                    lock (m_SendQueueLock)
                    {
                        var temp = m_SendQueueThread;
                        m_SendQueueThread = m_SendQueue;
                        m_SendQueue = temp;
                    }

                    try
                    {
                        for (int i = 0; i < m_SendQueueThread.Count; ++i)
                        {
                            m_WriteMemoryStream.Position = 0;
                            m_SendQueueThread[i].Write(m_NetWriter);
                            m_Tcp.Client.Send(m_WriteMemoryStream.GetBuffer(), (int)m_WriteMemoryStream.Position, SocketFlags.None);
                        }
                    }
                    catch (Exception e)
                    {
                        AddEvent(OnSendException, e);
                        break;
                    }
                    finally
                    {
                        for (int i = 0; i < m_SendQueueThread.Count; ++i)
                        {
                            NetPackagePool.Recycle(m_SendQueueThread[i]);
                        }
                        m_SendQueueThread.Clear();
                    }
                }
            }
        }

        public override void SendPackage(INetPackage package)
        {
            if (m_NetState == State.Connected)
            {
                if (m_LogEnable) Debug.Log($"TcpConnection.Send id={package.id} length={package.bodyLength}");

                lock (m_SendQueueLock)
                {
                    m_SendQueue.Add(package);
                }
                m_SendSemaphore.ProduceResrouce();
            }
        }

        void OnSendException(object e)
        {
            if (m_LogEnable)
            {
                Debug.LogException((Exception)e);
            }

            if (disconnectOnSendException)
            {
                OnDisconnect();
            }
        }
        #endregion

        #region receive
        volatile bool m_ReceiveThreadRunning = false;
        Thread m_ReceiveThread;
        object m_ReceiveQueueLock = new object();
        List<INetPackage> m_ReceiveQueue = new List<INetPackage>();
        List<INetPackage> m_ReceiveQueueThread = new List<INetPackage>();
        int m_ReceiveCount = 0;

        const int RECEIVE_BUFFER_SIZE = 1024 * 10;
        byte[] m_ReceiveBuffer = new byte[RECEIVE_BUFFER_SIZE];

        void StartReceiveThread()
        {
            m_ReceiveThreadRunning = true;
            if (m_ReceiveThread == null)
            {
                m_ReceiveThread = new Thread(ReceiveThread);
                m_ReceiveThread.Start();
            }
        }

        void StopReceiveThread()
        {
            m_ReceiveThreadRunning = false;

            lock (m_ReceiveQueueLock)
            {
                m_ReceiveQueue.Clear();
                m_ReceiveQueueThread.Clear();
            }

            if (m_ReceiveThread != null)
            {
                m_ReceiveThread.Join();
                m_ReceiveThread = null;
            }
        }

        void ReceiveThread()
        {
            INetPackage package = NetPackagePool.Get();
            while (m_ReceiveThreadRunning)
            {
                try
                {
                    int readLength = m_Tcp.Client.Receive(m_ReceiveBuffer, 0, RECEIVE_BUFFER_SIZE, SocketFlags.None);
                    if (readLength <= 0)
                    {
                        throw new IOException("readLength <= 0");
                    }

                    m_ReadMemoryStream.Write(m_ReceiveBuffer, 0, readLength);
                    m_ReadMemoryStream.Position = 0;
                    int available = (int)m_ReadMemoryStream.Length;
                    while (package.Read(m_NetReader, available))
                    {
                        lock (m_ReceiveQueueLock)
                        {
                            m_ReceiveQueueThread.Add(package);
                        }
                        package = NetPackagePool.Get();
                        available = (int)(m_ReadMemoryStream.Length - m_ReadMemoryStream.Position);
                    }

                    if (m_ReadMemoryStream.Position != 0)
                    {
                        available = (int)(m_ReadMemoryStream.Length - m_ReadMemoryStream.Position);
                        if (available > 0)
                        {
                            var buffer = m_ReadMemoryStream.GetBuffer();
                            Buffer.BlockCopy(buffer, (int)m_ReadMemoryStream.Position, buffer, 0, available);
                        }
                        m_ReadMemoryStream.SetLength(available);
                    }
                    m_ReadMemoryStream.Position = available;
                }
                catch (Exception e)
                {
                    if (m_NetState != State.Closed)
                    {
                        AddEvent(OnReceiveException, e);
                    }
                    break;
                }
            }
        }

        void UpdateReceive()
        {
            if (m_NetState != State.Connected)
                return;

            int maxCount = MaxReceiveCountPerFrame;
            if (m_ReceiveCount < m_ReceiveQueue.Count)
            {
                int count = Mathf.Min(m_ReceiveQueue.Count - m_ReceiveCount, maxCount);
                for (int i = 0; i < count && i < m_ReceiveQueue.Count; ++i)
                {
                    Recieve(m_ReceiveQueue[m_ReceiveCount + i]);
                }
                m_ReceiveCount += count;
                maxCount -= count;
            }

            if (maxCount > 0 && m_ReceiveQueueThread.Count > 0)
            {
                m_ReceiveQueue.Clear();
                lock (m_ReceiveQueueLock)
                {
                    var temp = m_ReceiveQueue;
                    m_ReceiveQueue = m_ReceiveQueueThread;
                    m_ReceiveQueueThread = temp;
                }

                int count = Mathf.Min(m_ReceiveQueue.Count, maxCount);
                for (int i = 0; i < count && i < m_ReceiveQueue.Count; ++i)
                {
                    Recieve(m_ReceiveQueue[i]);
                }
                m_ReceiveCount = count;
            }
        }

        void OnReceiveException(object e)
        {
            if (m_LogEnable)
            {
                Debug.LogException((Exception)e);
            }
            OnDisconnect();
        }
        #endregion

        #region event
        struct Event
        {
            public Action<object> func;
            public object arg;
        }
        List<Event> m_Events = new List<Event>();

        void AddEvent(Action<object> func, object arg = null)
        {
            lock (m_Events)
            {
                m_Events.Add(new Event
                {
                    func = func,
                    arg = arg,
                });
            }
        }

        void UpdateEvents()
        {
            lock (m_Events)
            {
                for (int i = 0; i < m_Events.Count; ++i)
                {
                    var evt = m_Events[i];
                    evt.func?.Invoke(evt.arg);
                }
                m_Events.Clear();
            }
        }
        #endregion
    }
}