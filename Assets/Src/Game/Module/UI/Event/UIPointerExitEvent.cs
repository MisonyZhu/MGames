using System;
using UnityEngine.EventSystems;

namespace Framework
{
    public class UIPointerExitEvent : UIBehaviour, IPointerExitHandler
    {
        public Action<PointerEventData> onPointerExit;

        public void OnPointerExit(PointerEventData eventData)
        {
            if (onPointerExit != null)
                onPointerExit(eventData);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onPointerExit = null;
        }
    }
}