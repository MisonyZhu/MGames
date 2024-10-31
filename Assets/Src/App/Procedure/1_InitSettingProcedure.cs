using Framework;
using UnityEngine;

public class InitSettingProcedure : ProdureBase<AppEntry>
{

    public override bool IsComplete { get; protected set; }
    
    public override void Enter()
    {
        Debug.Log("【Procedure 1 InitSettingProcedure isEnter!】");
        
        ApplySettinglmpl();
    }

    void ApplySettinglmpl()
    {
        Application.targetFrameRate =AppEntry.Instance.SettingConfig.Frame;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        IsComplete = true;
    }
    
    

    public override void Tick(float detlaTime)
    {
        
    }

    public override void Exit()
    {
        
    }
}