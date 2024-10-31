using System;
using Framework;

namespace Game
{
    public class UnityNet : INetManager
    {
        public enum ENetModel
        {
            Tcp,
            Udp,
            Kcp,
            WebSocket,
        }


        public NetConnection Connection { get; private set; }

        public static UnityNet Create(ENetModel model)
        {
            UnityNet net = new UnityNet(model);
            return net;
        }

        UnityNet(ENetModel model)
        {
            switch (model)
            {
                case ENetModel.Tcp:
                    Connection = new TcpConnection();
                    break;
                case ENetModel.Udp:
                    Connection = new UdpConnection();
                    break;
                case ENetModel.Kcp:
                    Connection = new KcpConnection();
                    break;
                case ENetModel.WebSocket:
#if UNITY_WEBGL
                    Connection = new WebConnection();
#endif
                    break;
                default:
                    Connection = new TcpConnection();
                    break;
            }
            Connection.NetReceiver = new NetReceiver(UnityNetMessage.OnReceive);
            Connection.onDisconnect += OnDisconnect;
        }

        void OnDisconnect()
        {
            
        }

        // public static void Send<T>(int id, T message) where T : Google.Protobuf.IMessage
        // {
        //     connection.Send(id, Protobuf.Serialize(message));
        // }

        public  void Send(int id)
        {
            Connection.Send(id, null);
        }
        
        /// <summary>
        /// 本地模拟接收网络包
        /// </summary>
        public void Simulate(INetPackage package)
        {
            if (package != null)
            {
                Connection.NetReceiver?.OnReceive(package);
            }
        }
        
        #region FrameInterface
     
        public void Update()
        {
            Connection?.Update();
        }

        public void Connect(string ip, int port, Action<bool> connectCall = null, float timeOut = 5)
        {
            Connection?.Connect(ip,port,connectCall,timeOut);
        }

        public void Connect(string ip, int port, uint uconv, Action<bool> connectCall = null, float timeOut = 5)
        {
            Connection?.Connect(ip,port,uconv,connectCall,timeOut);
        }

        public void Send(int id, byte[] data)
        {
            Connection?.Send(id,data);
        }

        public void SendPackage(INetPackage package)
        {
           Connection.SendPackage(package);
        }

        public void Close()
        {
            Connection.Close();
        }
        
        #endregion
    }
}