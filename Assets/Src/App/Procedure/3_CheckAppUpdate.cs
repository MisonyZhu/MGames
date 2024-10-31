using App;
using Framework;
using UnityEngine;

/// <summary>
/// ���������¡�
/// </summary>
public class CheckAppUpdate : ProdureBase<AppEntry>
{
    
    public override bool IsComplete { get; protected set; }
    
    public override void Enter()
    {
        Debug.Log("【Procedure 3 CheckAppUpdate isEnter!】");

        AppUpdateLmpl();
    }

    void AppUpdateLmpl()
    {
        if (AppEntry.Instance.Config.ResourceMode == EResourceMode.HostPlayMode)
        {
            //TODO
        }

        IsComplete = true;
    }

    public override void Tick(float detlaTime)
    {
      
    }

    public override void Exit()
    {
       
    }
}