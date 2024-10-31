using Framework;

namespace YooAsset
{
    /// <summary>
    /// 清理未使用的缓存文件  [7]
    /// </summary>
    public class FsmClearPackageCache : FsmBaseNode
    {

        public override void Enter()
        {
            var packageName = (Fsm.GetOwner as PatchOperation).PackageName;  
            var package = YooAssets.GetPackage(packageName);
            var operation = package.ClearUnusedCacheFilesAsync();
            operation.Completed += Operation_Completed;
        }

        public override void Update(float detlaTime)
        {
            
        }

        public override void Exit()
        {
            
        }
        
        private void Operation_Completed(YooAsset.AsyncOperationBase obj)
        {
           Fsm.ChangeState<FsmUpdaterDone>();
        }
    }
}