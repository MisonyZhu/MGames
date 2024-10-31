using System.Collections;
using Framework;
using UnityEngine;
using App;

namespace YooAsset
{
    /// <summary>
    /// 创建文件下载器 [4]
    /// </summary>
    public class FsmCreatePackageDownloader : FsmBaseNode
    {
        private PatchOperation m_Operation;
        
        public override void Enter()
        {
            m_Operation = Fsm.GetOwner as PatchOperation;
            UIPatch.Instance.ShowPathTip("创建补丁下载器！");
            AppEntry.Instance.StartCoroutine(CreateDownloader());
        }

        public override void Update(float detlaTime)
        {
            
        }

        public override void Exit()
        {
            
        }
        
        IEnumerator CreateDownloader()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            var packageName = m_Operation.PackageName;
            var package = YooAssets.GetPackage(packageName);
            int downloadingMaxNum = 10;
            int failedTryAgain = 3;
            var downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
            Fsm.BlackBoardData.SetData("Downloader", downloader);

            if (downloader.TotalDownloadCount == 0)
            {
                Debug.Log("Not found any download files !");
                Fsm.ChangeState<FsmUpdaterDone>();
            }
            else
            {
                // 发现新更新文件后，挂起流程系统
                // 注意：开发者需要在下载前检测磁盘空间不足
                int totalDownloadCount = downloader.TotalDownloadCount;
                long totalDownloadBytes = downloader.TotalDownloadBytes;
                float sizeMB = totalDownloadBytes / 1048576f;
                sizeMB = Mathf.Clamp(sizeMB, 0.1f, float.MaxValue);
                string totalSizeMB = sizeMB.ToString("f1");
                UIMessage.Instance.ShowTip($"Found update patch files, Total count {totalDownloadCount} Total szie {totalSizeMB}MB",
                    () => { Fsm.ChangeState<FsmDownloadPackageFiles>(); }, () => { Application.Quit();});
            }
        }
    }
}