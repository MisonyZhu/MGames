using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework.Debug
{
    public static class DebugToolUtil
    {
        public const float MB = 1024 * 1024;

        public static float ToMB(long size)
        {
            return size / MB;
        }

        public static string GetSizeString(long size)
        {
            long asize = Abs(size);
            if (asize < 1024L)
            {
                return string.Format("{0} Bytes", size);
            }

            if (asize < 1024L * 1024L)
            {
                return string.Format("{0:0.00} KB", size / 1024f);
            }

            if (asize < 1024L * 1024L * 1024L)
            {
                return string.Format("{0:0.00} MB", size / 1024f / 1024f);
            }

            if (asize < 1024L * 1024L * 1024L * 1024L)
            {
                return string.Format("{0:0.00} GB", size / 1024f / 1024f / 1024f);
            }

            return string.Format("{0:0.00} TB", size / 1024f / 1024f / 1024f / 1024f);
        }

        public static long Abs(long size)
        {
            return size < 0 ? -size : size;
        }

        public static string GetTimeTextMS(float time)
        {
            return string.Format("{0:0.00}", time * 1000);
        }

        public static string GetTimeTextMS(double time)
        {
            return string.Format("{0:0.00}", time * 1000);
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        static int[] m_ProcessID = null;
        public static int[] processID
        {
            get
            {
                if (m_ProcessID == null)
                {
                    using (AndroidJavaObject process = new AndroidJavaClass("android.os.Process"))
                    {
                        int pid = process.CallStatic<int>("myPid");
                        m_ProcessID = new int[] { pid };
                    }
                }
                return m_ProcessID;
            }
        }

        static AndroidJavaObject m_SystemService = null;
        public static AndroidJavaObject systemService
        {
            get
            {
                if (m_SystemService == null)
                {
                    using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    {
                        using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                        {
                            using (AndroidJavaObject application = currentActivity.Call<AndroidJavaObject>("getApplication"))
                            {
                                m_SystemService = application.Call<AndroidJavaObject>("getSystemService", "activity");
                            }
                        }
                    }
                }
                return m_SystemService;
            }
        }

        public static AndroidJavaObject GetProcessMemoryInfo()
        {
            return systemService.Call<AndroidJavaObject[]>("getProcessMemoryInfo", processID)[0];
        }
        
        public static float GetMemory(string field)
        {
            try
            {
                using (new FunctionProfiler.Scope(field, "GetMemory"))
                {
                    using (var mi = GetProcessMemoryInfo())
                    {
                        return mi.Get<int>(field) / 1024f;
                    }
                }
            }
            catch
            {
                return 0;
            }
        }

        public static float CallMemory(string method)
        {
            try
            {
                using (new FunctionProfiler.Scope(method, "GetMemory"))
                {
                    using (var mi = GetProcessMemoryInfo())
                    {
                        return mi.Call<int>(method) / 1024f;
                    }
                }
            }
            catch
            {
                return 0;
            }
        }

        class BiConsumerProxy<T, U> : AndroidJavaProxy
        {
            Action<T, U> m_OnAccept;

            public BiConsumerProxy(Action<T, U> onAccept) : base("java.util.function.BiConsumer")
            {
                m_OnAccept = onAccept;
            }

            public void accept(T t, U u)
            {
                if (m_OnAccept != null)
                    m_OnAccept(t, u);
            }
        }


        const float GET_MEMORY_STATES_INTERVAL = 1;
        static float m_LastGetMemoryStatsTime = -1;
        static Dictionary<string, string> m_MemoryStatus;

        public static Dictionary<string, string> memoryStats
        {
            get
            {
                if (m_MemoryStatus == null || Time.unscaledTime - m_LastGetMemoryStatsTime > GET_MEMORY_STATES_INTERVAL)
                {
                    m_LastGetMemoryStatsTime = Time.unscaledTime;

                    if (m_MemoryStatus == null)
                        m_MemoryStatus = new Dictionary<string, string>();
                    else
                        m_MemoryStatus.Clear();

                    try
                    {
                        using (new FunctionProfiler.Scope("Stats", "GetMemory"))
                        {
                            using (var mi = GetProcessMemoryInfo())
                            {
                                var statsObj = mi.Call<AndroidJavaObject>("getMemoryStats");
                                statsObj.Call("forEach", new BiConsumerProxy<string, string>((key, value) =>
                                {
                                    m_MemoryStatus.Add(key, value);
                                }));
                            }
                        }
                    }
                    catch
                    { }
                }
                return m_MemoryStatus;
            }
        }
#endif
    }
}