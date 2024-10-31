namespace Framework
{
    using UnityEngine;
    public static class ObjectEx
    {
        public static void Destroy(Object obj)
        {
            if (Application.isPlaying)
                Object.Destroy(obj);
            else
                Object.DestroyImmediate(obj);
        }

        public static bool IsNull(Object obj) => obj == (Object) null;
    }
}
