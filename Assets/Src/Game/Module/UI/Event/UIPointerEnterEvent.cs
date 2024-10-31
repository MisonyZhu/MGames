using System;
using UnityEngine.EventSystems;

namespace Framework
{
    public class UIPointerEnterEvent : UIBehaviour, IPointerEnterHandler
    {
        public Action<PointerEventData> onPointerEnter;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (onPointerEnter != null)
                onPointerEnter(eventData);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onPointerEnter = null;
        }
    }
}