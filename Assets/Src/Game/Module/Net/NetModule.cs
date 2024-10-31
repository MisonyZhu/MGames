using Framework;
using Game.Net;
using System.Collections.Generic;

namespace Game
{
    public class NetModule : ModuleBase<NetModule>
    {
        private GameFrameworkLinkedList<NetServiceBase> netServices = new GameFrameworkLinkedList<NetServiceBase>();
        private UnityNet m_Net;
        
            
        public override int Priority  => ModulePriority.Net_Priority; 
        
        public override void Init()
        {
            base.Init();
            
            m_Net = UnityNet.Create(UnityNet.ENetModel.Tcp);
        }
        
        public override void OnUpdate(float detlaTime)
        {
            m_Net?.Update();

            var current = netServices.First;
            while (current != null)
            {
                current.Value.Update(detlaTime, detlaTime);
                current = current.Next;
            }
        }

        public override void OnShutDown()
        {
            m_Net?.Close();

            for (LinkedListNode<NetServiceBase> current = netServices.Last; current != null; current = current.Previous)
            {
                current.Value.Shutdown();
            }
            netServices.Clear();
        }

        public void RegisterService(NetServiceBase module)
        {
            if (module == null)
            {
                LogModule.LogError($"空引用无法注册模块");
                return;
            }

            LinkedListNode<NetServiceBase> current = netServices.First;
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
                netServices.AddBefore(current, module);
            }
            else
            {
                netServices.AddLast(module);
            }
        }

        public void UnRegisterService(NetServiceBase module)
        {
            if (module == null)
            {
                LogModule.LogError($"空引用无法反注册");
                return;
            }

            netServices.Remove(module);
        }
    }
}