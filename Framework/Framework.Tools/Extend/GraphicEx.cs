using UnityEngine;
using UnityEngine.UI;

namespace Framework
{

    public static class GraphicEx
    {
        public static void SetColor(this Graphic graphic, int color)
        {
            graphic.color = ColorEx.IntToColor(color);
        }

        public static void SetAlpha(this Graphic graphic, float alpha)
        {
            Color color = graphic.color with { a = alpha };
            graphic.color = color;
        }
    }
}
