using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Game
{
    static partial class UnityNetMessage
    {
        static Dictionary<int, Action<INetPackage>> m_MessageReceives = new Dictionary<int, Action<INetPackage>>();

        public static void OnReceive(INetPackage package)
        {
            if (m_MessageReceives.TryGetValue(package.id, out var onReceive))
            {
                onReceive(package);
            }
        }

        public static void RegistReceive(int id, Action<INetPackage> onReceive)
        {
            m_MessageReceives.Add(id, onReceive);
        }
    }
}
