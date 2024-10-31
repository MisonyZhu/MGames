using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Game
{
    class UIModule : ModuleBase<UIModule>
    {
        public static Transform root { get; private set; }

        public override int Priority => ModulePriority.UI_Priority;

        static UIModule()
        {
            root = GameObject.Find("/Main/Canvas").transform;
        }

        enum UIOperationType
        {
            Add,
            Remove,
        }

        struct UIOperation
        {
            public UIOperationType type;
            public BaseUI ui;

            public UIOperation(UIOperationType type, BaseUI ui)
            {
                this.type = type;
                this.ui = ui;
            }
        }

        static int m_UpdateCount = 0;
        static bool updating => m_UpdateCount > 0;
        static List<UIOperation> m_Operations = new List<UIOperation>();
        static List<BaseUI> m_SortedUIs = new List<BaseUI>();

        public static void AddUI(BaseUI ui)
        {
            if (updating)
            {
                m_Operations.Add(new UIOperation(UIOperationType.Add, ui));
                return;
            }

            int i;
            for (i = 0; i < m_SortedUIs.Count; ++i)
            {
                if (ui.sortingOrder >= m_SortedUIs[i].sortingOrder)
                {
                    break;
                }
            }

            m_SortedUIs.Insert(i, ui);
        }

        public static void RemoveUI(BaseUI ui)
        {
            if (updating)
            {
                m_Operations.Add(new UIOperation(UIOperationType.Remove, ui));
                return;
            }

            m_SortedUIs.Remove(ui);
        }

        struct UpdatingScope : IDisposable
        {
            public UpdatingScope(int _)
            {
                BeginUpdate();
            }

            public void Dispose()
            {
                EndUpdate();
            }

            static void BeginUpdate()
            {
                ++m_UpdateCount;
            }

            static void EndUpdate()
            {
                if (--m_UpdateCount == 0)
                {
                    for (int i = 0; i < m_Operations.Count; ++i)
                    {
                        var operation = m_Operations[i];
                        switch (operation.type)
                        {
                            case UIOperationType.Add:
                                AddUI(operation.ui);
                                break;
                            case UIOperationType.Remove:
                                RemoveUI(operation.ui);
                                break;
                        }
                    }
                    m_Operations.Clear();
                }
            }
        }
        
        public override void OnUpdate(float detlaTime)
        {
            Update();
        }


        public static void Update()
        {
            using (new UpdatingScope(0))
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    OnEscClose();
                }

                OnKeyInput();

                for (int i = 0; i < m_SortedUIs.Count; ++i)
                {
                    var ui = m_SortedUIs[i];
                    if (ui.visible && ui.loaded)
                    {
                        ui.Update();
                    }
                }
            }
        }

        public static BaseUI escUI { get; set; }

        static void OnEscClose()
        {
            for (int i = 0; i < m_SortedUIs.Count; ++i)
            {
                var ui = m_SortedUIs[i];
                if (ui.visible && ui.OnEscClose())
                {
                    return;
                }
            }

            escUI?.Show();
        }

        static void OnKeyInput()
        {
            for (int i = 0; i < m_SortedUIs.Count; ++i)
            {
                var ui = m_SortedUIs[i];
                if (ui.visible && ui.loaded && ui.OnKeyInput())
                {
                    return;
                }
            }
        }

        public static void LateUpdate()
        {
            using (new UpdatingScope(0))
            {
                for (int i = 0; i < m_SortedUIs.Count; ++i)
                {
                    var ui = m_SortedUIs[i];
                    if (ui.visible && ui.loaded)
                    {
                        ui.LateUpdate();
                    }
                }
            }
        }

        public static void HideLayer(UILayer layer, int orderInLayer, bool playAnimation = true)
        {
            using (new UpdatingScope(0))
            {
                for (int i = 0; i < m_SortedUIs.Count; ++i)
                {
                    var ui = m_SortedUIs[i];
                    if (ui.layer == layer && ui.orderInLayer == orderInLayer)
                    {
                        ui.Hide(playAnimation);
                    }
                }
            }
        }

        public static void HideLayer(UILayer layer, bool playAnimation = true)
        {
            using (new UpdatingScope(0))
            {
                for (int i = 0; i < m_SortedUIs.Count; ++i)
                {
                    var ui = m_SortedUIs[i];
                    if (ui.layer == layer)
                    {
                        ui.Hide(playAnimation);
                    }
                }
            }
        }


        public override void OnShutDown()
        {
            throw new NotImplementedException();
        }
    }
}