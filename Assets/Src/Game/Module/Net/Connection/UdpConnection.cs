using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Framework;
using UnityEngine;

namespace Game
{
    public class UdpConnection : NetConnection
    {
        UdpClient m_Udp;

        public override void Update()
        {
            UpdateReceive();
        }

        #region connect
        public bool Connect(string host, int port)
        {
            base.Connect(host, port);

            var ipAddr = GetAddress(host, port);
            if (ipAddr == null)
            {
                OnConnectFailed();
                return false;
            }

            try
            {
                m_Udp = new UdpClient(ipAddr.AddressFamily);
                m_Udp.Connect(ipAddr, port);
                OnConnectSuccess();
            }
            catch (Exception e)
            {
                if (m_LogEnable) Debug.LogException(e);
                OnConnectFailed();
                return false;
            }
            return true;
        }

        protected override void InitNet()
        {
            base.InitNet();
            StartReceiveThread();
        }

        public override void Close()
        {
            base.Close();

            if (m_Udp != null)
            {
                m_Udp.Close();
                m_Udp = null;
            }

            StopReceiveThread();
        }
        #endregion

        #region send
        public override void SendPackage(INetPackage package)
        {
            if (m_Udp != null)
            {
                m_WriteMemoryStream.Position = 0;
                package.Write(m_NetWriter);
                m_Udp.Client.Send(m_WriteMemoryStream.GetBuffer(), (int)m_WriteMemoryStream.Position, SocketFlags.None);
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
            if (m_ReceiveThread != null)
            {
                m_ReceiveThread.Join();
                m_ReceiveThread = null;
            }
        }

        void ReceiveThread()
        {
            while (m_ReceiveThreadRunning)
            {
                try
                {
                    int readLength = m_Udp.Client.Receive(m_ReceiveBuffer, 0, RECEIVE_BUFFER_SIZE, SocketFlags.None);
                    if (readLength <= 0)
                    {
                        throw new IOException("readLength <= 0");
                    }

                    m_ReadMemoryStream.Write(m_ReceiveBuffer, 0, readLength);
                    m_ReadMemoryStream.Position = 0;
                    
                    INetPackage package = NetPackagePool.Get();
                    if (package.Read(m_NetReader, readLength))
                    {
                        lock (m_ReceiveQueueLock)
                        {
                            m_ReceiveQueueThread.Add(package);
                        }
                    }
                    m_ReadMemoryStream.Position = 0;
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
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
        #endregion
    }
}