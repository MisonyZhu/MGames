using System.Collections;
using Framework;
using App;

namespace YooAsset
{
    /// <summary>
    /// 下载更新文件 [5]
    /// </summary>
    public class FsmDownloadPackageFiles : FsmBaseNode
    {
    
        public override void Enter()
        { 
            UIPatch.Instance.ShowPathTip("开始下载补丁文件！");
            AppEntry.Instance.StartCoroutine(BeginDownload());
        }

        public override void Update(float detlaTime)
        {
           
        }

        public override void Exit()
        {
            
        }
        
        private IEnumerator BeginDownload()
        {
            var downloader = (ResourceDownloaderOperation)Fsm.BlackBoardData.GetData("Downloader");
            downloader.OnDownloadErrorCallback = DownloadErrorCallBack;
            downloader.OnDownloadProgressCallback = DownloadProgressCallback;
            downloader.BeginDownload();
            yield return downloader;

            // 检测下载结果
            if (downloader.Status != EOperationStatus.Succeed)
                yield break;

            Fsm.ChangeState<FsmDownloadPackageOver>();
        }

       private  void DownloadErrorCallBack(string fileName, string error)
        {
            UIMessage.Instance.ShowTip($"Failed to download file : {fileName}", () => { Fsm.ChangeState<FsmCreatePackageDownloader>();});
        }
       
       private  void DownloadProgressCallback(int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes, long currentDownloadBytes)
       {
           float value = (float)currentDownloadCount / totalDownloadCount;
           string currentSizeMB = (currentDownloadBytes / 1048576f).ToString("f1");
           string totalSizeMB = (totalDownloadBytes / 1048576f).ToString("f1");
           string tip = $"{currentDownloadCount}/{totalDownloadCount} {currentSizeMB}MB/{totalSizeMB}MB";
           UIPatch.Instance.ShowPatchTipWithProcess(tip,value);
       }
    }
}