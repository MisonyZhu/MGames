using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

namespace Game
{
    /// <summary>
    /// 资源组
    /// 【1】负责处理资源释放
    /// 【2】使用Group加载的一类资源都要使用Group
    /// 【3】非GameObject 类型建议都走Group加载 GameObject 可视情况而定自定义加载，切记加载方式伴随生命周期
    /// </summary>
    public sealed class ResourceGroup : IDisposable
    {
        List<AssetHandle> mHandles = new List<AssetHandle>();


        public AssetHandle LoadAsset(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                LogModule.LogError("Asset path is invalid.");
                return default;
            }

            var handle = YooAssets.LoadAssetSync(path);
            mHandles.Add(handle);

            return handle;
        }

        public T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(path))
            {
                LogModule.LogError("Asset path is invalid.");
                return default;
            }

            var handle = YooAssets.LoadAssetSync<T>(path);
            mHandles.Add(handle);

            if (typeof(T) == typeof(GameObject))
            {
                return handle.InstantiateSync() as T;
            }
            else
            {
                return handle.AssetObject as T;
            }
        }

        public AssetHandle LoadAssetAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                LogModule.LogError("Asset path is invalid.");
                return default;
            }

            var handle = YooAssets.LoadAssetAsync(path);
            mHandles.Add(handle);

            return handle;
        }

        public void LoadAssetAsync<T>(string path, Action<T> call) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(path))
            {
                LogModule.LogError("Asset path is invalid.");
                return;
            }

            var handle = YooAssets.LoadAssetAsync<T>(path);
            mHandles.Add(handle);

            if (typeof(T) == typeof(GameObject))
            {
                InstantiateOperation operation = handle.InstantiateAsync();
                operation.Completed += (instantiateOperation) => { call?.Invoke(operation.Result as T); };
            }
            else
            {
                handle.Completed += (handle) => { call?.Invoke(handle.AssetObject as T); };
            }
        }
        
        public AssetHandle LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(path))
            {
                LogModule.LogError("Asset path is invalid.");
                return null;
            }

            var handle = YooAssets.LoadAssetAsync<T>(path);
            mHandles.Add(handle);

            return handle;
        }

        public async UniTask AwaitLoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(path))
            {
                LogModule.LogError("Asset path is invalid.");
                await UniTask.Yield();
            }

            var handle = YooAssets.LoadAssetAsync<T>(path);
            mHandles.Add(handle);
            await handle;
        }

        public void Dispose()
        {
            for (int i = 0; i < mHandles.Count; ++i)
            {
                mHandles[i].Dispose();
            }

            mHandles.Clear();
        }
    }
}