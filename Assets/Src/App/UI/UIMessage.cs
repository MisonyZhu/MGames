using System;
using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace App
{
    

public class UIMessage : MonoSingleton<UIMessage>
{
    private GameObject m_Root;

    private Text m_TipText;

    protected override void Awake()
    {
        base.Awake();
        InitUI();
    }

    void InitUI()
    {
        GameObject ui = Resources.Load<GameObject>("UIMessage");
        m_Root = GameObject.Instantiate(ui,AppEntry.Instance.transform.Find("Canvas"),false);
        m_TipText = m_Root.transform.Find("Tip_Text").GetComponent<Text>();
    }

    private void OnDestroy()
    {
        GameObject.DestroyImmediate(m_Root);
    }

    public void ShowTip(string tip,Action call)
    {
        m_TipText.text = tip;
    }
    
    public void ShowTip(string tip,Action ok,Action canel)
    {
        m_TipText.text = tip;
    }

    public void Destroy()
    {
        DestroyImmediate(m_Root);
        UIMessage.DestroyInstance();
    }
}

}