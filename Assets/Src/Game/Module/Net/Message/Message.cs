using System;
using System.Collections.Generic;
using Framework;

namespace Game
{
    interface IMessageID
    {
        int id { get; }
    }

    class Message<TMessageID> where TMessageID : IMessageID
    {
        struct EventData
        {
            public Action func;
            public int priority;
        }

        struct RegistEventData
        {
            public bool regist;
            public Action func;
            public int priority;
        }

        static TMessageID m_MessageID = default(TMessageID);
        static int id => m_MessageID.id;

        static List<EventData> m_Events;
        static List<RegistEventData> m_RegistEvents;
        static int m_DispatchCount = 0;
        static bool isDispatching => m_DispatchCount > 0;

        static Message()
        {
            UnityNetMessage.RegistReceive(id, OnReceive);
        }

        static void OnReceive(INetPackage package)
        {
            Dispatch(package.id);
        }

        public static void Send()
        {
            // NetManager.Send(id);
        }

        public static void RegistEvent(Action func, int priority)
        {
            if (isDispatching)
            {
                if (m_RegistEvents == null)
                {
                    m_RegistEvents = new List<RegistEventData>();
                }

                m_RegistEvents.Add(new RegistEventData
                {
                    regist = true,
                    func = func,
                    priority = priority,
                });
                return;
            }

            if (m_Events == null)
            {
                m_Events = new List<EventData>();
            }

            int index = m_Events.Count;
            for (int i = 0; i < m_Events.Count; ++i)
            {
                if (m_Events[i].priority > priority)
                {
                    index = i;
                    break;
                }
            }

            m_Events.Insert(index, new EventData
            {
                func = func,
                priority = priority,
            });
        }

        public static void UnRegistEvent(Action func)
        {
            if (isDispatching)
            {
                if (m_RegistEvents == null)
                {
                    m_RegistEvents = new List<RegistEventData>();
                }

                m_RegistEvents.Add(new RegistEventData
                {
                    regist = false,
                    func = func,
                });
                return;
            }

            if (m_Events == null) return;

            for (int i = 0; i < m_Events.Count; ++i)
            {
                if (m_Events[i].func == func)
                {
                    m_Events.RemoveAt(i);
                }
            }
        }

        static void Dispatch(int id)
        {
            BeginDispatch();
            if (m_Events != null)
            {
                for (int i = 0; i < m_Events.Count; ++i)
                {
                    var func = m_Events[i].func;
                    func();
                }
            }
            EndDispatch();
        }

        static void BeginDispatch()
        {
            ++m_DispatchCount;
        }

        static void EndDispatch()
        {
            if (--m_DispatchCount > 0)
                return;

            if (m_RegistEvents != null && m_RegistEvents.Count > 0)
            {
                for (int i = 0; i < m_RegistEvents.Count; ++i)
                {
                    var data = m_RegistEvents[i];
                    if (data.regist)
                    {
                        RegistEvent(data.func, data.priority);
                    }
                    else
                    {
                        UnRegistEvent(data.func);
                    }
                }
                m_RegistEvents.Clear();
            }
        }
    }

//     class Message<TMessageID, TMessage> where TMessageID : IMessageID where TMessage : Google.Protobuf.IMessage, new()
//     {
//         public string message = "ttt";
//         struct EventData
//         {
//             public Action<TMessage> func;
//             public int priority;
//         }
//
//         struct RegistEventData
//         {
//             public bool regist;
//             public Action<TMessage> func;
//             public int priority;
//         }
//
//         static TMessageID m_MessageID = default(TMessageID);
//         public static int id => m_MessageID.id;
//         public static TMessage instance
//         {
//             get
//             {
//                 var message = Singleton<TMessage>.instance;               
//                 message.Clear();
//                 return message;
//             }
//         }
//
//         static List<EventData> m_Events;
//         static List<RegistEventData> m_RegistEvents;
//         static int m_DispatchCount = 0;
//         static bool isDispatching => m_DispatchCount > 0;
//         static Message()
//         {
//             MessageManager.RegistReceive(id, OnReceive);
//         }
//
//         static void OnReceive(INetPackage package)
//         {
//             var message = instance;
//             Protobuf.Deserialize(message, package.body, package.bodyLength);
// #if UNITY_EDITOR
//             //if (NetManager.logEnabled)
//             //    UnityEngine.Debug.Log($"[NET][RECV]:<color=#23FF00>{message.ToString()}</color> ID:{id}  {JsonParser.Default.ToJson(message)}");
// #endif
//
//             Dispatch(package.id, message);
//         }
//
//         public static void Send(TMessage msg)
//         {
//
// #if UNITY_EDITOR
//             //if (NetManager.logEnabled)
//             //    UnityEngine.Debug.Log($"[NET][SEND]:<color=#FFFF00>{msg.ToString()}</color> ID:{id}  {JsonParser.Default.ToJson(msg)}");
// #endif
//             NetManager.Send(id, msg);
//         }
//
//         public static void RegistEvent(Action<TMessage> func, int priority = 0)
//         {
//             if (isDispatching)
//             {
//                 if (m_RegistEvents == null)
//                 {
//                     m_RegistEvents = new List<RegistEventData>();
//                 }
//
//                 m_RegistEvents.Add(new RegistEventData
//                 {
//                     regist = true,
//                     func = func,
//                     priority = priority,
//                 });
//                 return;
//             }
//
//             if (m_Events == null)
//             {
//                 m_Events = new List<EventData>();
//             }
//
//             int index = m_Events.Count;
//             for (int i = 0; i < m_Events.Count; ++i)
//             {
//                 if (m_Events[i].priority > priority)
//                 {
//                     index = i;
//                     break;
//                 }
//             }
//
//             m_Events.Insert(index, new EventData
//             {
//                 func = func,
//                 priority = priority,
//             });
//         }
//
//         public static void UnRegistEvent(Action<TMessage> func)
//         {
//             if (isDispatching)
//             {
//                 if (m_RegistEvents == null)
//                 {
//                     m_RegistEvents = new List<RegistEventData>();
//                 }
//
//                 m_RegistEvents.Add(new RegistEventData
//                 {
//                     regist = false,
//                     func = func,
//                 });
//                 return;
//             }
//
//             if (m_Events == null) return;
//
//             for (int i = 0; i < m_Events.Count; ++i)
//             {
//                 if (m_Events[i].func == func)
//                 {
//                     m_Events.RemoveAt(i);
//                 }
//             }
//         }
//
//         static void Dispatch(int id, TMessage message)
//         {
//             BeginDispatch();
//             if (m_Events != null)
//             {
//                 for (int i = 0; i < m_Events.Count; ++i)
//                 {
//                     var func = m_Events[i].func;
//                     func(message);
//                 }
//             }
//             EndDispatch();
//         }
//
//         static void BeginDispatch()
//         {
//             ++m_DispatchCount;
//         }
//
//         static void EndDispatch()
//         {
//             if (--m_DispatchCount > 0)
//                 return;
//
//             if (m_RegistEvents != null && m_RegistEvents.Count > 0)
//             {
//                 for (int i = 0; i < m_RegistEvents.Count; ++i)
//                 {
//                     var data = m_RegistEvents[i];
//                     if (data.regist)
//                     {
//                         RegistEvent(data.func, data.priority);
//                     }
//                     else
//                     {
//                         UnRegistEvent(data.func);
//                     }
//                 }
//                 m_RegistEvents.Clear();
//             }
//         }
//     }
}
