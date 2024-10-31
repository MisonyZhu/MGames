using System;
using Framework;
using UnityEngine;
using YooAsset;

namespace Game
{
    public class UnityResource: IResourceManager
    {
        private ResourceGroup m_ResourceGroup;
        
        private static IResourceAdapter m_Adapter;

        public static UnityResource Init(int model)
        {
            var manager = new UnityResource(model);
            ResourceManager.SetResourceManager(manager);
            return manager;
        }

        UnityResource(int mode)
        {
            m_ResourceGroup = new ResourceGroup();
            m_Adapter = new YooAdapter(mode);
            m_Adapter.Init();
        }

        public void Tick(float detlaTime)
        {
            m_Adapter?.Tick(detlaTime);
        }

        public void Destroy()
        {
            m_ResourceGroup.Dispose();
        }

        public GameObject Instantiate(string path, Transform parent = null, bool stayWorld = true)
        {
            var obj = YooAssets.LoadAssetSync(path).InstantiateSync(parent, stayWorld);
            return obj;
        }

        public AssetHandle LoadAssetSync(string path)
        {
            return m_ResourceGroup.LoadAsset(path);
        }

        public T LoadAssetSync<T>(string path) where T : UnityEngine.Object
        {
            return m_ResourceGroup.LoadAsset<T>(path);
        }


        public InstantiateOperation InstantiateAsync(string path, Transform parent = null, bool stayWorld = true)
        {
            var handler = YooAssets.LoadAssetAsync(path).InstantiateAsync(parent, stayWorld);
            return handler;
        }

       
        public AssetHandle LoadAssetAsync(string path)
        {
            return m_ResourceGroup.LoadAssetAsync(path);
        }

        public void LoadAssetAsync<T>(string path,Action<T> call) where T: UnityEngine.Object
        {
            var handle =  m_ResourceGroup.LoadAssetAsync(path);
            handle.Completed += (h) =>
            {
                call?.Invoke(h.AssetObject as T);
            };
        }

        public ResourceHandler LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            return new ResourceHandler(m_ResourceGroup.LoadAssetAsync(path));
        }
    }
}