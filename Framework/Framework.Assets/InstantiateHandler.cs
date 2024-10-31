using System;
using UnityEngine;


namespace Framework
{
    public class InstantiateHandler : IInstantiateHandler
    {
        IInstantiateHandler m_HandlerAdapter;
        public GameObject Result => m_HandlerAdapter.Result;

        public void Cancel()
        {
            m_HandlerAdapter.Cancel();
        }

        public void Dispose()
        {
            m_HandlerAdapter.Dispose();
        }

        public void WaitForAsyncComplete()
        {
            m_HandlerAdapter.WaitForAsyncComplete();
        }
    }
}
