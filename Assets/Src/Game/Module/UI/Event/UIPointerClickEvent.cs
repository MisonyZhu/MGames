using System;
using UnityEngine.EventSystems;

namespace Framework
{
    public class UIPointerClickEvent : UIBehaviour, IPointerClickHandler
    {
        public Action<PointerEventData> onPointerClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (onPointerClick != null)
                onPointerClick(eventData);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onPointerClick = null;
        }
    }
}