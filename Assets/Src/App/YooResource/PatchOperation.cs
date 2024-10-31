
using Framework;
using UnityEngine;

namespace YooAsset
{
    
    public class PatchOperation : GameAsyncOperation
    {
       

        private enum ESteps
        {
            None,
            Update,
            Done,
        }
        
        IFsm m_Fsm;
        ESteps m_Step;
        
        public string PackageName { get; private set; }
        public string BuildPipeline { get; private set; }

        public PatchOperation(string package_name,string buildPipeline)
        {
            PackageName = package_name;
            BuildPipeline = buildPipeline;
            
            m_Fsm = Fsm.CreateFsm(this);
            m_Fsm.AddStateNode<FsmInitializePackage>();
            m_Fsm.AddStateNode<FsmUpdatePackageVersion>();
            m_Fsm.AddStateNode<FsmUpdatePackageManifest>();
            m_Fsm.AddStateNode<FsmCreatePackageDownloader>();
            m_Fsm.AddStateNode<FsmDownloadPackageFiles>();
            m_Fsm.AddStateNode<FsmDownloadPackageOver>();
            m_Fsm.AddStateNode<FsmClearPackageCache>();
            m_Fsm.AddStateNode<FsmUpdaterDone>();
        }

        protected override void OnStart()
        {  
            m_Step = ESteps.Update;
            m_Fsm.RunStateNode<FsmInitializePackage>();
        }

        protected override void OnUpdate()
        {
            if (m_Step==ESteps.None || m_Step == ESteps.Done)
            {
                return;
            }

            if (m_Step == ESteps.Update)
            {
                m_Fsm?.Tick(Time.deltaTime);
                if(m_Fsm.GetCurState<FsmUpdaterDone>().GetType() == typeof(FsmUpdaterDone))
                {
                    Status = EOperationStatus.Succeed;
                    m_Step = ESteps.Done;
                }
            }
        
        }

        protected override void OnAbort()
        {
            m_Fsm?.Dispose();
        }

        public void ReTryInitPackage()
        {
            m_Fsm.ChangeState<FsmInitializePackage>();
        }

        public void ReTryUpdatePackageVersion()
        {
            m_Fsm.ChangeState<FsmUpdatePackageVersion>();
        }
        
        public void ReTryUpdatePackageMainiFest()
        {
            m_Fsm.ChangeState<FsmUpdatePackageManifest>();
        }
    }
}
