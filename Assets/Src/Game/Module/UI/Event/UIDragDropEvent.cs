using System;
using UnityEngine.EventSystems;

namespace Framework
{
    public class UIDragDropEvents : UIBehaviour, IBeginDragHandler, IDragHandler, IDropHandler
    {
        public Action<PointerEventData> onBeginDrag;
        public Action<PointerEventData> onDrag;
        public Action<PointerEventData> onDrop;
        public Action<PointerEventData> onEndDrag;

        public void OnBeginDrag(PointerEventData eventData)
        {
            onBeginDrag?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            onDrag?.Invoke(eventData);
        }

        public void OnDrop(PointerEventData eventData)
        {
            onDrop?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            onEndDrag?.Invoke(eventData);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onBeginDrag = null;
            onDrag = null;
            onDrop = null;
            onEndDrag = null;
        }
    }
}