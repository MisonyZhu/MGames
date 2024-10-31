using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Framework.Debug
{
    public class DebugShaderVariantsProfilerPanel : DebugScrollPanel
    {
        private sealed class Sample
        {
            public string name;
            public string material;
            public int count;

            public override bool Equals(object obj)
            {
                var a = obj as Sample;
                if (a == null)
                    return false;

                return name == a.name;
            }

            public override int GetHashCode()
            {
                return name.GetHashCode();
            }
        }

        private DateTime m_SampleTime = DateTime.MinValue;
        private List<Sample> m_Samples = new List<Sample>();

        protected override void DrawContent()
        {
            GUILayout.Label(string.Format("<b>Shader Variants Information</b>"));
            GUILayout.BeginVertical("box");
            {
                if (GUILayout.Button(string.Format("Take Sample for Shader Variants"), GUILayout.Height(30f)))
                {
                    TakeSample();
                }

                if (m_SampleTime <= DateTime.MinValue)
                {
                    GUILayout.Label(string.Format("<b>Please take sample for Shader Variants first.</b>"));
                }
                else
                {
                    GUILayout.Label(string.Format("<b>{0} Shader Variants obtained at {1}.</b>", m_Samples.Count.ToString(), m_SampleTime.ToString("yyyy-MM-dd HH:mm:ss")));

                    if (m_Samples.Count > 0)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(string.Format("<b>Name</b>"));
                            GUILayout.Label("<b>Material</b>", GUILayout.Width(240f));
                            GUILayout.Label("<b>Count</b>", GUILayout.Width(80f));
                        }
                        GUILayout.EndHorizontal();
                    }

                    for (int i = 0; i < m_Samples.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(m_Samples[i].name);
                            GUILayout.Label(m_Samples[i].material, GUILayout.Width(240f));
                            GUILayout.Label(m_Samples[i].count.ToString(), GUILayout.Width(80f));
                        }
                        GUILayout.EndHorizontal();
                    }
                }
            }
            GUILayout.EndVertical();
        }

        private void TakeSample()
        {
            m_SampleTime = DateTime.Now;
            m_Samples.Clear();

            var materials = Resources.FindObjectsOfTypeAll<Material>();
            foreach (var material in materials)
            {
                if (material.shaderKeywords.Length == 0)
                    continue;

                string text = material.shader.name + ":" + Concat(material.shaderKeywords, " ");
                int index = -1;
                for (int i = 0; i < m_Samples.Count; ++i)
                {
                    if (m_Samples[i].name == text)
                    {
                        index = i;
                        break;
                    }
                }

                if (index == -1)
                    m_Samples.Add(new Sample() { name = text, material = material.name, count = 1 });
                else
                    ++m_Samples[index].count;
            }

            m_Samples.Sort(SampleComparer);
        }

        string Concat(string[] texts, string split)
        {
            if (texts == null || texts.Length == 0)
                return "";

            StringBuilder builder = new StringBuilder();
            builder.Append(texts[0]);

            for (int i = 1; i < texts.Length; ++i)
            {
                builder.Append(split).Append(texts[i]);
            }
            return builder.ToString();
        }

        private int SampleComparer(Sample a, Sample b)
        {
            return a.name.CompareTo(b.name);
        }
    }
}