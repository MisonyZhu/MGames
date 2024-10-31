using Framework;

namespace YooAsset
{
    /// <summary>
    /// 下载完毕  [6]
    /// </summary>
    public class FsmDownloadPackageOver :FsmBaseNode
    {
      
        public override void Enter()
        {
            Fsm.ChangeState<FsmClearPackageCache>();
        }

        public override void Update(float detlaTime)
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}