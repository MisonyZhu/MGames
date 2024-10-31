using Framework;
using System;
using System.Collections.Generic;


namespace Game
{
     class EventModule : ModuleBase<EventModule>
    {
        struct EventData
        {
            public Delegate func;
            public bool triggerOnce;
            public int priority;
        }

        struct RegistEventData
        {
            public bool regist;
            public int id;
            public Delegate func;
            public bool triggerOnce;
            public int priority;
        }

        static Dictionary<int, List<EventData>> m_Events = new Dictionary<int, List<EventData>>();
        static List<RegistEventData> m_RegistEvents = new List<RegistEventData>();
        static int m_DispatchCount = 0;

        public override int Priority => ModulePriority.Event_Priority;

        public static void RegistEvent<T>(int id, Action<T> func, bool triggerOnce = false, int priority = 0) where T : IEventArgs
        {
            RegistEventImpl(id, func, triggerOnce, priority);
        }

        public static void RegistEvent(int id, Action func, bool triggerOnce = false, int priority = 0)
        {
            RegistEventImpl(id, func, triggerOnce, priority);
        }

        static void RegistEventImpl(int id, Delegate func, bool triggerOnce, int priority)
        {
            if (isDispatching)
            {
                m_RegistEvents.Add(new RegistEventData
                {
                    regist = true,
                    id = id,
                    func = func,
                    triggerOnce = triggerOnce,
                    priority = priority,
                });
                return;
            }

            if (!m_Events.TryGetValue(id, out List<EventData> events))
            {
                events = new List<EventData>(1);
                m_Events.Add(id, events);
            }

            int index = events.Count;
            for (int i = 0; i < events.Count; ++i)
            {
                if (events[i].priority > priority)
                {
                    index = i;
                    break;
                }
            }

            events.Insert(index, new EventData
            {
                func = func,
                triggerOnce = triggerOnce,
                priority = priority,
            });
        }

        public static void UnRegistEvent<T>(int id, Action<T> func) where T : IEventArgs
        {
            UnRegistEventImpl(id, func);
        }

        public static void UnRegistEvent(int id, Action func)
        {
            UnRegistEventImpl(id, func);
        }

        static void UnRegistEventImpl(int id, Delegate func)
        {
            if (isDispatching)
            {
                m_RegistEvents.Add(new RegistEventData
                {
                    regist = false,
                    id = id,
                    func = func,
                });
                return;
            }

            if (!m_Events.TryGetValue(id, out List<EventData> events))
            {
                return;
            }

            for (int i = 0; i < events.Count; ++i)
            {
                if (events[i].func == func)
                {
                    events.RemoveAt(i);
                }
            }
        }

        public static void Dispatch<T>(T args) where T : IEventArgs
        {
            if (m_Events.TryGetValue(args.id, out List<EventData> events) && events.Count > 0)
            {
                BeginDispatch();
                int count = events.Count;
                for (int i = 0; i < count;)
                {
                    var func = events[i].func;
                    if (func is Action<T> actionT)
                        actionT(args);
                    else if (func is Action action)
                        action();

                    if (events[i].triggerOnce)
                    {
                        events.RemoveAt(i);
                        --count;
                    }
                    else
                        ++i;
                }
                EndDispatch();
            }
        }

        public static void Dispatch(int id)
        {
            Dispatch(new EventArgs(id));
        }

        static bool isDispatching => m_DispatchCount > 0;


        static void BeginDispatch()
        {
            ++m_DispatchCount;
        }

        static void EndDispatch()
        {
            if (--m_DispatchCount > 0)
                return;

            if (m_RegistEvents.Count > 0)
            {
                for (int i = 0; i < m_RegistEvents.Count; ++i)
                {
                    var data = m_RegistEvents[i];
                    if (data.regist)
                    {
                        RegistEventImpl(data.id, data.func, data.triggerOnce, data.priority);
                    }
                    else
                    {
                        UnRegistEventImpl(data.id, data.func);
                    }
                }
                m_RegistEvents.Clear();
            }
        }

        public override void OnUpdate(float detlaTime)
        {
            
        }

        public override void OnShutDown()
        {
           
        }

        public class Container
        {
            struct EventData
            {
                public int id;
                public Delegate func;

                public EventData(int id, Delegate func)
                {
                    this.id = id;
                    this.func = func;
                }
            }
            List<EventData> m_Events = new List<EventData>();

            public void RegistEvent<T>(int id, Action<T> func, bool triggerOnce = true, int priority = 0) where T : IEventArgs
            {
                RegistEventImpl(id, func, triggerOnce, priority);
            }

            public void RegistEvent(int id, Action func, bool triggerOnce = true, int priority = 0)
            {
                RegistEventImpl(id, func, triggerOnce, priority);
            }

            void RegistEventImpl(int id, Delegate func, bool triggerOnce, int priority)
            {
                var data = new EventData(id, func);
                if (FindEvent(id, func) != -1)
                {
                    LogModule.LogError($"Event:{id} Func:{func} is already registed");
                    return;
                }

                EventModule.RegistEventImpl(id, func, triggerOnce, priority);
                m_Events.Add(data);
            }

            public void UnRegistEvent<T>(int id, Action<T> func) where T : IEventArgs
            {
                UnRegistEventImpl(id, func);
            }

            public void UnRegistEvent(int id, Action func)
            {
                UnRegistEventImpl(id, func);
            }

            void UnRegistEventImpl(int id, Delegate func)
            {
                var data = new EventData(id, func);
                int index = FindEvent(id, func);
                if (index == -1)
                {
                    LogModule.LogError($"Event:{id} Func:{func} is not registed");
                    return;
                }

                m_Events.RemoveAt(index);
                EventModule.UnRegistEventImpl(id, func);
            }

            public void UnRegistAllEvents()
            {
                if (m_Events.Count > 0)
                {
                    foreach (var data in m_Events)
                    {
                        EventModule.UnRegistEventImpl(data.id, data.func);
                    }
                    m_Events.Clear();
                }
            }

            int FindEvent(int id, Delegate func)
            {
                for (int i = 0; i < m_Events.Count; ++i)
                {
                    var e = m_Events[i];
                    if (e.id == id && e.func == func)
                    {
                        return i;
                    }
                }
                return -1;
            }
        }
    }
}
