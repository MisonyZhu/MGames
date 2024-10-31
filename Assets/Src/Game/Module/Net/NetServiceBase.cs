using Framework;

namespace Game.Net
{
    public abstract class NetServiceBase : IReference
    {
        internal virtual int Priority
        {
            get
            {
                return 0;
            }
        }
        
        public virtual void RegisterEvents()
        {
        }

        public virtual void UnRegisterEvents()
        {
        }

        public virtual void Update(float elapseSeconds, float realElapseSeconds){}

        public void Clear()
        {
        }
        
        public virtual void Shutdown() {}
    }
    
    public abstract class NetServiceBase<T> : NetServiceBase where T : new()
    {
        private static T m_instance;

        public static T Instance
        {
            get
            {
                if(null == m_instance)
                    m_instance = new T();
                
                return m_instance;
            }
        } 
    }
}