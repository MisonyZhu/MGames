using System;

namespace Framework
{
    public abstract class ModuleBase
    {
        public ModuleBase()
        {
            Moduler.RegisterModule(this);
        }
        
        public virtual int Priority { get; }
        
        public virtual void Update(float detlaTime)
        {
            
        }

        public virtual void ShutDown()
        {
            
        }
    }

    public abstract class ModuleBase<T> : ModuleBase  where T :  class, new()
    {
        private static T m_instance;

        public static T Instance
        {
            get
            {
                if (ModuleBase<T>.m_instance == null)
                {
                    ModuleBase<T>.m_instance = Activator.CreateInstance<T>();
                    if (ModuleBase<T>.m_instance != null)
                    {
                        (ModuleBase<T>.m_instance as ModuleBase<T>).Init();
                    }
                }

                return ModuleBase<T>.m_instance;
            }
        }

        public static void Release()
        {
            if (ModuleBase<T>.m_instance != null)
            {
                ModuleBase<T>.m_instance = (T)((object)null);
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

        public abstract int Priority { get; }
        
        public abstract void OnUpdate(float detlaTime);

        public abstract void OnShutDown();
    }
}