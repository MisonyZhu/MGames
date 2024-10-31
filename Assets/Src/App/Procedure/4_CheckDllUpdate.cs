using System.Collections;
using System.Reflection;
using Framework;
using HybridCLR;
using YooAsset;
using UnityEngine;
/// <summary>
///
/// </summary>
public class CheckDllUpdate : ProdureBase<AppEntry>
{
    private const string LauncherPackage = "LauncherPackage";
    public override void Enter()
    {
        Debug.Log("【Procedure 4 CheckDllUpdate isEnter!】");
        YooAssets.Initialize();
        AppEntry.Instance.StartCoroutine(CheckLauncherPackage());
    }


    public override void Tick(float detlaTime)
    {
        
    }

    public override void Exit()
    {
        
    }

    private IEnumerator CheckLauncherPackage()
    {
        var package = YooAssets.TryGetPackage(LauncherPackage);
        if (package == null)
            package = YooAssets.CreatePackage(LauncherPackage);
        PatchOperation operation = new PatchOperation(LauncherPackage,EDefaultBuildPipeline.BuiltinBuildPipeline.ToString());
        YooAssets.StartOperation(operation);
        yield return operation; 
        var gamePackage = YooAssets.GetPackage(LauncherPackage);
#if !UNITY_EDITOR
        //加载AotMeta
        LoadAotMetaDll();
        //加载更新代码
        LoadHotFixDll();
#endif
        IsComplete = true;
    }

    void LoadAotMetaDll()
    {
        var dlls = YooAssets.GetAssetInfos("aotDll");
        foreach (var file in dlls)
        {
            if (file.AssetPath.EndsWith(".Dll.txt"))
            {
                var hanlder = YooAssets.LoadAssetSync(file);
                RuntimeApi.LoadMetadataForAOTAssembly((hanlder.AssetObject as TextAsset).bytes, HomologousImageMode.SuperSet);
                hanlder.Dispose();
            }
        }
    }

    void LoadHotFixDll()
    {
        var dlls = YooAssets.GetAssetInfos("hotfixDll");
        foreach (var file in dlls)
        {
            if (file.AssetPath.EndsWith(".Dll.txt"))
            {
                var hanlder = YooAssets.LoadAssetSync(file);
                Assembly.Load((hanlder.AssetObject as TextAsset).bytes);
                hanlder.Dispose();
            }
        }
    }



    public override bool IsComplete { get; protected set; }
}