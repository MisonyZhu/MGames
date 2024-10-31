using System;
using YooAsset;

namespace Game
{
    public class AssetLoader : IDisposable
    {
        private Action<UnityEngine.Object> m_Complete;
        private AssetHandle m_Handle;
        public AssetLoader(Action<UnityEngine.Object> onComplete)
        {
            m_Complete = onComplete;
        }

        public AssetHandle Load(string path, System.Type type)
        {
            m_Handle = YooAssets.LoadAssetSync(path, type);
            return m_Handle;
        }
        
        public AssetHandle LoadAsync(string path, System.Type type)
        {
            m_Handle = YooAssets.LoadAssetAsync(path, type);
            return m_Handle;
        }

        public void Dispose()
        {
            m_Handle?.Dispose();
        }
    }
}