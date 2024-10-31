using System.Collections.Generic;

namespace Framework
{
    public abstract class Moduler 
    {
        private static readonly ModuleLinkedList<ModuleBase> m_AllModules = new ModuleLinkedList<ModuleBase>();
        
        public static void RegisterModule(ModuleBase module)
        {
            if (module == null)
            {
                Log.LogError($"模块注册不能为空");
                return;
            }

            LinkedListNode<ModuleBase> current = m_AllModules.First;
            while (current != null)
            {
                if (module.Priority > current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }

            if (current != null)
            {
                m_AllModules.AddBefore(current, module);
            }
            else
            {
                m_AllModules.AddLast(module);
            }
        }

        public static void UnRegisterModule(ModuleBase module)
        {
            if (module == null)
            {
                Log.LogError($"模块不能为空");
                return;
            }

            m_AllModules.Remove(module);
        }

        protected static void OnUpdate(float detlaTime)
        {
            var current = m_AllModules.First;
            while (current != null)
            {
                current.Value.Update(detlaTime);
                current = current.Next;
            }
        }

        protected static void OnShutdown()
        {
            for (LinkedListNode<ModuleBase> current = m_AllModules.Last; current != null; current = current.Previous)
            {
                current.Value.ShutDown();
            }
            m_AllModules.Clear();
        }


    }
}