using System;
using UnityEngine.EventSystems;

namespace Framework
{
    public class UIPointerDownEvent : UIBehaviour, IPointerDownHandler
    {
        public Action<PointerEventData> onPointerDown;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (onPointerDown != null)
                onPointerDown(eventData);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onPointerDown = null;
        }
    }
}