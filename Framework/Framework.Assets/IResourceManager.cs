using System.Collections.Generic;

namespace Framework
{
    //模块化和接口隔离的设计有助于维护和扩展你的资源管理模块。
    //基于unityEngine的资源管理模块，需求：1、可以接入不同的第三方资源管理，但是不影响现有的接口使用，只用更换底层即可
    //2、框架层不使用unityengine相关api   3、框架层需要封装同步和异步的接口，需要通用的异步回调接口
    public interface IResourceManager
    {
        void Tick(float detlaTime);
        //
        // void Destroy();
        // //维护资源handle 避免重复生成
        // IResourceHandler GetResourceHandler(string path);
        ResourceHandler LoadAssetAsync<T>(string path) where T : UnityEngine.Object;
    }
}