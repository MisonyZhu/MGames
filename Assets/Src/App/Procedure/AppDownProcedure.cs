using System;
using Framework;
using App;
using UnityEngine;
using YooAsset;

public class AppDownProcedure : ProdureBase<AppEntry>
{
    private SceneHandle m_Hanlder;

    private const string LoginScenePath = "Assets/Res/Scene/Login.unity";
    public override bool IsComplete { get; protected set; }

    public override void Enter()
    {
        Debug.Log("【Procedure Over ! Ready Go GameEntry!】");

        LoadGameScene();
    }

    void LoadGameScene()
    {
        m_Hanlder = YooAssets.LoadSceneAsync(LoginScenePath);
    }

    public override void Tick(float detlaTime)
    {
        if (m_Hanlder!=null&&m_Hanlder.IsDone)
        {
            IsComplete = true;
        }
    }

    public override void Exit()
    {
        
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            if (assembly.GetName().Name == "Assembly-CSharp")
            {
                var mainType = assembly.GetType("Game.GameEntry");
                GameObject entry = new GameObject("GameEntry");
                entry.AddComponent(mainType);
                break;
            }
        }

        AppEntry.Instance.Destroy();
    }
}