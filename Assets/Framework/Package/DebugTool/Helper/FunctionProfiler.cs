using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Framework;

namespace Framework.Debug
{

    public static class FunctionProfiler
    {
        public class Record
        {
            public string name;

            public string info;

            public int count;

            public float maxTime;

            public float totalTime;

            public Record(string name, string info)
            {
                this.name = name;
                this.info = info;
                count = 0;
                maxTime = 0f;
                totalTime = 0f;
            }

            public void Add(float time)
            {
                count++;
                if (maxTime < time)
                {
                    maxTime = time;
                }

                totalTime += time;
            }
        }

        private struct Sample
        {
            public int nextFreeIndex;

            public string name;

            public string info;

            public float startTime;

            public void Begin(string name, string info)
            {
                this.name = name;
                this.info = info;
                startTime = Time.realtimeSinceStartup;
            }

            public float End()
            {
                return Time.realtimeSinceStartup - startTime;
            }
        }

        public struct Scope : IDisposable
        {
            private int m_ID;

            public Scope(string name, string info)
            {
                m_ID = BeginSample(name, info);
            }

            public void Dispose()
            {
                EndSample(m_ID);
            }
        }

        public struct Sampler
        {
            private int m_ID;

            [Conditional("ENABLE_PROFILER")]
            public void Begin(string name, string info = "")
            {
                m_ID = BeginSample(name, info);
            }

            [Conditional("ENABLE_PROFILER")]
            public void End()
            {
                EndSample(m_ID);
            }
        }

        public const int INVALID_SAMPLE_ID = -2;

        public static Dictionary<string, Dictionary<string, Record>> name2Records;

        private static Sample[] m_Samples;

        private static int m_SampleIndex;

        private static List<int> m_SampleIds;

        static FunctionProfiler()
        {
            name2Records = new Dictionary<string, Dictionary<string, Record>>();
            m_SampleIndex = -1;
            m_SampleIds = new List<int>(1000);
            m_Samples = new Sample[1000];
            m_SampleIndex = 0;
            StartSample(0);
        }

        private static void StartSample(int startIndex)
        {
            int num = m_Samples.Length - 1;
            for (int i = startIndex; i < num; i++)
            {
                m_Samples[i].nextFreeIndex = i + 1;
            }

            m_Samples[num].nextFreeIndex = -1;
        }

        private static int GetNextSampleIndex()
        {
            if (m_SampleIndex != -1)
            {
                int num = m_SampleIndex;
                m_SampleIndex = m_Samples[num].nextFreeIndex;
                return num;
            }

            int num2 = m_Samples.Length;
            Array.Resize(ref m_Samples, num2 + 100);
            m_SampleIndex = num2 + 1;
            StartSample(num2);
            return num2;
        }

        private static Record GetRecord(string groupKey, string recordKey)
        {
            if (!name2Records.TryGetValue(groupKey, out var value))
            {
                value = new Dictionary<string, Record>();
                name2Records.Add(groupKey, value);
            }

            if (!value.TryGetValue(recordKey, out var value2))
            {
                value2 = new Record(groupKey, recordKey);
                value.Add(recordKey, value2);
            }

            return value2;
        }

        public static int BeginSample(string name, string info = "")
        {
            int num = GetNextSampleIndex();
            m_Samples[num].Begin(name, info);
            m_SampleIds.Add(num);
            return num;
        }

        public static int BeginSample(int nameID, int infoID = -1)
        {
            return BeginSample(StringPool.Get(nameID), (infoID < 0) ? null : StringPool.Get(infoID));
        }

        public static void EndSample(int id = -1)
        {
            switch (id)
            {
                case -2:
                    return;
                case -1:
                    id = m_SampleIds[m_SampleIds.Count - 1];
                    break;
            }

            float time = m_Samples[id].End();
            string name = m_Samples[id].name;
            Record record = GetRecord(name, m_Samples[id].info);
            record.Add(time);
            m_Samples[id].nextFreeIndex = m_SampleIndex;
            m_SampleIndex = id;
            m_SampleIds.Remove(id);
        }

        public static void Clear(string name)
        {
            name2Records.Remove(name);
        }

        public static void ClearAll()
        {
            name2Records.Clear();
        }
    }
}
