using Framework;
using System;
using System.Collections.Generic;


namespace Game
{
    public class DataServiceBase : IDataServiceCall
    {
        internal virtual int Priority
        {
            get
            {
                return 0;
            }
        }

        public DataServiceBase()
        {
            DataModule.RegisterModule(this);
        }


        public virtual void Update(float detlaTime) { }

        public virtual void Reset()
        { 
            
        }



        #region 各种状态回调数据
        public virtual void ShutDown()
        {
            
        }

        public virtual void OnLogin()
        {
            
        }

        public virtual void OnLoginOut()
        {
            
        }

        public virtual void OnReConnect()
        {
           
        }

        public virtual void OnDisConnect()
        {
           
        }

        #endregion
    }

    public abstract class DataServiceBase<T> : DataServiceBase where T : class, new()
    {
        private static T m_instance;

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new T();
                    if (m_instance != null)
                    {
                        (m_instance as DataServiceBase<T>).Init();
                    }
                }

                return m_instance;
            }
        }

        public new abstract int Priority { get; }

        public static void Release()
        {
            if (m_instance != null)
            {
                m_instance = null;
            }
        }

        public virtual void Init()
        {
        }

        public override void Update(float detlaTime)
        {
            OnUpdate(detlaTime);
        }

        public override void ShutDown()
        {
            OnShutDown();
        }

        public abstract void OnUpdate(float detlaTime);

        public abstract void OnShutDown();
    }
}
