using Framework;
using System;
using UnityEngine;
using YooAsset;

namespace Game
{
    public class ResourceModule : ModuleBase<ResourceModule>
    {
        private static UnityResource m_Resourcelmpl;

        public override void Init()
        {
            base.Init();
            m_Resourcelmpl = UnityResource.Init((int)GameEntry.Instance.Config.ResourceMode);
        }

        public override int Priority => ModulePriority.Resource_Priority;

        public override void OnUpdate(float detlaTime)
        {
            m_Resourcelmpl.Tick(detlaTime);
        }

        public override void OnShutDown()
        {
            m_Resourcelmpl.Destroy();
        }


        public static GameObject Instantiate(string path,Transform parent = null,bool stayWorld = true)
        {
            var obj = m_Resourcelmpl.Instantiate(path, parent, stayWorld);
            return obj;
        }
        
        public static InstantiateOperation InstantiateAsync(string path,Transform parent = null,bool stayWorld = true)
        {
            var handler = m_Resourcelmpl.InstantiateAsync(path, parent, stayWorld);
            return handler;
        }

        public static T LoadAsset<T>(string path) where T : UnityEngine.Object 
        {
            return m_Resourcelmpl.LoadAssetSync<T>(path);
        }

        public static AssetHandle LoadAsset(string path) 
        {
            return m_Resourcelmpl.LoadAssetSync(path);
        }

        public static AssetHandle LoadAssetAsync(string path) 
        {
            return m_Resourcelmpl.LoadAssetAsync(path);
        }

        public static void LoadAssetAsync<T>(string path, Action<T> call) where T : UnityEngine.Object
        {
             m_Resourcelmpl.LoadAssetAsync(path);
        }

        //public AssetHandle LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        //{
        //    return m_Resourcelmpl.LoadAssetAsync(path);
        //}

        public static ResourceHandler LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            return new ResourceHandler(m_Resourcelmpl.LoadAssetAsync(path));
        }


        //TODO Unload ClearCache
    }
}