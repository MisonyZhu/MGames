using System;
using UnityEngine.EventSystems;

namespace Framework
{
    public class UIEndDragEvent : UIBehaviour, IEndDragHandler
    {
        public Action<PointerEventData> onEndDrag;

        public void OnEndDrag(PointerEventData eventData)
        {
            if (onEndDrag != null)
                onEndDrag(eventData);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onEndDrag = null;
        }
    }
}