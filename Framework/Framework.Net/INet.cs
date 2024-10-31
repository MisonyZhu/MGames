using System;

namespace Framework
{
    public interface INetManager
    {
        void Update();
        void Connect(string ip, int port, Action<bool> connectCall = null, float timeOut = 5);
        
        void Connect(string ip, int port,uint uconv, Action<bool> connectCall = null, float timeOut = 5);

        void Send(int id, byte[] data);

        void SendPackage(INetPackage package);

        void Simulate(INetPackage package);

        void Close();
    }
}