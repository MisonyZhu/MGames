using System;
using UnityEngine.EventSystems;

namespace Framework
{
    public class UIDropEvent : UIBehaviour, IDropHandler
    {
        public Action<PointerEventData> onDrop;

        public void OnDrop(PointerEventData eventData)
        {
            if (onDrop != null)
                onDrop(eventData);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onDrop = null;
        }
    }
}