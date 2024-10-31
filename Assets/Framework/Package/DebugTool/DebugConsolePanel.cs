using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Framework.Debug
{
    public class DebugConsolePanel : DebugPanel
    {
        const int ONE_PAGE_COUNT = 100;
        int m_Page = 0;
        bool m_Collapse = true;
        bool m_InfoFilter = false;
        bool m_WarningFilter = false;
        bool m_ErrorFilter = true;
        DraggableScrollView m_LogScrollView = new DraggableScrollView();
        DraggableScrollView m_StackScrollView = new DraggableScrollView(GUILayout.Height(120));
        DraggableScrollView m_LogSwitchScrollView = new DraggableScrollView();
        string m_Input = string.Empty;

        public int infoCount { get; private set; }
        public int warningCount { get; private set; }
        public int errorCount { get; private set; }
        int count
        {
            get
            {
                int value = 0;
                if (m_InfoFilter) value += infoCount;
                if (m_WarningFilter) value += warningCount;
                if (m_ErrorFilter) value += errorCount;
                return value;
            }
        }

        class Node
        {
            public LogType type;
            public string message;
            public string stackTrace;

            public Node(LogType type, string message, string stackTrace)
            {
                this.type = type;
                this.message = message;
                this.stackTrace = stackTrace;
            }

            public override bool Equals(object obj)
            {
                var node = obj as Node;
                if (node == null)
                    return false;

                return type == node.type && message == node.message && stackTrace == node.stackTrace;
            }

            public override int GetHashCode()
            {
                return type.GetHashCode() + message.GetHashCode() + stackTrace.GetHashCode();
            }
        }

        List<Node> m_Nodes = new List<Node>();
        Node m_SelectNode = null;

        public override void OnEnable()
        {
            Application.logMessageReceived += OnLog;
        }

        public override void OnDisable()
        {
            Application.logMessageReceived -= OnLog;
        }

        static readonly string STACK_TRACEBACK = "\nstack traceback:";
        void OnLog(string condition, string stackTrace, LogType type)
        {
            int index = condition.IndexOf(STACK_TRACEBACK);
            if (index >= 0)
            {
                stackTrace = condition.Substring(index + 1);
                condition = condition.Substring(0, index);
            }

            var node = new Node(type, condition, stackTrace);
            m_Nodes.Add(node);
            switch (type)
            {
                case LogType.Log:
                    ++infoCount;
                    break;
                case LogType.Warning:
                    ++warningCount;
                    break;
                default:
                    ++errorCount;
                    break;
            }
        }

        void Clear()
        {
            m_Nodes.Clear();
            m_SelectNode = null;
            infoCount = 0;
            warningCount = 0;
            errorCount = 0;
        }

        void Save()
        {
            StringBuilder strb = new StringBuilder();
            foreach (var node in m_Nodes)
            {
                if (!CheckLogType(node))
                    continue;

                strb.AppendLine(node.message);
            }

            string name = "Console_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string path = debugDir + name + ".txt";
            File.WriteAllText(path, strb.ToString());
            Application.OpenURL(path);
        }

        const string INPUT_NAME = "DebugConsolePanel.Input";
        public override void OnGUI()
        {
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Clear All", GUILayout.Height(30)))
                {
                    Clear();
                }

                if (GUILayout.Button("Save", GUILayout.Height(30)))
                {
                    Save();
                }

                m_Collapse = GUILayout.Toggle(m_Collapse, "Collapse", "button", GUILayout.Height(30));
                m_InfoFilter = GUILayout.Toggle(m_InfoFilter, string.Format("Info Filter({0})", infoCount), "button", GUILayout.Height(30));
                m_WarningFilter = GUILayout.Toggle(m_WarningFilter, string.Format("Warning Filter({0})", warningCount), "button", GUILayout.Height(30));
                m_ErrorFilter = GUILayout.Toggle(m_ErrorFilter, string.Format("Error Filter({0})", errorCount), "button", GUILayout.Height(30));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical("box");
            {
                using (m_LogScrollView.Scope())
                {
                    var style = GUI.skin.label;

                    int start = m_Page * ONE_PAGE_COUNT;
                    int end = Mathf.Min(start + ONE_PAGE_COUNT, count);
                    int index = 0;
                    for (int i = 0; i < m_Nodes.Count; ++i)
                    {
                        var node = m_Nodes[i];
                        if (!CheckLogType(node))
                            continue;

                        ++index;
                        if (index <= start || index > end)
                            continue;

                        if (m_Collapse && Contains(m_Nodes, node, i))
                            continue;

                        int count = m_Collapse ? Count(m_Nodes, node) : 1;
                        if (m_SelectNode == node)
                        {
                            GUI.backgroundColor = Color.gray;
                            style.normal.background = Texture2D.whiteTexture;
                        }

                        var text = GetLogString(node);
                        var rect = GUILayoutUtility.GetRect(new GUIContent(text), style);
                        if (GUI.Button(rect, text, style))
                        {
                            m_SelectNode = node;
                        }

                        if (m_SelectNode == node)
                        {
                            GUI.backgroundColor = Color.white;
                            style.normal.background = null;
                        }

                        if (count > 1)
                        {
                            var str = count.ToString();
                            var size = GUI.skin.box.CalcSize(new GUIContent(str));
                            GUI.Label(new Rect(rect.xMax - size.x, rect.y, size.x, rect.height), str, GUI.skin.box);
                        }
                    }
                }

                GUILayout.BeginHorizontal();
                m_Page = DrawPage(m_Page, count, ONE_PAGE_COUNT);
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            using (new GUILayout.VerticalScope("box", GUILayout.Width(100), GUILayout.ExpandHeight(true)))
            {
                GUI.skin.label.alignment = TextAnchor.UpperCenter;
                GUILayout.Label("<b>Log Switch</b>");
                GUI.skin.label.alignment = TextAnchor.UpperLeft;

                using (m_LogSwitchScrollView.Scope())
                {
                    foreach (var logSwitch in LogSwitch.instances)
                    {
                        logSwitch.isOn = GUILayout.Toggle(logSwitch.isOn, logSwitch.name, "button", GUILayout.Height(30));
                    }
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("box");
            {
                using (m_StackScrollView.Scope())
                {
                    if (m_SelectNode != null)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(GetLogString(m_SelectNode));
                        if (GUILayout.Button("COPY", GUILayout.Width(60f), GUILayout.Height(30f)))
                        {
                            GUIUtility.systemCopyBuffer = string.Format("{0}\n\n{1}", m_SelectNode.message, m_SelectNode.stackTrace);
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.Label(m_SelectNode.stackTrace);
                    }
                }
            }
            GUILayout.EndVertical();

            GUI.SetNextControlName(INPUT_NAME);
            GUILayout.BeginHorizontal();
            {
                m_Input = GUILayout.TextArea(m_Input);
                if (GUILayout.Button("Enter", GUILayout.Width(100)))
                    Enter();
            }
            GUILayout.EndHorizontal();

            if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return)
            {
                if (GUI.GetNameOfFocusedControl() == INPUT_NAME)
                {
                    Enter();
                    GUI.FocusControl(string.Empty);
                }
                else
                    GUI.FocusControl(INPUT_NAME);
            }
        }

        bool CheckLogType(Node node)
        {
            switch (node.type)
            {
                case LogType.Log:
                    return m_InfoFilter;
                case LogType.Warning:
                    return m_WarningFilter;
            }
            return m_ErrorFilter;
        }

        void Enter()
        {
            if (!string.IsNullOrEmpty(m_Input))
            {
                UnityEngine.Debug.Log(m_Input);
                m_Input = string.Empty;
            }
        }
        
        bool Contains(List<Node> nodes, Node node, int index)
        {
            for (int i = 0; i < index; ++i)
            {
                if (Equals(nodes[i], node))
                    return true;
            }
            return false;
        }

        int Count(List<Node> nodes, Node node)
        {
            int count = 0;
            foreach (var item in nodes)
            {
                if (Equals(item, node))
                    ++count;
            }
            return count;
        }

        private string GetLogString(Node node)
        {
            // 最多显示2行
            string message = node.message;
            int index = message.IndexOf('\n');
            if (index != -1)
            {
                index = message.IndexOf('\n', index + 1);
                if (index != -1)
                {
                    message = message.Substring(0, index);
                }
            }

            switch (node.type)
            {
                case LogType.Log:
                    return string.Format("<color=#FFFFFF>{0}</color>", message);
                case LogType.Warning:
                    return string.Format("<color=#FFFF00>{0}</color>", message);
                default:
                    return string.Format("<color=#FF0000>{0}</color>", message);
            }
        }
    }
}
