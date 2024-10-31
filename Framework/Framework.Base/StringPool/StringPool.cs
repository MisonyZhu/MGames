
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Framework
{

    public static class StringPool
    {
        private static List<string> m_CacheStrings = new List<string>();

        private static Dictionary<SubString, int> m_SubStringMap = new Dictionary<SubString, int>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [CompilerGenerated]
        private static int m_MemorySize;

        public static int count => m_CacheStrings.Count;

        //用于跟踪缓存字符串使用的总内存。当向池中添加新字符串时，它会增加相应的内存量。
        public static int memory
        {
            [CompilerGenerated]
            get
            {
                return m_MemorySize;
            }
            [CompilerGenerated]
            private set
            {
                m_MemorySize = value;
            }
        }

        public static int Add(string str)
        {
            return Add(str, 0, str.Length);
        }

        public static int Add(string str, int index)
        {
            return Add(str, index, str.Length - index);
        }

        public static int Add(string str, int index, int count)
        {
            if (count == 0)
            {
                return 0;
            }

            SubString key = new SubString(str, index, count);
            if (!m_SubStringMap.TryGetValue(key, out var value))
            {
                string text = key.ToString();
                m_CacheStrings.Add(text);
                value = m_CacheStrings.Count;
                m_SubStringMap.Add(new SubString(text), value);
                memory += text.Length * 2;
            }

            return value;
        }

        public static int Add(StringBuilder sb)
        {
            return Add(sb, 0, sb.Length);
        }

        public static int Add(StringBuilder sb, int index)
        {
            return Add(sb, index, sb.Length - index);
        }

        public static int Add(StringBuilder sb, int index, int count)
        {
            if (count == 0)
            {
                return 0;
            }

            SubString key = new SubString(sb, index, count);
            if (!m_SubStringMap.TryGetValue(key, out var value))
            {
                string text = key.ToString();
                m_CacheStrings.Add(text);
                value = m_CacheStrings.Count;
                m_SubStringMap.Add(new SubString(text), value);
                memory += text.Length * 2;
            }

            return value;
        }

        public static string Get(int id)
        {
            if (id <= 0)
            {
                return "";
            }

            return m_CacheStrings[id - 1];
        }
    }
}