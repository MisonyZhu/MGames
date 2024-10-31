using System;
using UnityEngine.Events;

namespace Framework
{
    public static class UnityEventEx
    {
        public static void SetListener(this UnityEvent unityEvent, Action call)
        {
            unityEvent.RemoveAllListeners();
            if (call == null)
                return;
            unityEvent.AddListener((UnityAction)(() => call()));
        }
    }
}