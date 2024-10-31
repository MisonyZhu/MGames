using System.Collections.Generic;

namespace Framework
{
    public interface INetPackagePool
    {
        INetPackage Get();
        
        void Recycle(INetPackage package);
    }

    public class NetPackagePool <T> : INetPackagePool where T : INetPackage,new ()
    {
        public int maxCount = 100;
        Stack<INetPackage> m_Pool = new Stack<INetPackage>();
        
        public INetPackage Get()
        {
            if (m_Pool.Count > 0)
            {
                lock (m_Pool)
                {
                    if (m_Pool.Count > 0)
                    {
                        return m_Pool.Pop();
                    }
                }
            }
            return new T();
        }

        public void Recycle(INetPackage package)
        {
            package.Reset();
            if (m_Pool.Count < maxCount)
            {
                lock (m_Pool)
                {
                    m_Pool.Push(package);
                }
            }
        }
    }
}