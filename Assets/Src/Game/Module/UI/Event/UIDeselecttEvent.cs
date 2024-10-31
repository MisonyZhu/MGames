using System;
using UnityEngine.EventSystems;

namespace Framework
{
    public class UIDeselectEvent : UIBehaviour, IDeselectHandler
    {
        public Action<BaseEventData> onDeselect;

        public void OnDeselect(BaseEventData eventData)
        {
            if (onDeselect != null)
                onDeselect(eventData);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onDeselect = null;
        }
    }
}