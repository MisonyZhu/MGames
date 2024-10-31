using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    // 自定义资源句柄
    public class ResourceHandler : IResourceHandler
    {
        private IResourceHandler m_HandlerAdapter;

       public ResourceHandler (IResourceHandler handlerAdapter)
        {
            m_HandlerAdapter = handlerAdapter;
        }

        public UnityEngine.Object AssetObject => m_HandlerAdapter.AssetObject;
       

        public event Action<IResourceHandler> Completed
        {
            add 
            {
                m_HandlerAdapter.Completed += value;
            }

            remove
            {
                m_HandlerAdapter.Completed -= value;
            }
        }

        public TAsset GetAssetObject<TAsset>() where TAsset : UnityEngine.Object
        {
            return m_HandlerAdapter.GetAssetObject<TAsset>();
        }

        public GameObject InstantiateSync()
        {
            return m_HandlerAdapter.InstantiateSync();
        }

        public GameObject InstantiateSync(Transform parent)
        {
            return m_HandlerAdapter.InstantiateSync(parent);
        }

        public GameObject InstantiateSync(Transform parent, bool worldPositionStays)
        {
            return m_HandlerAdapter.InstantiateSync(parent, worldPositionStays);
        }

        public GameObject InstantiateSync(Vector3 position, Quaternion rotation)
        {
            return m_HandlerAdapter.InstantiateSync(position, rotation);
        }

        public GameObject InstantiateSync(Vector3 position, Quaternion rotation, Transform parent)
        {
            return m_HandlerAdapter.InstantiateSync(position, rotation, parent);
        }

        public InstantiateHandler InstantiateAsync()
        {
           return  m_HandlerAdapter.InstantiateAsync();
        }

        public InstantiateHandler InstantiateAsync(Transform parent)
        {
            return m_HandlerAdapter.InstantiateAsync(parent);
        }

        public InstantiateHandler InstantiateAsync(Transform parent, bool worldPositionStays)
        {
            return m_HandlerAdapter.InstantiateAsync(parent, worldPositionStays);
        }

        public InstantiateHandler InstantiateAsync(Vector3 position, Quaternion rotation)
        {
            return m_HandlerAdapter.InstantiateAsync(position, rotation);
        }

        public InstantiateHandler InstantiateAsync(Vector3 position, Quaternion rotation, Transform parent)
        {
            return m_HandlerAdapter.InstantiateAsync(position, rotation, parent);
        }

        public GameObject InstantiateSyncInternal(bool setPositionAndRotation, Vector3 position, Quaternion rotation, Transform parent, bool worldPositionStays)
        {
            return m_HandlerAdapter.InstantiateSyncInternal(setPositionAndRotation, position, rotation, parent, worldPositionStays);
        }

        public InstantiateHandler InstantiateAsyncInternal(bool setPositionAndRotation, Vector3 position, Quaternion rotation, Transform parent, bool worldPositionStays)
        {
            return m_HandlerAdapter.InstantiateAsyncInternal(setPositionAndRotation, position, rotation, parent, worldPositionStays);
        }

        public void Release()
        {
            m_HandlerAdapter.Release();
        }

        public void WaitForAsyncComplete()
        {
            m_HandlerAdapter.WaitForAsyncComplete();
        }

        public void Dispose()
        {
            m_HandlerAdapter.Dispose();
        }
    }


}






