using UnityEngine;

namespace Framework
{
    public static class RectTransformEx
    {
        public static void GetPivot(this RectTransform rect, out float x, out float y)
        {
            Vector2 pivot = rect.pivot;
            x = pivot.x;
            y = pivot.y;
        }

        public static void SetPivot(this RectTransform rect, float x, float y) => rect.pivot = new Vector2(x, y);

        public static void GetSizeDelta(this RectTransform rect, out float x, out float y)
        {
            Vector2 sizeDelta = rect.sizeDelta;
            x = sizeDelta.x;
            y = sizeDelta.y;
        }

        public static void SetSizeDelta(this RectTransform rect, float x, float y) =>
            rect.sizeDelta = new Vector2(x, y);

        public static float GetSizeDeltaX(this RectTransform rect) => rect.sizeDelta.x;

        public static void SetSizeDeltaX(this RectTransform rect, float x)
        {
            Vector2 sizeDelta = rect.sizeDelta with { x = x };
            rect.sizeDelta = sizeDelta;
        }

        public static float GetSizeDeltaY(this RectTransform rect) => rect.sizeDelta.y;

        public static void SetSizeDeltaY(this RectTransform rect, float y)
        {
            Vector2 sizeDelta = rect.sizeDelta with { y = y };
            rect.sizeDelta = sizeDelta;
        }

        public static void GetAnchoredPosition(this RectTransform rect, out float x, out float y)
        {
            x = rect.anchoredPosition.x;
            y = rect.anchoredPosition.y;
        }

        public static void SetAnchoredPosition(this RectTransform rect, float x, float y) =>
            rect.anchoredPosition = new Vector2(x, y);

        public static float GetAnchoredPositionX(this RectTransform rect) => rect.anchoredPosition.x;

        public static void SetAnchoredPositionX(this RectTransform rect, float x)
        {
            Vector2 anchoredPosition = rect.anchoredPosition with 
            {
                x = x
            };
            rect.anchoredPosition = anchoredPosition;
        }

        public static float GetAnchoredPositionY(this RectTransform rect) => rect.anchoredPosition.y;

        public static void SetAnchoredPositionY(this RectTransform rect, float y)
        {
            Vector2 anchoredPosition = rect.anchoredPosition with
            {
                y = y
            };
            rect.anchoredPosition = anchoredPosition;
        }

        public static void GetAnchorMin(this RectTransform rect, out float x, out float y)
        {
            Vector2 anchorMin = rect.anchorMin;
            x = anchorMin.x;
            y = anchorMin.y;
        }

        public static void SetAnchorMin(this RectTransform rect, float x, float y) =>
            rect.anchorMin = new Vector2(x, y);

        public static void GetAnchorMax(this RectTransform rect, out float x, out float y)
        {
            Vector2 anchorMax = rect.anchorMax;
            x = anchorMax.x;
            y = anchorMax.y;
        }

        public static void SetAnchorMax(this RectTransform rect, float x, float y) =>
            rect.anchorMax = new Vector2(x, y);
    }
}