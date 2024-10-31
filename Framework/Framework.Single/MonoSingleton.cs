using UnityEngine;

namespace Framework
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        protected static T m_Instance;

        public static T Instance
        {
            get
            {
                MonoSingleton<T>.CheckInstance();
                return MonoSingleton<T>.m_Instance;
            }
        }

        public static void CheckInstance()
        {
            if (!((Object)MonoSingleton<T>.m_Instance == (Object)null))
                return;
            MonoSingleton<T>.m_Instance = Object.FindObjectOfType<T>();
            if ((Object)MonoSingleton<T>.m_Instance == (Object)null)
            {
                GameObject target = new GameObject(typeof(T).Name);
                Object.DontDestroyOnLoad((Object)target);
                MonoSingleton<T>.m_Instance = target.AddComponent<T>();
            }
        }

        public static void DestroyInstance()
        {
            if (m_Instance != null)
            {
                DestroyImmediate(m_Instance.gameObject);
            }
        }

        public static bool hasInstance => (Object)MonoSingleton<T>.m_Instance != (Object)null;

        protected virtual void Awake()
        {
            if (!(bool)(Object)MonoSingleton<T>.m_Instance)
            {
                MonoSingleton<T>.m_Instance = (T)this;
                if (!((Object)this.transform.parent == (Object)null))
                    return;
                Object.DontDestroyOnLoad((Object)this);
            }
            else
            {
                if (!((Object)MonoSingleton<T>.m_Instance != (Object)this))
                    return;
                Debug.LogErrorFormat("Instance of {0} already exist, this destroyed!", (object)this.name);
                Object.Destroy((Object)this);
            }
        }
    }
}