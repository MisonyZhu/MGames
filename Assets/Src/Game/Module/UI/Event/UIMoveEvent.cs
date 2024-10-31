using System;
using UnityEngine.EventSystems;

namespace Framework
{
    public class UIMoveEvent : UIBehaviour, IMoveHandler
    {
        public Action<AxisEventData> onMove;

        public void OnMove(AxisEventData eventData)
        {
            if (onMove != null)
                onMove(eventData);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onMove = null;
        }
    }
}