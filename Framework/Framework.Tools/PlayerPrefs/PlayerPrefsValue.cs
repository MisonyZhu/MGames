
using UnityEngine;

namespace Framework
{
    public class PlayerPrefsValue
    {
        protected static int m_Save;
        protected static bool m_Dirty;

        public static void BeginUpdate() => ++PlayerPrefsValue.m_Save;

        public static void EndUpdate()
        {
            if (--PlayerPrefsValue.m_Save != 0 || !PlayerPrefsValue.m_Dirty)
                return;
            PlayerPrefs.Save();
        }

        public static void Save()
        {
            if (PlayerPrefsValue.m_Save != 0 || !PlayerPrefsValue.m_Dirty)
                return;
            PlayerPrefs.Save();
        }
    }
}