using System;

namespace Framework
{
    public interface INetReceiver
    {
        void OnReceive(INetPackage package);
    }

    public class NetReceiver: INetReceiver
    {
        Action<INetPackage> m_OnReceive;

        public NetReceiver(Action<INetPackage> onReceive)
        {
            m_OnReceive = onReceive;
        }

        public void OnReceive(INetPackage package)
        {
            m_OnReceive?.Invoke(package);
        }
    }
}