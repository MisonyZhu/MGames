using System;
using UnityEngine.EventSystems;

namespace Framework
{
    public class UIInitializePotentialDragEvent : UIBehaviour, IInitializePotentialDragHandler
    {
        public Action<PointerEventData> onInitializePotentialDrag;

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            if (onInitializePotentialDrag != null)
                onInitializePotentialDrag(eventData);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onInitializePotentialDrag = null;
        }
    }
}