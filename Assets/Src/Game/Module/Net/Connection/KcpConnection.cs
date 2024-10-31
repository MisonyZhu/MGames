using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Framework;
using UnityEngine;

namespace Game
{
    public class KcpConnection : NetConnection
    {
        UdpClient m_Udp;
        KCP m_KCP;
        public int sendTimeout = 30000;
        public int receiveTimeout = 30000;


        public override void Update()
        {
            m_KCP.Update();
            UpdateReceive();
        }

        #region connent

        public override void Connect(string host, int port, uint conv, Action<bool> callback = null,float timeOut =5)
        {
            base.Connect(host, port, conv, callback);
            var ipAddr = GetAddress(host, port);
            if (ipAddr == null)
            {
                OnConnectFailed();
            }

            try
            {
                m_Udp = new UdpClient(ipAddr.AddressFamily);
                m_Udp.Client.SendTimeout = sendTimeout;
                m_Udp.Client.ReceiveTimeout = receiveTimeout;
                m_Udp.Connect(ipAddr, port);
                InitKCP(conv);
                OnConnectSuccess();
            }
            catch (Exception e)
            {
                if (m_LogEnable) Debug.LogException(e);
                OnConnectFailed();
            }
        }

        void InitKCP(uint conv)
        {
            m_KCP = new KCP(conv, RawSend);
            m_KCP.NoDelay(0, 30, 2, 1);
            m_KCP.SetStreamMode(false);
        }

        protected override void InitNet()
        {
            base.InitNet();
            StartReceiveThread();
        }

        public override void Close()
        {
            base.Close();

            m_KCP = null;
            if (m_Udp != null)
            {
                m_Udp.Close();
                m_Udp = null;
            }

            StopReceiveThread();
        }
        #endregion

        #region send
        void RawSend(byte[] buffer, int length)
        {
            m_Udp.Send(buffer, length);
        }

        public override void SendPackage(INetPackage package)
        {
            if (m_KCP == null)
                return;

            m_WriteMemoryStream.Position = 0;
            package.Write(m_NetWriter);
            m_KCP.Send(m_WriteMemoryStream.GetBuffer(), 0, (int)m_WriteMemoryStream.Position);
        }
        #endregion

        #region receive
        volatile bool m_ReceiveThreadRunning = false;
        Thread m_ReceiveThread;
        object m_ReceiveQueueLock = new object();
        List<(byte[], int)> m_ReceiveQueue = new List<(byte[], int)>();
        List<(byte[], int)> m_ReceiveQueueThread = new List<(byte[], int)>();

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

                    var buffer = BufferPool.Get(readLength);
                    Array.Copy(m_ReceiveBuffer, buffer, readLength);
                    lock (m_ReceiveQueueLock)
                    {
                        m_ReceiveQueueThread.Add((buffer, readLength));
                    }
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

            lock (m_ReceiveQueueLock)
            {
                var temp = m_ReceiveQueue;
                m_ReceiveQueue = m_ReceiveQueueThread;
                m_ReceiveQueueThread = temp;
            }

            if (m_ReceiveQueue.Count > 0)
            {
                for (int i = 0; i < m_ReceiveQueue.Count; ++i)
                {
                    var item = m_ReceiveQueue[i];
                    m_KCP.Input(item.Item1, 0, item.Item2, true, true);
                }
                m_ReceiveQueue.Clear();
            }

            int size = m_KCP.PeekSize();
            while (size > 0)
            {
                var buffer = BufferPool.Get(size);

                m_KCP.Recv(buffer, 0, size);
               
                m_ReadMemoryStream.Write(buffer, 0, size);
                m_ReadMemoryStream.Position = 0;

                INetPackage package = NetPackagePool.Get();
                package.Read(m_NetReader, size);
                Recieve(package);

                m_ReadMemoryStream.Position = 0;
                size = m_KCP.PeekSize();
            }
        }
        #endregion
    }
}