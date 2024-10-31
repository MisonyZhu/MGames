using System;
using System.Collections;
using UnityEngine;
using Framework;
using Sirenix.OdinInspector;
using App;

/// <summary>
/// ��������
/// 
/// 1�����½���ֻ����һ��Ĭ�ϵ�ͼ������ʱ���ȡ����ͼ������汾��Ϣ����Դ�б��ʱ�򣬶ԱȲ�������в��������µ�ͼ�滻���ɣ��������ͼҲ�����ǽ����bundle��Դ
/// 2���Ѹ�����Դ���̶���������������Ϸǰ���κ�ʱ������������̣�������������ĻҶȸ���
/// 
/// </summary>
public class AppEntry : MonoSingleton<AppEntry>
{
    [SerializeField] private AppConfig m_Config;
    [SerializeField] private AppSetingConfig m_SettingConfig;
    
    Produrer<AppEntry> m_AppProdure;

    public AppConfig Config => m_Config;
    public AppSetingConfig SettingConfig => m_SettingConfig;

    void Awake()
    {
        DontDestroyOnLoad(this);
        Init();
    }

    void Init()
    {
        m_AppProdure = new(this);
        m_AppProdure.Add<InitSettingProcedure>();
        m_AppProdure.Add<InitChannelProcedure>();
        m_AppProdure.Add<CheckAppUpdate>();
        m_AppProdure.Add<CheckDllUpdate>();
        m_AppProdure.Add<CheckResourceUpdate>();
        m_AppProdure.Add<AppDownProcedure>();
        m_AppProdure.RunProdurce();
    }


    void Update()
    {
        m_AppProdure?.Update(Time.deltaTime);
    }

    public void Destroy()
    {
        m_AppProdure = null;
        UIMessage.Instance.Destroy();
        UIPatch.Instance.Destroy();
        DestroyInstance();
    }
    
}


[Serializable, LabelText("配置")]
public class AppConfig
{
    public EResourceMode ResourceMode;

    public string AppVersion;
    

    public string ResourceUrl
    {
        get { return ""; }
    }
    
}

[Serializable,LabelText("设置")]
public class AppSetingConfig
{
    [ValueDropdown("FrameKeys")]
    public int Frame = 60;

    private IEnumerable FrameKeys = new ValueDropdownList<int>()
    {
        { "30帧", 30 },
        { "60帧", 60 },
        { "120帧", 120 },
    };
}