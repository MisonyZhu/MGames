using System;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Profiling;
using Framework;

namespace Framework.Debug
{
    public class DebugTool : MonoSingleton<DebugTool>
    {
        const float DEFAULT_ICON_X = 10;
        const float DEFAULT_ICON_Y = 10;
        const float ICON_LINE_HEIGHT = 20;

        public enum ShowType
        {
            Auto,
            Show,
            Hide,
        }
        public ShowType show = ShowType.Auto;

        public PlayerPrefsFloat iconX;
        public PlayerPrefsFloat iconY;
        public PlayerPrefsBool showFPS;
        public PlayerPrefsBool showMono;
        public PlayerPrefsBool showNative;
        public PlayerPrefsBool showLua;
#if UNITY_ANDROID && !UNITY_EDITOR
        public PlayerPrefsBool showPSS;
#endif
        public Action customDrawIconFunc;

        bool showBundleMode
        {
            get
            {
#if UNITY_EDITOR
                return true; //ResourceManager.mode != ResourceMode.Editor;
#else
                return false;
#endif
            }
        }

        float iconButtonHeight
        {
            get
            {
                float height = 20;
                if (showBundleMode) height += ICON_LINE_HEIGHT;
                if (showFPS) height += ICON_LINE_HEIGHT;
                if (showMono) height += ICON_LINE_HEIGHT;
                if (showNative) height += ICON_LINE_HEIGHT;
                if (showLua) height += ICON_LINE_HEIGHT;
#if UNITY_ANDROID && !UNITY_EDITOR
                if (showPSS) height += ICON_LINE_HEIGHT;
#endif
                return height;
            }
        }

        Rect iconRect
        {
            get
            {
                return new Rect(iconX, iconY, 150, iconButtonHeight + 35);
            }
            set
            {
                PlayerPrefsValue.BeginUpdate();
                iconX.Set(value.x);
                iconY.Set(value.y);
                PlayerPrefsValue.EndUpdate();
            }
        }

        const float WINDOW_HEIGHT = 720;
        Rect windowRect
        {
            get
            {
                float width = Screen.width / windowScale;
                return new Rect(0, 0, width, WINDOW_HEIGHT);
            }
        }

        float windowScale
        {
            get { return Screen.height / WINDOW_HEIGHT; }
        }

        static readonly Rect DRAG_RECT = new Rect(0, 0, float.MaxValue, 25);

        bool m_Minimize = true;
        public bool minimize
        {
            get { return m_Minimize; }
            set
            {
                if (m_Minimize != value)
                {
                    m_Minimize = value;
                    RefreshInteractable();

                    if (visible)
                    {
                        if (m_Minimize)
                            m_Root.OnLeave();
                        else
                            m_Root.OnEnter();
                    }
                }
            }
        }

        public static bool visible
        {
            get { return hasInstance && Instance.enabled; }
            set
            {
                if (visible == value)
                    return;

                Instance.enabled = value;
                Instance.RefreshInteractable();

                if (!Instance.minimize)
                {
                    if (value)
                        Instance.m_Root.OnEnter();
                    else
                        Instance.m_Root.OnLeave();
                }
            }
        }

        public static bool isOpen
        {
            get { return visible && !Instance.minimize; }
        }

        EventSystem m_EventSystem;
        void RefreshInteractable()
        {
            if (!m_EventSystem)
                m_EventSystem = EventSystem.current;
            if (m_EventSystem)
                m_EventSystem.enabled = m_Minimize;
        }

        FPSCounter m_FPSConter = new FPSCounter(1);
        DebugPanelRoot m_Root;

        protected override void Awake()
        {
            base.Awake();
            switch (show)
            {
                case ShowType.Auto:
                    if (!UnityEngine.Debug.isDebugBuild)
                        enabled = true;
                    break;
                case ShowType.Hide:
                    enabled = false;
                    break;
            }
        }

        bool IsRectOutOfScreen(Rect rect)
        {
            return rect.xMax > Screen.width || rect.xMin < 0 || rect.yMax > Screen.height || rect.yMin < 0;
        }

        private void OnEnable()
        {
            Init();
            m_Root.OnEnable();
        }

        private void OnDisable()
        {
            m_Root.OnDisable();
        }

        DebugConsolePanel m_Console;
        bool m_Inited = false;
        void Init()
        {
            if (m_Inited)
                return;

            m_Inited = true;
            minimize = true;

            iconX = new PlayerPrefsFloat("DebugWindow.iconX", DEFAULT_ICON_X);
            iconY = new PlayerPrefsFloat("DebugWindow.iconY", DEFAULT_ICON_Y);
            showFPS = new PlayerPrefsBool("DebugWindow.showFPS", true);
            showMono = new PlayerPrefsBool("DebugWindow.showMono", false);
            showNative = new PlayerPrefsBool("DebugWindow.showNative", false);
            //showLua = new PlayerPrefsBool("DebugWindow.showLua", false);
#if UNITY_ANDROID && !UNITY_EDITOR
            showPSS = new PlayerPrefsBool("DebugWindow.showPSS", false);
#endif

            InitPanels();

            if (IsRectOutOfScreen(iconRect))
            {
                ResetSize();
            }
        }

        void InitPanels()
        {
            m_Root = new DebugPanelRoot();
            m_Console = new DebugConsolePanel();
            AddPanel("Console", m_Console);
            AddPanel("Hierarchy", new DebugHierarchyPanel());
            AddPanel("Infomation", new DebugPanelGroup());
            AddPanel("Infomation/System", new DebugSystemInfomationPanel());
            AddPanel("Infomation/Environment", new DebugEnvironmentInfomationPanel());
            AddPanel("Infomation/Screen", new DebugScreenInfomationPanel());
            AddPanel("Infomation/Graphics", new DebugGraphicsInfomationPanel());
            AddPanel("Infomation/Quality", new DebugQualityInfomationPanel());
            AddPanel("Infomation/Input", new DebugPanelGroup());
            AddPanel("Infomation/Input/Summary", new DebugSummaryInputInfomationPanel());
            AddPanel("Infomation/Input/Touch", new DebugTouchInputInfomationPanel());
            AddPanel("Infomation/Input/Location", new DebugLocationInputInfomationPanel());
            AddPanel("Infomation/Input/Acceleration", new DebugAccelerationInputInfomationPanel());
            AddPanel("Infomation/Input/Gyroscope", new DebugGyroscopeInputInfomationPanel());
            AddPanel("Infomation/Input/Compass", new DebugCompassInputInfomationPanel());
            AddPanel("Infomation/Other", new DebugPanelGroup());
            AddPanel("Infomation/Other/Scene", new DebugSceneInfomationPanel());
            AddPanel("Infomation/Other/Path", new DebugPathInfomationPanel());
            AddPanel("Infomation/Other/Time", new DebugTimeInfomationPanel());
            AddPanel("Profiler", new DebugPanelGroup());
            AddPanel("Profiler/Summary", new DebugSummaryProfilerPanel());
            AddPanel("Profiler/Memory", new DebugPanelGroup());
            AddPanel("Profiler/Memory/All", new DebugMemoryProfilerPanel<UnityEngine.Object>());
            AddPanel("Profiler/Memory/Texture", new DebugMemoryProfilerPanel<Texture>());
            AddPanel("Profiler/Memory/Mesh", new DebugMemoryProfilerPanel<Mesh>());
            AddPanel("Profiler/Memory/Material", new DebugMemoryProfilerPanel<Material>());
            AddPanel("Profiler/Memory/Shader", new DebugMemoryProfilerPanel<Shader>());
            AddPanel("Profiler/Memory/AnimationClip", new DebugMemoryProfilerPanel<AnimationClip>());
            AddPanel("Profiler/Memory/AudioClip", new DebugMemoryProfilerPanel<AudioClip>());
            AddPanel("Profiler/Memory/Font", new DebugMemoryProfilerPanel<Font>());
            AddPanel("Profiler/Memory/GameObject", new DebugMemoryProfilerPanel<GameObject>());
            AddPanel("Profiler/Memory/Component", new DebugMemoryProfilerPanel<Component>());
            AddPanel("Profiler/Object", new DebugObjectProfilerPanel());
            //AddPanel("Profiler/Asset", new DebugAssetPanel());
            // AddPanel("Profiler/Bundle", new DebugBundlePanel());
            // AddPanel("Profiler/LoadBundle", new DebugLoadBundlePanel());
            // AddPanel("Profiler/Download", new DebugDownloadPanel());
            AddPanel("Profiler/Function", new DebugFunctionProfilerPanel());
            //AddPanel("Profiler/LuaFunction", new DebugLuaFunctionProfilerPanel());
            AddPanel("Setting", new DebugSettingPanel());
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();//.Where(t => String.Equals(t.Namespace, "CustomDebugToolPanle", StringComparison.Ordinal)).ToArray();
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i].Namespace != null && types[i].Namespace.Equals("CustomDebugToolPanle"))
                {
                    var obj = Activator.CreateInstance(types[i]);
                    FieldInfo panelNameField = types[i].GetField("panelName");
                    string panelName = (string)panelNameField.GetValue(obj);
                    AddPanel(panelName, (DebugPanel)obj);
                }
            }
        }

        public void ResetSize()
        {
            iconRect = new Rect(DEFAULT_ICON_X, DEFAULT_ICON_Y, 0, 0);
        }

        public void AddPanel(string path, DebugPanel panel)
        {
            m_Root.AddPanel(path, panel);
        }
        
        private void Update()
        {
            m_FPSConter.Update();
        }

        private void LateUpdate()
        {
            UnityEngine.Debug.developerConsoleVisible = false;
        }

        private void OnGUI()
        {
            var oldMatrix = GUI.matrix;
            float scale = windowScale;
            GUI.matrix = Matrix4x4.Scale(new Vector3(scale, scale, scale));

            if (minimize)
                iconRect = GUI.Window(0, iconRect, DrawIcon, "<b>DEBUG TOOL</b>");
            else
                GUI.Window(0, windowRect, DrawWindow, "<b>DEBUG TOOL</b>");

            GUI.matrix = oldMatrix;
        }

        void DrawIcon(int windowID)
        {
            GUI.DragWindow(DRAG_RECT);
            GUILayout.Space(5);

            GUI.color = GetIconTextColor();
            if (customDrawIconFunc != null)
                customDrawIconFunc();
            else
            {
                if (GUILayout.Button(GetIconText(), GUILayout.ExpandWidth(true), GUILayout.Height(iconButtonHeight)))
                {
                    minimize = false;
                }
            }
            GUI.color = Color.white;
        }

        StringBuilder builder = new StringBuilder();
        string GetIconText()
        {
            builder.Clear();
            bool first = true;
            if (showBundleMode) AppendText(ref first, "Bundle Mode");
            if (showFPS) AppendText(ref first, "<b>FPS:{0:D2} MIN:{1:D2}</b>", Mathf.RoundToInt(m_FPSConter.FPS), Mathf.RoundToInt(m_FPSConter.minFPS));
            if (showMono) AppendText(ref first, "<b>Mono:{0:0.00}MB</b>", DebugToolUtil.ToMB(Profiler.GetMonoUsedSizeLong()));
            if (showNative) AppendText(ref first, "<b>Native:{0:0.00}MB</b>", DebugToolUtil.ToMB(Profiler.GetTotalAllocatedMemoryLong()));
#if UNITY_ANDROID && !UNITY_EDITOR
            if (showPSS)
            {
                string pss = DebugToolUtil.memoryStats["summary.total-pss"];
                AppendText(ref first, "<b>PSS:{0:0.00MB}</b>", int.Parse(pss) / 1024f);
            }
#endif
            return builder.ToString();
        }

        void AppendText(ref bool first, string format, params object[] args)
        {
            if (first)
                first = false;
            else
                builder.Append('\n');
            builder.AppendFormat(format, args);
        }

        Color GetIconTextColor()
        {
            if (m_Console.errorCount > 0)
                return Color.red;
            else if (m_Console.warningCount > 0)
                return Color.yellow;
            else
                return Color.white;
        }

        void DrawWindow(int windowID)
        {
            GUI.DragWindow(DRAG_RECT);
            GUILayout.Space(5);
            m_Root.OnGUI();
        }
    }
}