using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Framework.Debug
{
    public class DebugSummaryProfilerPanel : DebugScrollPanel
    {
        GameObject m_UWA_Launcher;

        public DebugSummaryProfilerPanel()
        {
            m_UWA_Launcher = GameObject.Find("/UWA_Launcher");
        }

        protected override void DrawContent()
        {
            GUILayout.Label("<b>Profiler Information</b>");
            using (new GUILayout.VerticalScope("box"))
            {
                DrawItem("Supported:", Profiler.supported.ToString());
                DrawItem("Enabled:", Profiler.enabled.ToString());
                EnableBinaryLog(DrawToggle("Enable Binary Log", Profiler.enableBinaryLog));
                if (DrawItemButton("Mono Used Size:", string.Format("{0:0.000} MB", DebugToolUtil.ToMB(Profiler.GetMonoUsedSizeLong())), "GC Collect"))
                {
                    GC.Collect();
                }
                DrawItem("Mono Heap Size:", string.Format("{0:0.000} MB", DebugToolUtil.ToMB(Profiler.GetMonoHeapSizeLong())));
                DrawItem("Used Heap Size:", string.Format("{0:0.000} MB", DebugToolUtil.ToMB(Profiler.usedHeapSizeLong)));
                DrawItem("Total Allocated Memory:", string.Format("{0:0.000} MB", DebugToolUtil.ToMB(Profiler.GetTotalAllocatedMemoryLong())));
                DrawItem("Total Reserved Memory:", string.Format("{0:0.000} MB", DebugToolUtil.ToMB(Profiler.GetTotalReservedMemoryLong())));
                DrawItem("Total Unused Reserved Memory:", string.Format("{0:0.000} MB", DebugToolUtil.ToMB(Profiler.GetTotalUnusedReservedMemoryLong())));
                DrawItem("Temp Allocator Size:", string.Format("{0:0.000} MB", DebugToolUtil.ToMB(Profiler.GetTempAllocatorSize())));

                if (m_UWA_Launcher)
                {
                    m_UWA_Launcher.SetActive(DrawToggle("UWA Enabled", m_UWA_Launcher.activeSelf));
                }
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            GUILayout.Label("<b>Android</b>");
            using (new GUILayout.VerticalScope("box"))
            {
                foreach (var item in DebugToolUtil.memoryStats)
                {
                    int size;
                    if (int.TryParse(item.Value, out size))
                        DrawItem(item.Key, string.Format("{0:0.000} MB", size / 1024f));
                    else
                        DrawItem(item.Key, item.Value);
                }
            }
#endif
            if (GUILayout.Button("Clear Memory"))
            {
                GC.Collect();
                Resources.UnloadUnusedAssets();
            }
        }

        void EnableBinaryLog(bool enable)
        {
            if (Profiler.enableBinaryLog == enable)
                return;

            if (enable)
                SetLogFile();

            Profiler.enableBinaryLog = enable;
            if (enable)
                Profiler.enabled = enable;
        }

        void SetLogFile()
        {
            Profiler.logFile = string.Concat(debugDir, "Profiler_", DateTime.Now.ToString("yyyyMMdd_HHmmss"), ".raw");
        }

        public override void OnDisable()
        {
            Profiler.logFile = "";
        }
    }
}