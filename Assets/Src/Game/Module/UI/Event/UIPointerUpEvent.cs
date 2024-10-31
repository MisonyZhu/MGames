using System;
using UnityEngine.EventSystems;

namespace Framework
{
    public class UIPointerUpEvent : UIBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        public Action<PointerEventData> onPointerUp;

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (onPointerUp != null)
                onPointerUp(eventData);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onPointerUp = null;
        }
    }
}