using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using App;

namespace YooAsset
{
    /// <summary>
    /// 更新资源版本号 [2]
    /// </summary>
    public class FsmUpdatePackageVersion : FsmBaseNode
    {
        private PatchOperation m_Owner;
      
        public override void Enter()
        {
            m_Owner = Fsm.GetOwner as PatchOperation;
            UIPatch.Instance.ShowPathTip("获取最新的资源版本 !");
            AppEntry.Instance.StartCoroutine(UpdatePackageVersion());
        }

        public override void Update(float detlaTime)
        {
           
        }

        public override void Exit()
        {
           
        }
        
        private IEnumerator UpdatePackageVersion()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            var packageName = m_Owner.PackageName;
            var package = YooAssets.GetPackage(packageName);
            var operation = package.UpdatePackageVersionAsync();
            yield return operation;

            if (operation.Status != EOperationStatus.Succeed)
            {
                Debug.LogWarning(operation.Error);
                //更新package失败 
                UIMessage.Instance.ShowTip($"Failed to update static version, please check the network status.",m_Owner.ReTryUpdatePackageVersion);
            }
            else
            {
                
                Fsm.BlackBoardData.SetData("PackageVersion", operation.PackageVersion);
                Fsm.ChangeState<FsmUpdatePackageManifest>();
            }
        }
    }
}