
using UnityEngine;

namespace  Framework
{
    public class ColorEx
    {
        public static int ColorToInt(byte r, byte g, byte b, byte a = 255) => ((int) r << 24) + ((int) g << 16) + ((int) b << 8) + (int) a;

        public static int ColorToInt(Color32 color) => ColorEx.ColorToInt(color.r, color.g, color.b, color.a);

        public static int ColorToInt(Color color) => ColorEx.ColorToInt((Color32) color);

        public static Color IntToColor(int rgba) => (Color) ColorEx.IntToColor32(rgba);

        public static Color32 IntToColor32(int rgba) => new Color32((byte) (rgba >> 24), (byte) (rgba >> 16), (byte) (rgba >> 8), (byte) rgba);
    }
}
