
using System;
using UnityEngine;

namespace Framework
{
    public interface IResourceHandler : IDisposable
    {
        public UnityEngine.Object AssetObject { get; }

        public event System.Action<IResourceHandler> Completed;

        TAsset GetAssetObject<TAsset>() where TAsset : UnityEngine.Object;

        #region Sync

        GameObject InstantiateSync();

        GameObject InstantiateSync(Transform parent);

        GameObject InstantiateSync(Transform parent, bool worldPositionStays);

        GameObject InstantiateSync(Vector3 position, Quaternion rotation);

        GameObject InstantiateSync(Vector3 position, Quaternion rotation, Transform parent);

        #endregion

        #region Async
        InstantiateHandler InstantiateAsync();

        InstantiateHandler InstantiateAsync(Transform parent);

        InstantiateHandler InstantiateAsync(Transform parent, bool worldPositionStays);

        InstantiateHandler InstantiateAsync(Vector3 position, Quaternion rotation);

        InstantiateHandler InstantiateAsync(Vector3 position, Quaternion rotation, Transform parent);

        GameObject InstantiateSyncInternal(bool setPositionAndRotation, Vector3 position, Quaternion rotation, Transform parent, bool worldPositionStays);

        InstantiateHandler InstantiateAsyncInternal(bool setPositionAndRotation, Vector3 position, Quaternion rotation, Transform parent, bool worldPositionStays);

        #endregion

        void WaitForAsyncComplete();

        void Release();
    }
}
