using Framework;
using Game.Net;
using System;
using System.Collections.Generic;


namespace Game
{
    public class DataModule : ModuleBase<DataModule>,IDataServiceCall
    {
        private static GameFrameworkLinkedList<DataServiceBase> m_DataServices = new GameFrameworkLinkedList<DataServiceBase>();

        public override int Priority => ModulePriority.Data_Priority;

       

        public override void OnUpdate(float detlaTime)
        {
            var current = m_DataServices.First;
            while (current != null)
            {
                current.Value.Update( detlaTime);
                current = current.Next;
            }
        }

        public override void OnShutDown()
        {
            for (LinkedListNode<DataServiceBase> current = m_DataServices.Last; current != null; current = current.Previous)
            {
                current.Value.ShutDown();
            }
            m_DataServices.Clear();
        }

        public static void RegisterModule(DataServiceBase service)
        {
            if (service == null)
            {
                LogModule.LogError("模块注册不能为空");
                return;
            }

            LinkedListNode<DataServiceBase> linkedListNode = m_DataServices.First;
            while (linkedListNode != null && service.Priority <= linkedListNode.Value.Priority)
            {
                linkedListNode = linkedListNode.Next;
            }

            if (linkedListNode != null)
            {
                m_DataServices.AddBefore(linkedListNode, service);
            }
            else
            {
                m_DataServices.AddLast(service);
            }
        }

        public static void UnRegisterModule(DataServiceBase module)
        {
            if (module == null)
            {
                LogModule.LogError("模块不能为空");
            }
            else
            {
                m_DataServices.Remove(module);
            }
        }


        #region 各种状态回调接口
        public  void OnLogin()
        {
            for (LinkedListNode<DataServiceBase> current = m_DataServices.Last; current != null; current = current.Previous)
            {
                current.Value.OnLogin();
            }
        }

        public void OnLoginOut()
        {
            for (LinkedListNode<DataServiceBase> current = m_DataServices.Last; current != null; current = current.Previous)
            {
                current.Value.OnLoginOut();
            }
        }

        public void OnReConnect()
        {
            for (LinkedListNode<DataServiceBase> current = m_DataServices.Last; current != null; current = current.Previous)
            {
                current.Value.OnReConnect();
            }
        }

        public void OnDisConnect()
        {
            for (LinkedListNode<DataServiceBase> current = m_DataServices.Last; current != null; current = current.Previous)
            {
                current.Value.OnDisConnect();
            }
        }

        #endregion
    }
}
