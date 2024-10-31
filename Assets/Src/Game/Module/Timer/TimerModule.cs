

using Framework;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimerModule : ModuleBase<TimerModule>
{
    public override int Priority => ModulePriority.TimerPriority;

    public const int LOOP_FOREVER = -1;
    static int m_NextID = 0;
    public static int nextID => ++m_NextID;
    static List<Item> m_Items = new List<Item>();

    class Item : IReference
    {
        public int id;
        public float time;
        public float nextTime;
        public int loop;
        public bool unscaledTime;
        public Action onTime;
        public Action onComplete;

        public void Clear()
        {
            id = 0;
            time = 0;
            nextTime = 0;
            loop = 0;
            unscaledTime = false;
            onTime = null;
            onComplete = null;

        }
    }

    public override void OnShutDown()
    {
        foreach (var item in m_Items) 
        {
            ReferencePool.Release(item);
        }
        m_Items.Clear();
        
    }

    public override void OnUpdate(float detlaTime)
    {
        int count = m_Items.Count;
        float time = Time.time;
        float unscaledTime = Time.unscaledTime;
        for (int i = 0; i < count;)
        {
            var item = m_Items[i];
            bool remove = false;
            if (item.loop == 0)
            {
                remove = true;
            }
            else
            {
                float now = item.unscaledTime ? unscaledTime : time;
                if (item.nextTime <= now)
                {
                    item.onTime?.Invoke();
                    if (item.loop == 1)
                    {
                        remove = true;
                    }
                    else
                    {
                        if (item.loop > 1) --item.loop;
                        item.nextTime += item.time;
                    }
                }
            }

            if (remove)
            {
                item.onComplete?.Invoke();
                ReferencePool.Release(item);
                m_Items.RemoveAt(i);
                --count;
            }
            else
                ++i;
        }
    }

    public static int Start(float time, Action onTime)
    {
        return Start(time, 1, false, onTime, null);
    }

    public static int Start(float time, int loop, Action func, Action onComplete = null)
    {
        return Start(time, loop, false, func, onComplete);
    }

    public static int Start(float time, bool unscaledTime, Action func)
    {
        return Start(time, 1, unscaledTime, func, null);
    }

    public static int Start(float time, int loop, bool unscaledTime, Action onTime, Action onComplete = null)
    {
        var item = ReferencePool.Acquire<Item>();
        item.id = nextID;
        item.time = time;
        item.nextTime = GetTime(unscaledTime) + time;
        item.loop = loop;
        item.unscaledTime = unscaledTime;
        item.onTime = onTime;
        item.onComplete = onComplete;
        m_Items.Add(item);
        return item.id;
    }

    static float GetTime(bool unscaledTime)
    {
        return unscaledTime ? Time.unscaledTime : Time.time;
    }

    public static bool Stop(int id)
    {
        int a = 0;
        int b = m_Items.Count;
        while (a < b)
        {
            int m = (a + b) / 2;
            int mid = m_Items[m].id;
            if (id == mid)
            {
                m_Items[m].loop = 0;
                return true;
            }
            else if (id < mid)
            {
                b = m;
            }
            else
            {
                a = m + 1;
            }
        }
        return false;
    }
}