using System.Collections;
using Framework;
using UnityEngine;
using YooAsset;

public class CheckResourceUpdate : ProdureBase<AppEntry>
{
    private const string DefaultPackage = "DefaultPackage";
    public override bool IsComplete { get; protected set; }
    public override void Enter()
    {
        Debug.Log("【Procedure 5 CheckResourceUpdate isEnter!】");
        AppEntry.Instance.StartCoroutine(CheckDefaultPackagePackage());
    }

    public override void Tick(float detlaTime)
    {
        
    }

    public override void Exit()
    {
        
    }
    
    private IEnumerator CheckDefaultPackagePackage()
    {
        var package = YooAssets.TryGetPackage(DefaultPackage);
        if (package == null)
            package = YooAssets.CreatePackage(DefaultPackage);
        PatchOperation operation = new PatchOperation(DefaultPackage,EDefaultBuildPipeline.BuiltinBuildPipeline.ToString());
        YooAssets.StartOperation(operation);
        yield return operation; 
        var gamePackage = YooAssets.GetPackage(DefaultPackage);
        YooAssets.SetDefaultPackage(gamePackage);

        //TODO 初始化Shader
        IsComplete = true;
    }
}