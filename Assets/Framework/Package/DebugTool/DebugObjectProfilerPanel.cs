using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;
using Object = UnityEngine.Object;

namespace Framework.Debug
{
    public class DebugObjectProfilerPanel : DebugPanel
    {
        static Type[] SHOW_TYPES =
        {
            typeof(AnimationClip),
            typeof(AssetBundle),
            typeof(AudioClip),
            typeof(Font),
            typeof(GameObject),
            typeof(Shader),
            typeof(Material),
            typeof(Mesh),
            typeof(Texture),
        };

        sealed class Sample : IComparable<Sample>
        {
            public string name;
            public string desc;
            public int count;
            public long size;

            public Sample(string name, string desc)
            {
                this.name = name;
                this.desc = desc;
                count = 0;
                size = 0;
            }

            public int CompareTo(Sample other)
            {
                return DebugToolUtil.Abs(other.size).CompareTo(DebugToolUtil.Abs(size));
            }
        }

        class TypeSample : IComparable<TypeSample>
        {
            public Type type;
            public int count;
            public long size;
            public List<Sample> samples;

            public TypeSample(Type type)
            {
                this.type = type;
                count = 0;
                size = 0;
                samples = new List<Sample>();
            }

            public int CompareTo(TypeSample other)
            {
                return DebugToolUtil.Abs(other.size).CompareTo(DebugToolUtil.Abs(size));
            }
        }

        class TimeSample
        {
            public int time;
            public int count;
            public long size;
            public List<TypeSample> typeSamples;

            public TimeSample()
            {
                time = Mathf.RoundToInt(Time.realtimeSinceStartup);
                count = 0;
                size = 0;
                typeSamples = new List<TypeSample>();
            }
        }

        const int ShowSampleCount = 5;
        const int ShowSampleCountSelected = 100;
        List<TimeSample> m_TimeSamples = new List<TimeSample>();
        List<TimeSample> m_CompareTimeSamples = new List<TimeSample>();
        TimeSample m_SelecedTimeSample = null;
        bool m_Compare = true;
        Type m_SelectedType = null;
        Vector2 m_ListPosition = Vector2.zero;
        Vector2 m_ContentPosition = Vector2.zero;
        int m_SelectPage = 0;

        public override void OnGUI()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical(GUILayout.Width(250));
                {
                    if (GUILayout.Button(string.Format("Take Sample"), GUILayout.Height(30f)))
                    {
                        TakeSample();
                    }

                    m_ListPosition = GUILayout.BeginScrollView(m_ListPosition, "box");
                    {
                        var style = GUI.skin.label;
                        foreach (var ts in m_TimeSamples)
                        {
                            string name = string.Format("{0} Objects ({1}) at {2}s", ts.count, DebugToolUtil.GetSizeString(ts.size), ts.time);
                            if (ts == m_SelecedTimeSample)
                            {
                                GUI.backgroundColor = Color.grey;
                                style.normal.background = Texture2D.whiteTexture;
                            }

                            if (GUILayout.Button(name, style))
                            {
                                m_SelecedTimeSample = ts;
                            }

                            if (ts == m_SelecedTimeSample)
                            {
                                GUI.backgroundColor = Color.white;
                                style.normal.background = null;
                            }
                        }
                    }
                    GUILayout.EndScrollView();
                }
                GUILayout.EndVertical();

                if (m_SelecedTimeSample != null)
                {
                    GUILayout.BeginVertical();
                    {
                        m_ContentPosition = GUILayout.BeginScrollView(m_ContentPosition);
                        {
                            var timeSample = m_Compare ? m_CompareTimeSamples[m_TimeSamples.IndexOf(m_SelecedTimeSample)] : m_SelecedTimeSample;

                            GUILayout.BeginHorizontal();
                            {
                                m_Compare = GUILayout.Toggle(m_Compare, "<b>Compare With Previous Sample</b>");
                                if (GUILayout.Button("Save", GUILayout.Width(80)))
                                {
                                    SaveTimeSample(timeSample, m_Compare);
                                }
                            }
                            GUILayout.EndHorizontal();

                            foreach (var ts in timeSample.typeSamples)
                            {
                                if (m_SelectedType != null && ts.type != m_SelectedType)
                                    continue;

                                int showCount = m_SelectedType != null ? ShowSampleCountSelected : ShowSampleCount;
                                GUILayout.BeginHorizontal();
                                {
                                    GUILayout.Label(string.Format("<b>{0} {1} ({2})</b>", ts.count, ts.type.Name, DebugToolUtil.GetSizeString(ts.size)));

                                    if (m_SelectedType != null)
                                    {
                                        if (GUILayout.Button("All", GUILayout.Width(80)))
                                        {
                                            m_SelectedType = null;
                                        }
                                    }
                                    else
                                    {
                                        if (GUILayout.Button("Detail", GUILayout.Width(80)))
                                        {
                                            m_SelectPage = 0;
                                            m_SelectedType = ts.type;
                                        }
                                    }
                                }
                                GUILayout.EndHorizontal();

                                GUILayout.BeginVertical("box");
                                {
                                    GUILayout.BeginHorizontal();
                                    {
                                        GUILayout.Label(string.Format("<b>Name</b>"));
                                        GUILayout.Label("<b>Desc</b>", GUILayout.Width(300f));
                                        GUILayout.Label("<b>Count</b>", GUILayout.Width(80f));
                                        GUILayout.Label("<b>Size</b>", GUILayout.Width(80f));
                                    }
                                    GUILayout.EndHorizontal();

                                    int index = m_SelectedType != null ? showCount * m_SelectPage : 0;
                                    int count = Mathf.Min(ts.samples.Count, index + showCount);
                                    for (int i = index; i < count; ++i)
                                    {
                                        var sample = ts.samples[i];
                                        GUILayout.BeginHorizontal();
                                        {
                                            GUILayout.Label(sample.name);
                                            GUILayout.Label(sample.desc, GUILayout.Width(300f));
                                            GUILayout.Label(sample.count.ToString(), GUILayout.Width(80f));
                                            GUILayout.Label(DebugToolUtil.GetSizeString(sample.size), GUILayout.Width(80f));
                                        }
                                        GUILayout.EndHorizontal();
                                    }
                                }
                                GUILayout.EndVertical();

                                if (m_SelectedType != null)
                                    m_SelectPage = DrawPage(m_SelectPage, ts.samples.Count, showCount);
                            }
                        }
                        GUILayout.EndScrollView();
                    }
                    GUILayout.EndVertical();
                }
            }
            GUILayout.EndHorizontal();
        }

        void TakeSample()
        {
            TimeSample timeSample = new TimeSample();
            m_TimeSamples.Add(timeSample);
            m_SelecedTimeSample = timeSample;

            var objs = Resources.FindObjectsOfTypeAll<Object>();
            foreach (var obj in objs)
            {
                if (!IsObjectShow(obj))
                    continue;

                string name = obj.name;
                string desc = GetObjectDesc(obj);
                long size = Profiler.GetRuntimeMemorySizeLong(obj);
                ++timeSample.count;
                timeSample.size += size;

                TypeSample ts = GetOrAddTypeSamples(timeSample.typeSamples, obj.GetType());
                ++ts.count;
                ts.size += size;

                Sample sample = GetOrAddSample(ts.samples, name, desc);
                ++sample.count;
                sample.size += size;
            }

            timeSample.typeSamples.Sort();

            foreach (var ts in timeSample.typeSamples)
            {
                ts.samples.Sort();
            }

            if (m_TimeSamples.Count == 1)
                m_CompareTimeSamples.Add(timeSample);
            else
                m_CompareTimeSamples.Add(CompareTimeSample(m_TimeSamples[m_TimeSamples.Count - 2], timeSample));
        }

        string GetObjectDesc(Object obj)
        {
            if (obj is AnimationClip)
            {
                var animation = obj as AnimationClip;
                return string.Format("{0:0.00}s", animation.length);
            }
            else if (obj is AudioClip)
            {
                var audio = obj as AudioClip;
                return string.Format("{0:0.00}s", audio.length);
            }
            else if (obj is Material)
            {
                var material = obj as Material;
                var shader = material.shader;
                return shader ? shader.name : string.Empty;
            }
            else if (obj is Mesh)
            {
                var mesh = obj as Mesh;
                return string.Format("{0}vs", mesh.vertexCount);
            }
            else if (obj is RenderTexture)
            {
                var renderTexture = obj as RenderTexture;
                return string.Format("{0} {1}x{2}", renderTexture.format, renderTexture.width, renderTexture.height);
            }
            else if (obj is Texture2D)
            {
                var texture2d = obj as Texture2D;
                return string.Format("{0} {1}x{2}", texture2d.format, texture2d.width, texture2d.height);
            }
            else if (obj is Texture3D)
            {
                var texture3d = obj as Texture3D;
                return string.Format("{0} {1}x{2}x{3}", texture3d.format, texture3d.width, texture3d.height, texture3d.depth);
            }
            else if (obj is Cubemap)
            {
                var cubemap = obj as Cubemap;
                return string.Format("{0} {1}x{2}x{3}", cubemap.format, cubemap.width, cubemap.height, cubemap.mipmapCount);
            }
            return string.Empty;
        }

        bool IsObjectShow(Object obj)
        {
            var t = obj.GetType();
            foreach (var st in SHOW_TYPES)
            {
                if (t == st || t.IsSubclassOf(st))
                    return true;
            }
            return false;
        }

        TypeSample GetOrAddTypeSamples(List<TypeSample> typeSamples, Type type)
        {
            foreach (var ts in typeSamples)
            {
                if (ts.type == type)
                    return ts;
            }

            {
                var ts = new TypeSample(type);
                typeSamples.Add(ts);
                return ts;
            }
        }

        Sample GetOrAddSample(List<Sample> samples, string name, string desc)
        {
            foreach (var sample in samples)
            {
                if (sample.name == name && sample.desc == desc)
                    return sample;
            }

            {
                var sample = new Sample(name, desc);
                samples.Add(sample);
                return sample;
            }
        }

        TimeSample CompareTimeSample(TimeSample timeSample1, TimeSample timeSample2)
        {
            var compare = new TimeSample();
            compare.time = timeSample2.time;
            compare.count = timeSample2.count - timeSample1.count;
            compare.size = timeSample2.size - timeSample1.size;

            compare.typeSamples = CompareTypeSamples(timeSample1.typeSamples, timeSample2.typeSamples);
            return compare;
        }

        List<TypeSample> CompareTypeSamples(List<TypeSample> tss1, List<TypeSample> tss2)
        {
            List<TypeSample> tss = new List<TypeSample>();
            foreach (var ts2 in tss2)
            {
                var ts = new TypeSample(ts2.type);
                ts.count = ts2.count;
                ts.size = ts2.size;
                ts.samples = null;

                foreach (var ts1 in tss1)
                {
                    if (ts1.type == ts2.type)
                    {
                        ts.count -= ts1.count;
                        ts.size -= ts1.size;
                        ts.samples = CompareSamples(ts1.samples, ts2.samples);
                    }
                }

                if (ts.samples == null)
                    ts.samples = new List<Sample>(ts2.samples);

                if (ts.samples.Count != 0)
                    tss.Add(ts);
            }

            foreach (var ts1 in tss1)
            {
                bool contains = false;
                foreach (var ts2 in tss2)
                {
                    if (ts2.type == ts1.type)
                    {
                        contains = true;
                        break;
                    }
                }

                if (!contains)
                {
                    var ts = new TypeSample(ts1.type);
                    ts.count = -ts1.count;
                    ts.size = -ts1.size;
                    ts.samples = new List<Sample>(ts1.samples);
                    tss.Add(ts);

                    foreach (var sample in ts.samples)
                    {
                        sample.count = -sample.count;
                        sample.size = -sample.size;
                    }
                }
            }

            tss.Sort();
            return tss;
        }

        List<Sample> CompareSamples(List<Sample> samples1, List<Sample> samples2)
        {
            List<Sample> samples = new List<Sample>();
            foreach (var s2 in samples2)
            {
                var s = new Sample(s2.name, s2.desc);
                s.count = s2.count;
                s.size = s2.size;

                foreach (var s1 in samples1)
                {
                    if (s1.name == s2.name && s1.desc == s2.desc)
                    {
                        s.count -= s1.count;
                        s.size -= s1.size;
                    }
                }

                if (s.count != 0)
                    samples.Add(s);
            }

            foreach (var s1 in samples1)
            {
                bool contains = false;
                foreach (var s2 in samples2)
                {
                    if (s2.name == s1.name && s2.desc == s1.desc)
                    {
                        contains = true;
                        break;
                    }
                }

                if (!contains)
                {
                    var s = new Sample(s1.name, s1.desc);
                    s.count = -s1.count;
                    s.size = -s1.size;
                    samples.Add(s);
                }
            }
            samples.Sort();
            return samples;
        }

        void SaveTimeSample(TimeSample timeSample, bool compare)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("### Total:{0} Objects ({1}) at {2}s", timeSample.count, DebugToolUtil.GetSizeString(timeSample.size), timeSample.time);
            foreach (var ts in timeSample.typeSamples)
            {
                builder.AppendFormat("\n----\n * {0} {1} ({2})\n\n| Name | Desc | Count | Size |\n| - | - | - | - |", ts.count, ts.type.ToString(), DebugToolUtil.GetSizeString(ts.size));
                foreach (var sample in ts.samples)
                {
                    builder.AppendFormat("\n| {0} | {1} | {2} | {3} |", sample.name, sample.desc, sample.count, DebugToolUtil.GetSizeString(sample.size));
                }
            }

            string name = "ObjectProfiler_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            if (compare)
                name += "_Compare";

            string path = debugDir + name + ".md";
            File.WriteAllText(path, builder.ToString());
            Application.OpenURL(path);
        }
    }
}
