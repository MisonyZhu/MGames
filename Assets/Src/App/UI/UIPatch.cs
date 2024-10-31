using System;
using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace App
{
    public class UIPatch : MonoSingleton<UIPatch>
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
            GameObject ui = Resources.Load<GameObject>("UIPatch");
            m_Root = GameObject.Instantiate(ui,AppEntry.Instance.transform.Find("Canvas"),false);
            m_TipText = m_Root.transform.Find("Tip_Text").GetComponent<Text>();
        }

        private void OnDestroy()
        {
            GameObject.DestroyImmediate(m_Root);
        }

        public void ShowPathTip(string msg)
        {
            m_TipText.text = msg;
        }

        public void ShowPatchTipWithProcess(string msg, float process)
        {
            m_TipText.text = msg;
        }

        public void Destroy()
        {
            DestroyImmediate(m_Root);
            DestroyInstance();
        }
    }
}