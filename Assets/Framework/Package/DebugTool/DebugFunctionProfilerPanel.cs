using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Framework;

namespace Framework.Debug
{
    public class DebugFunctionProfilerPanel : DebugPanel
    {
        const float WIDTH = 120;
        const int MAX_SHOW_COUNT = 110;
        string m_Name;
        string m_Filter;
        Vector2 m_ListPosition = Vector2.zero;
        Vector2 m_ContentPosition = Vector2.zero;
        List<FunctionProfiler.Record> m_Records = new List<FunctionProfiler.Record>();
        Comparison<FunctionProfiler.Record> m_CompareFunc;

        public DebugFunctionProfilerPanel()
        {
            m_CompareFunc = SortByTotalTime;
        }

        void RefreshRecords()
        {
            foreach (var item in FunctionProfiler.name2Records)
            {
                if (m_Name == null)
                {
                    m_Name = item.Key;
                }

                if (item.Key == m_Name)
                {
                    GetRecords(item.Value, m_Filter, m_Records);
                    break;
                }
            }
        }

        void GetRecords(Dictionary<string, FunctionProfiler.Record> records, string filter, List<FunctionProfiler.Record> result)
        {
            var total = new FunctionProfiler.Record(m_Name, "Total");
            result.Clear();
            result.Add(total);

            foreach (var item in records)
            {
                if (string.IsNullOrEmpty(filter) || item.Key.IndexOf(filter, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    var record = item.Value;
                    if (!string.IsNullOrEmpty(record.info))
                    {
                        result.Add(record);
                    }

                    total.count += record.count;
                    total.maxTime = Mathf.Max(total.maxTime, record.maxTime);
                    total.totalTime += record.totalTime;
                }
            }
            result.Sort(m_CompareFunc);
        }

        public override void OnGUI()
        {
            RefreshRecords();
            using (new GUILayout.HorizontalScope())
            {
                using (new GUILayout.VerticalScope(GUILayout.Width(250)))
                {
                    if (GUILayout.Button("Clear", GUILayout.Height(30f)))
                    {
                        FunctionProfiler.ClearAll();
                    }

                    m_ListPosition = GUILayout.BeginScrollView(m_ListPosition, "box");
                    {
                        var style = GUI.skin.label;
                        foreach (var item in FunctionProfiler.name2Records)
                        {
                            NameButton(item.Key, style);
                        }
                    }
                    GUILayout.EndScrollView();
                }

                using (new GUILayout.VerticalScope("box"))
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Search");
                        m_Filter = GUILayout.TextField(m_Filter, GUILayout.Width(200));
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Save", GUILayout.Width(120)))
                        {
                            Save();
                        }

                        if (GUILayout.Button("Save All", GUILayout.Width(120)))
                        {
                            SaveAll();
                        }
                    }

                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Space(4);
                        GUILayout.Label(string.Format("<b>Name</b>"));
                        if (GUILayout.Button("<b>Count</b>", "label", GUILayout.Width(WIDTH)))
                        {
                            m_CompareFunc = SortByCount;
                        }
                        if (GUILayout.Button("<b>TotalTime ms</b>", "label", GUILayout.Width(WIDTH)))
                        {
                            m_CompareFunc = SortByTotalTime;
                        }
                        if (GUILayout.Button("<b>AvarageTime ms</b>", "label", GUILayout.Width(WIDTH)))
                        {
                            m_CompareFunc = SortByAvarageTime;
                        }
                        if (GUILayout.Button("<b>MaxTime ms</b>", "label", GUILayout.Width(WIDTH)))
                        {
                            m_CompareFunc = SortByMaxTime;
                        }
                        GUILayout.Space(20);
                    }

                    m_ContentPosition = GUILayout.BeginScrollView(m_ContentPosition, false, true);
                    {
                        int count = 0;
                        for (int i = 0; i < m_Records.Count; ++i)
                        {
                            var record = m_Records[i];
                            using (new GUILayout.HorizontalScope())
                            {
                                GUILayout.Label(record.info);
                                GUILayout.Label(record.count.ToString(), GUILayout.Width(WIDTH));
                                GUILayout.Label(GetTimeText(record.totalTime), GUILayout.Width(WIDTH));
                                GUILayout.Label(GetTimeText(record.totalTime / record.count), GUILayout.Width(WIDTH));
                                GUILayout.Label(GetTimeText(record.maxTime), GUILayout.Width(WIDTH));
                            }

                            if (++count >= MAX_SHOW_COUNT)
                                break;
                        }
                    }
                    GUILayout.EndScrollView();
                }
            }
        }

        void NameButton(string name, GUIStyle style)
        {
            if (name == m_Name)
            {
                GUI.backgroundColor = Color.grey;
                style.normal.background = Texture2D.whiteTexture;
            }

            if (GUILayout.Button(name, style))
            {
                m_Name = name;
            }

            if (name == m_Name)
            {
                GUI.backgroundColor = Color.white;
                style.normal.background = null;
            }
        }

        int SortByCount(FunctionProfiler.Record a, FunctionProfiler.Record b)
        {
            return b.count.CompareTo(a.count);
        }

        int SortByTotalTime(FunctionProfiler.Record a, FunctionProfiler.Record b)
        {
            return b.totalTime.CompareTo(a.totalTime);
        }

        int SortByAvarageTime(FunctionProfiler.Record a, FunctionProfiler.Record b)
        {
            return (b.totalTime / b.count).CompareTo(a.totalTime / a.count);
        }

        int SortByMaxTime(FunctionProfiler.Record a, FunctionProfiler.Record b)
        {
            return b.maxTime.CompareTo(a.maxTime);
        }

        void Save()
        {
            StringBuilder strb = new StringBuilder();
            SaveRecords(strb, m_Name, m_Filter, m_Records);
            SaveFile(strb);
        }

        void SaveAll()
        {
            StringBuilder strb = new StringBuilder();
            List<FunctionProfiler.Record> result = new List<FunctionProfiler.Record>();
            foreach (var item in FunctionProfiler.name2Records)
            {
                GetRecords(item.Value, string.Empty, result);
                SaveRecords(strb, item.Key, string.Empty, result);
            }
            SaveFile(strb);
        }

        void SaveFile(StringBuilder strb)
        {
            string name = "FunctionProfiler_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string path = debugDir + "/" + name + ".md";
            File.WriteAllText(path, strb.ToString());

            if (Application.isEditor)
            {
                Application.OpenURL(path);
            }
        }

        static void SaveRecords(StringBuilder strb, string name, string filter, List<FunctionProfiler.Record> records)
        {
            strb.Append("***\n## ").Append(name);
            if (!string.IsNullOrEmpty(filter))
            {
                strb.Append(", Filter:").Append(filter);
            }

            strb.Append("\n| Name | Count | TotalTime ms | AvarageTime ms | MaxTime ms |\n| - | - | - | - | - |\n");

            for (int i = 0; i < records.Count; ++i)
            {
                var record = records[i];
                if (!CheckFilter(record, filter)) continue;

                strb.AppendFormat("| {0} | {1} | {2} | {3} | {4} |\n", record.info, record.count, GetTimeText(record.totalTime), GetTimeText(record.totalTime / record.count), GetTimeText(record.maxTime));
            }
        }

        static bool CheckFilter(FunctionProfiler.Record record, string filter)
        {
            return string.IsNullOrEmpty(filter) || record.info.IndexOf(filter, StringComparison.OrdinalIgnoreCase) != -1;
        }

        static string GetTimeText(float time)
        {
            return string.Format("{0:0.00}", time * 1000);
        }
    }
}
