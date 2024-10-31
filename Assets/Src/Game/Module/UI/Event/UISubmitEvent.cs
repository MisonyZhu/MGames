using System;
using UnityEngine.EventSystems;

namespace Framework
{
    public class UISubmitEvent : UIBehaviour, ISubmitHandler
    {
        public Action<BaseEventData> onSubmit;

        public void OnSubmit(BaseEventData eventData)
        {
            if (onSubmit != null)
                onSubmit(eventData);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onSubmit = null;
        }
    }
}