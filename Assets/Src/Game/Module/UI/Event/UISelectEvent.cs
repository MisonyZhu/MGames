using System;
using UnityEngine.EventSystems;

namespace Framework
{
    public class UISelectEvent : UIBehaviour, ISelectHandler
    {
        public Action<BaseEventData> onSelect;

        public void OnSelect(BaseEventData eventData)
        {
            if (onSelect != null)
                onSelect(eventData);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onSelect = null;
        }
    }
}