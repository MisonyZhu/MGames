using System;
using UnityEngine.EventSystems;

namespace Framework
{
    public class UIPointerEvents : UIBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
                                              , IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Action<PointerEventData> onPointerClick;
        public Action<PointerEventData> onPointerDown;
        public Action<PointerEventData> onPointerEnter;
        public Action<PointerEventData> onPointerExit;
        public Action<PointerEventData> onPointerUp;

        public Action<PointerEventData> onBeginDrag;
        public Action<PointerEventData> onDraging;
        public Action<PointerEventData> onEndDrag;



        public void OnPointerClick(PointerEventData eventData)
        {
            onPointerClick?.Invoke(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            onPointerDown?.Invoke(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            onPointerEnter?.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onPointerExit?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            onPointerUp?.Invoke(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            onBeginDrag?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            onDraging?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            onEndDrag?.Invoke(eventData);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onPointerClick = null;
            onPointerDown = null;
            onPointerEnter = null;
            onPointerExit = null;
            onPointerUp = null;
            onBeginDrag = null;
            onDraging = null;
            onEndDrag = null;
        }
    }
}