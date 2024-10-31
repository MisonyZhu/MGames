
using Framework;
using System.Collections.Generic;
using System;

public abstract class DebugSwitch<T> where T : DebugSwitch<T>, new()
{
    private static readonly Dictionary<string, T> m_Name2Switch = new Dictionary<string, T>();

    private static T[] m_Instances;

    private PlayerPrefsBool m_IsOn;

    public static T[] instances
    {
        get
        {
            if (m_Instances == null)
            {
                m_Instances = new T[m_Name2Switch.Count];
                int num = 0;
                foreach (KeyValuePair<string, T> item in m_Name2Switch)
                {
                    m_Instances[num++] = item.Value;
                }

                Array.Sort(m_Instances, (T a, T b) => a.name.CompareTo(b.name));
            }

            return m_Instances;
        }
    }

    public string name { get; private set; }

    public bool isOn
    {
        get
        {
            return m_IsOn;
        }
        set
        {
            if ((bool)m_IsOn != value)
            {
                m_IsOn.Set(value);
                this.m_OnValueChanged?.Invoke(value);
            }
        }
    }

    private event Action<bool> m_OnValueChanged;

    public static T Get(string name, bool @default = false)
    {
        return Get(name, @default, null);
    }

    public static T Get(string name, bool @default, Action<bool> onValueChanged)
    {
        if (!m_Name2Switch.TryGetValue(name, out var value))
        {
            value = new T();
            value.Init(name, @default);
            m_Name2Switch.Add(name, value);
            m_Instances = null;
        }

        if (onValueChanged != null)
        {
            value.m_OnValueChanged += onValueChanged;
        }

        return value;
    }

    public static bool IsOn(string name)
    {
        return Get(name).isOn;
    }

    public static void SetOn(string name, bool on)
    {
        Get(name).isOn = on;
    }

    protected void Init(string name, bool @default = false)
    {
        this.name = name;
        m_IsOn = new PlayerPrefsBool("T" + "_" + name, @default);
    }
}