using System;
using UnityEngine;

namespace Framework.Debug
{
    public class DraggableScrollView
    {
        GUILayoutOption[] m_Options;
        Vector2 m_ScrollPosition;
        bool m_Dragging;
        Rect m_DragArea;

        public DraggableScrollView(params GUILayoutOption[] options)
        {
            m_Options = options;
        }

        public void Begin()
        {
            var e = Event.current;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (!m_Dragging && m_DragArea.Contains(e.mousePosition))
                    {
                        m_Dragging = true;
                    }
                    break;
                case EventType.MouseUp:
                    {
                        m_Dragging = false;
                    }
                    break;
                case EventType.MouseDrag:
                    if (m_Dragging)
                    {
                        m_ScrollPosition += e.delta;
                    }
                    break;
            }

            m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition, m_Options);
        }

        public void End()
        {
            GUILayout.EndScrollView();
            
            var e = Event.current;
            switch (e.type)
            {
                case EventType.Repaint:
                    m_DragArea = GUILayoutUtility.GetLastRect();
                    break;
            }
        }

        public ScopeStruct Scope()
        {
            return new ScopeStruct(this);
        }

        public struct ScopeStruct : IDisposable
        {
            DraggableScrollView m_DragScrollView;

            public ScopeStruct(DraggableScrollView dragScrollView)
            {
                m_DragScrollView = dragScrollView;
                m_DragScrollView.Begin();
            }

            public void Dispose()
            {
                m_DragScrollView.End();
            }
        }
    }
}