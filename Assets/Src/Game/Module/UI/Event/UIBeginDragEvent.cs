using System;
using UnityEngine.EventSystems;

namespace Framework
{
    public class UIBeginDragEvent : UIBehaviour, IBeginDragHandler
    {
        public Action<PointerEventData> onBeginDrag;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (onBeginDrag != null)
                onBeginDrag(eventData);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onBeginDrag = null;
        }
    }
}