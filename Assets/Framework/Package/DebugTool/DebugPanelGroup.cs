using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Framework.Debug
{
    public class DebugPanelGroup : DebugPanel
    {
        List<DebugPanel> m_Panels = new List<DebugPanel>();
        protected int m_SelectIndex = 0;

        public List<DebugPanel> panels
        {
            get { return m_Panels; }
        }

        public DebugPanel selectPanel
        {
            get { return m_SelectIndex < panels.Count ? panels[m_SelectIndex] : null; }
        }

        DebugPanel GetPanelByName(string name)
        {
            foreach (var panel in panels)
            {
                if (panel.panelName == name)
                    return panel;
            }
            return null;
        }

        public DebugPanel GetPanel(string path)
        {
            if (string.IsNullOrEmpty(path))
                return this;

            DebugPanelGroup p = this;
            var names = path.Split('/');
            for (int i = 0; i < names.Length - 1; ++i)
            {
                p = p.GetPanelByName(names[i]) as DebugPanelGroup;
                if (p == null)
                    return null;
            }

            return p.GetPanelByName(names[names.Length - 1]);
        }

        public T GetPanel<T>(string path) where T : DebugPanel
        {
            return GetPanel(path) as T;
        }

        public void AddPanel(string path, DebugPanel panel)
        {
            var p = this;
            var index = path.LastIndexOf('/');
            if (index >= 0)
            {
                panel.panelName = path.Substring(index + 1);
                p = GetPanel(path.Substring(0, index)) as DebugPanelGroup;
                if (p == null)
                {
                    UnityEngine.Debug.Log($"add panel {path} failed!");
                    return;
                }
            }
            else
                panel.panelName = path;

            p.m_Panels.Add(panel);
            p.m_ToolbarNames = null;
        }

        public override void OnEnable()
        {
            for (int i = 0; i < m_Panels.Count; ++i)
            {
                m_Panels[i].OnEnable();
            }
        }

        public override void OnDisable()
        {
            for (int i = 0; i < m_Panels.Count; ++i)
            {
                m_Panels[i].OnDisable();
            }
        }

        public override void OnEnter()
        {
            var panel = selectPanel;
            if (panel != null)
                panel.OnEnter();
        }

        public override void OnLeave()
        {
            var panel = selectPanel;
            if (panel != null)
                panel.OnLeave();
        }

        public override void Update()
        {
            var panel = selectPanel;
            if (panel != null)
                panel.Update();
        }

        protected string[] m_ToolbarNames;
        private string[] toolbarNames
        {
            get
            {
                if (m_ToolbarNames == null)
                    CalcToolbarNames();
                return m_ToolbarNames;

            }
        }

        protected virtual void CalcToolbarNames()
        {
            m_ToolbarNames = new string[panels.Count];
            for (int i = 0; i < panels.Count; ++i)
            {
                m_ToolbarNames[i] = string.Format("<b>{0}</b>", panels[i].panelName);
            }
        }

        protected virtual void OnToolbarIndex(int index)
        {
            selectPanel.OnLeave();
            m_SelectIndex = index;
            selectPanel.OnEnter();
        }

        public override void OnGUI()
        {
            int index = GUILayout.Toolbar(m_SelectIndex, toolbarNames, GUILayout.Height(30));
            if (m_SelectIndex != index)
                OnToolbarIndex(index);

            var panel = selectPanel;
            if (panel != null)
                panel.OnGUI();
        }
    }
}