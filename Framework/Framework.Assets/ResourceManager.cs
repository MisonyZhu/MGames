using UnityEngine;

namespace Framework
{
    public class ResourceManager
    {
        //基于上述，同时保证上述的要求，想要进一步实现一个资源句柄，来处理同步和异步的情况，
        internal static IResourceManager ResourceMgr;

        public static void SetResourceManager(IResourceManager manager)
        {
            ResourceMgr = manager;
        }

        public static void Update()
        {
            ResourceMgr?.Tick(Time.deltaTime);
        }

        public static ResourceHandler LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            return new ResourceHandler(ResourceMgr.LoadAssetAsync<T>(path));
        }
    }
}