using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Framework.Debug
{
    public class DebugHierarchyPanel : DebugScrollPanel
    {
        HashSet<GameObject> m_Expends = new HashSet<GameObject>();
        HashSet<GameObject> m_Inspectors = new HashSet<GameObject>();
        Scene m_DontDestroyOnLoadScene;

        public DebugHierarchyPanel()
        {
            GameObject temp = new GameObject("Temp");
            Object.DontDestroyOnLoad(temp);
            m_DontDestroyOnLoadScene = temp.scene;
            Object.Destroy(temp);
        }

        protected override void DrawContent()
        {
            for (int i = 0; i < SceneManager.sceneCount; ++i)
            {
                DrawScene(SceneManager.GetSceneAt(i));
            }

            DrawScene(m_DontDestroyOnLoadScene);
        }

        void DrawScene(Scene scene)
        {
            GUILayout.Label(string.Format("<b>Scene {0}</b>", scene.name));
            GUILayout.BeginVertical();
            {
                foreach (var item in scene.GetRootGameObjects())
                {
                    DrawItem(item, 0);
                }
            }
            GUILayout.EndVertical();
        }

        void DrawItem(GameObject go, int deep)
        {
            bool expend = m_Expends.Contains(go);
            bool inspector = m_Inspectors.Contains(go);
            GUILayout.BeginHorizontal("box");
            {
                GUILayout.Space(20 * deep);
                if (go.transform.childCount > 0)
                {
                    if (GUILayout.Button(expend ? "-" : "+", GUILayout.Width(30)))
                    {
                        if (expend)
                            m_Expends.Remove(go);
                        else
                            m_Expends.Add(go);
                    }
                }
                else
                {
                    var rect = GUILayoutUtility.GetRect(new GUIContent("+"), GUI.skin.button, GUILayout.Width(30));
                    GUILayout.Space(rect.width);
                }

                bool realActive = go.activeInHierarchy;
                bool active = go.activeSelf;

                GUI.color = realActive ? Color.white : Color.gray;
                GUILayout.Label(go.name, GUILayout.ExpandWidth(true));
                GUI.color = Color.white;

                if (GUILayout.Button(active ? "Deactive" : "Active", GUILayout.Width(100)))
                {
                    go.SetActive(!active);
                }

                if (GUILayout.Button(inspector ? "HideDetail" : "ShowDetail", GUILayout.Width(100)))
                {
                    inspector = !inspector;
                    if (inspector)
                        m_Inspectors.Add(go);
                    else
                        m_Inspectors.Remove(go);
                }
            }
            GUILayout.EndHorizontal();

            if (inspector)
            {
                using (new GUILayout.HorizontalScope())
                {
                    using (new GUILayout.VerticalScope())
                    {
                        DrawObject(go);

                        var components = go.GetComponents<Component>();
                        foreach (var component in components)
                        {
                            DrawObject(component);
                        }
                    }
                }
            }

            if (expend)
            {
                foreach (Transform child in go.transform)
                {
                    DrawItem(child.gameObject, deep + 1);
                }
            }
        }

        static string[] IGNORE_PROPERTICES =
        {
            "useGUILayout",
            "runInEditMode",
            "allowPrefabModeInPlayMode",
            "transform",
            "gameObject",
            "tag",
            "name",
            "hideFlags",
            "enabled",
            "renderingDisplaySize", // Canvas上获取这个值会崩溃
        };

        void DrawObject(Object obj)
        {
            var type = obj.GetType();
            using (new GUILayout.VerticalScope(type.Name, "box"))
            {
                if (obj is GameObject go)
                {
                    DrawGameObject(go);
                    return;
                }

                if (obj is RectTransform rect)
                {
                    DrawRectTransform(rect);
                    return;
                }

                if (obj is Transform transform)
                {
                    DrawTransform(transform);
                    return;
                }

                DrawCommonComponent(obj as Component);
            }
        }

        void DrawGameObject(GameObject go)
        {
            DrawItem("Name", go.name);
            DrawItem("Tag", go.tag);
            DrawItem("Layer", LayerMask.LayerToName(go.layer));
        }

        void DrawRectTransform(RectTransform rect)
        {
            DrawItem("Position", rect.anchoredPosition3D.ToString("G"));
            DrawItem("Size", rect.sizeDelta.ToString("G"));
            DrawItem("AnchorMin", rect.anchorMin.ToString("G"));
            DrawItem("AnchorMax", rect.anchorMax.ToString("G"));
            DrawItem("Rotation", rect.localEulerAngles.ToString("G"));
            DrawItem("Scale", rect.localScale.ToString("G"));
        }

        void DrawTransform(Transform transform)
        {
            DrawItem("Position", transform.localPosition.ToString("G"));
            DrawItem("Rotation", transform.localEulerAngles.ToString("G"));
            DrawItem("Scale", transform.localScale.ToString("G"));
        }

        void DrawCommonComponent(Component component)
        {
            if (!component) return;

            var type = component.GetType();
            var enabledProperty = type.GetProperty("enabled", BindingFlags.Public | BindingFlags.Instance);
            if (enabledProperty != null)
            {
                bool enabled = (bool)enabledProperty.GetValue(component);
                if (DrawToggle("Enabled", enabled) != enabled)
                {
                    enabledProperty.SetValue(component, !enabled);
                }
            }

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                object value = field.GetValue(component);
                DrawItem(field.Name, value != null ? value.ToString() : "null");
            }

            var propertices = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var property in propertices)
            {
                if (property.GetCustomAttribute<ObsoleteAttribute>() != null)
                    continue;

                if (IGNORE_PROPERTICES.Contains(property.Name))
                    continue;

                if (property.GetGetMethod() == null)
                    continue;

                try
                {
                    object value = property.GetValue(component);
                    DrawItem(property.Name, value != null ? value.ToString() : "null");
                }
                catch
                { }
            }
        }
    }
}
