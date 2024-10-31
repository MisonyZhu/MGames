using YooAsset;

namespace Game
{
    public class YooAdapter :IResourceAdapter
    {
        /// <summary>
        /// 资源系统运行模式
        /// </summary>
        public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;

        public YooAdapter(int model)
        {
            PlayMode = (EPlayMode)model;
        }


        public void Init()
        {
            YooAssets.Initialize();
            
            // PatchOperation operation = new PatchOperation("DefaultPackage", EDefaultBuildPipeline.BuiltinBuildPipeline.ToString(), PlayMode);
            // YooAssets.StartOperation(operation);
            
            var gamePackage = YooAssets.GetPackage("DefaultPackage");
            YooAssets.SetDefaultPackage(gamePackage);
        }

        public void Tick(float detlaTime)
        {
           
        }

      
    }
}