using System;
using UnityEngine.EventSystems;

namespace Framework
{
    public class UIUpdateSelectedEvent : UIBehaviour, IUpdateSelectedHandler
    {
        public Action<BaseEventData> onUpdateSelected;

        public void OnUpdateSelected(BaseEventData eventData)
        {
            if (onUpdateSelected != null)
                onUpdateSelected(eventData);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onUpdateSelected = null;
        }
    }
}