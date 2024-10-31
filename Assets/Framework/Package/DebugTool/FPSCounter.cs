using UnityEngine;

namespace Framework.Debug
{
    public class FPSCounter
    {
        float m_Interval = 1;
        float m_Time = 0;
        int m_Count = 0;
        float m_FPS = 0;
        float m_MaxTime = 0;
        float m_MinFPS = 0;

        public float FPS
        {
            get { return m_FPS; }
        }

        public float minFPS
        {
            get { return m_MinFPS; }
        }

        public FPSCounter(float interval = 1)
        {
            m_Interval = interval;
        }

        public void Update()
        {
            ++m_Count;
            var dt = Time.unscaledDeltaTime;
            if (m_MaxTime < dt)
                m_MaxTime = dt;

            m_Time += dt;
            if (m_Time > m_Interval)
            {
                m_FPS = m_Count / m_Time;
                m_Count = 0;
                m_MinFPS = 1 / m_MaxTime;
                m_MaxTime = 0;
                m_Time = 0;
            }
        }
    }
}