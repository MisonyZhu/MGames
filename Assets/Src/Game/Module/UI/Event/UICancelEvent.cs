using System;
using UnityEngine.EventSystems;

namespace Framework
{
    public class UICancelEvent : UIBehaviour, ICancelHandler
    {
        public Action<BaseEventData> onCancel;

        public void OnCancel(BaseEventData eventData)
        {
            if (onCancel != null)
                onCancel(eventData);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onCancel = null;
        }
    }
}