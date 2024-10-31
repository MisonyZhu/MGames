using System;
using UnityEngine.EventSystems;

namespace Framework
{
    public class UIScrollEvent : UIBehaviour, IScrollHandler
    {
        public Action<PointerEventData> onScroll;

        public void OnScroll(PointerEventData eventData)
        {
            if (onScroll != null)
                onScroll(eventData);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onScroll = null;
        }
    }
}