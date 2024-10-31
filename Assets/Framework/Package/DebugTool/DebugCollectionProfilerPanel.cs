using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace Framework.Debug
{
    public class DebugCollectionProfilerPanel : DebugScrollPanel
    {
        string m_ObjectName;

        protected override void DrawContent()
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Object");
                m_ObjectName = GUILayout.TextField(m_ObjectName);
            }

            if (!string.IsNullOrEmpty(m_ObjectName))
            {
                Type type = null;
                string fieldName = null;
                string typeName = m_ObjectName;
                int start = m_ObjectName.Length - 1;
                while (true)
                {
                    type = FindType(typeName);
                    if (type != null)
                        break;

                    int index = m_ObjectName.LastIndexOf('.', start);
                    if (index == -1)
                        break;

                    typeName = m_ObjectName.Substring(0, index);
                    fieldName = m_ObjectName.Substring(index + 1);
                    start = index - 1;
                }

                if (type == null)
                    return;

                using (new GUILayout.VerticalScope("box"))
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label(string.Format("<b>Name</b>"));
                        GUILayout.Label("<b>Count</b>", GUILayout.Width(80f));
                    }

                    object obj = GetField(type, fieldName);
                    if (obj != null)
                        type = obj.GetType();

                    var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    foreach (var field in fields)
                    {
                        var value = field.GetValue(null) as ICollection;
                        if (value != null)
                        {
                            using (new GUILayout.HorizontalScope())
                            {
                                GUILayout.Label(field.Name);
                                GUILayout.Label(value.Count.ToString(), GUILayout.Width(80f));
                            }
                        }
                    }

                    if (obj != null)
                    {
                        fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        foreach (var field in fields)
                        {
                            var value = field.GetValue(obj) as ICollection;
                            if (value != null)
                            {
                                using (new GUILayout.HorizontalScope())
                                {
                                    GUILayout.Label(field.Name);
                                    GUILayout.Label(value.Count.ToString(), GUILayout.Width(80f));
                                }
                            }
                        }
                    }
                }
            }
        }

        Type FindType(string name)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var type = assembly.GetType(name);
                if (type != null)
                    return type;
            }
            return null;
        }

        object GetField(Type type, string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            string[] names = name.Split('.');
            const BindingFlags FLAGS = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var field = type.GetField(names[0], FLAGS);
            object obj = field != null ? field.GetValue(null) : null;

            for (int i = 1; obj != null && i < names.Length; ++i)
            {
                field = obj.GetType().GetField(names[i], FLAGS);
                obj = field != null ? field.GetValue(obj) : null;
            }
            return obj;
        }
    }
}