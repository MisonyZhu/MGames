using System;
using UnityEngine.EventSystems;

namespace Framework
{
    public class UIDragEvent : UIBehaviour, IDragHandler
    {
        public Action<PointerEventData> onDrag;

        public void OnDrag(PointerEventData eventData)
        {
            if (onDrag != null)
                onDrag(eventData);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onDrag = null;
        }
    }
}