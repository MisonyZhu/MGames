using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public static class GameObjectEx
    {
        public static Component AddMissingComponent(this GameObject go, System.Type t)
        {
            Component component = go.GetComponent(t);
            if ((UnityEngine.Object)component == (UnityEngine.Object)null)
                component = go.AddComponent(t);
            return component;
        }

        public static T AddMissingComponent<T>(this GameObject gameObject) where T : Component
        {
            T obj = gameObject.GetComponent<T>();
            if ((UnityEngine.Object)obj == (UnityEngine.Object)null)
                obj = gameObject.AddComponent<T>();
            return obj;
        }

        public static GameObject Clone(this GameObject gameObject) =>
            UnityEngine.Object.Instantiate<GameObject>(gameObject, gameObject.transform.parent, false);

        public static void EnableComponents(this GameObject go, System.Type t, bool enabled)
        {
            List<Component> list = Temp<Component>.list;
            go.GetComponents(t, list);
            for (int index = 0; index < list.Count; ++index)
            {
                MonoBehaviour monoBehaviour = list[index] as MonoBehaviour;
                if ((bool)(UnityEngine.Object)monoBehaviour)
                    monoBehaviour.enabled = enabled;
            }
        }

        public static void EnableComponents<T>(this GameObject go, bool enabled) where T : Component =>
            go.EnableComponents(typeof(T), enabled);

        public static Material GetMaterial(this GameObject gameObject, int index = 0)
        {
            Renderer component = gameObject.GetComponent<Renderer>();
            if (!(bool)(UnityEngine.Object)component)
                return (Material)null;
            return index == 0
                ? component.material
                : (index < component.materials.Length ? component.materials[index] : (Material)null);
        }

        public static Material GetSharedMaterial(this GameObject gameObject, int index = 0)
        {
            Renderer component = gameObject.GetComponent<Renderer>();
            if (!(bool)(UnityEngine.Object)component)
                return (Material)null;
            return index == 0
                ? component.sharedMaterial
                : (index < component.sharedMaterials.Length ? component.materials[index] : (Material)null);
        }

        public static Component GetRootComponent(this GameObject gameObject, System.Type t)
        {
            Component rootComponent = (Component)null;
            for (Transform transform = gameObject.transform;
                 (UnityEngine.Object)transform != (UnityEngine.Object)null;
                 transform = transform.parent)
            {
                Component component = transform.GetComponent(t);
                if ((bool)(UnityEngine.Object)component)
                    rootComponent = component;
            }

            return rootComponent;
        }

        public static T GetRootComponent<T>(this GameObject gameObject) where T : UnityEngine.Object
        {
            T rootComponent = default(T);
            for (Transform transform = gameObject.transform;
                 (UnityEngine.Object)transform != (UnityEngine.Object)null;
                 transform = transform.parent)
            {
                T component = transform.GetComponent<T>();
                if ((bool)(UnityEngine.Object)component)
                    rootComponent = component;
            }

            return rootComponent;
        }

        public static bool HaveComponent(this GameObject go, System.Type t) =>
            (UnityEngine.Object)go.GetComponent(t) != (UnityEngine.Object)null;

        public static bool HaveComponent<T>(this GameObject go) =>
            (UnityEngine.Object)go.GetComponent(typeof(T)) != (UnityEngine.Object)null;

        public static bool RemoveComponent(this GameObject go, System.Type t)
        {
            Component component = go.GetComponent(t);
            if (!(bool)(UnityEngine.Object)component)
                return false;
            ObjectEx.Destroy((UnityEngine.Object)component);
            return true;
        }

        public static bool RemoveComponent<T>(this GameObject go) where T : Component => go.RemoveComponent(typeof(T));

        public static void RemoveComponents(this GameObject go, System.Type t)
        {
            List<Component> list = Temp<Component>.list;
            go.GetComponents(t, list);
            for (int index = 0; index < list.Count; ++index)
                ObjectEx.Destroy((UnityEngine.Object)list[index]);
        }

        public static void RemoveComponents<T>(this GameObject go) where T : Component =>
            go.RemoveComponents(typeof(T));

        public static void SetLayer(this GameObject go, int layer)
        {
            go.layer = layer;
            Transform transform = go.transform;
            int index = 0;
            for (int childCount = transform.childCount; index < childCount; ++index)
                transform.GetChild(index).gameObject.SetLayer(layer);
        }
    }
}