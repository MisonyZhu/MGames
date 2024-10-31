using System.Collections;
using Framework;
using UnityEngine;
using App;

namespace YooAsset
{
    /// <summary>
    /// 更新资源清单 [3]
    /// </summary>
    public class FsmUpdatePackageManifest : FsmBaseNode
    {
        private PatchOperation m_Operation;
        
        public override void Enter()
        {
            m_Operation = Fsm.GetOwner as PatchOperation;
            UIPatch.Instance.ShowPathTip("更新资源清单！");
            AppEntry.Instance.StartCoroutine(UpdateManifest());
        }

        public override void Update(float detlaTime)
        {
          
        }

        public override void Exit()
        {
           
        }
        
        private IEnumerator UpdateManifest()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            var packageName = m_Operation.PackageName;
            var packageVersion = (string)Fsm.BlackBoardData.GetData("PackageVersion");
            var package = YooAssets.GetPackage(packageName);
            bool savePackageVersion = true;
            var operation = package.UpdatePackageManifestAsync(packageVersion, savePackageVersion);
            yield return operation;

            if (operation.Status != EOperationStatus.Succeed)
            {
                Debug.LogWarning(operation.Error);
                //更新清单文件失败 TODO
                UIMessage.Instance.ShowTip($"Failed to update patch manifest, please check the network status.", m_Operation.ReTryUpdatePackageMainiFest);
                yield break;
            }
            else
            {
                Fsm.ChangeState<FsmCreatePackageDownloader>();
            }
        }
    }
}