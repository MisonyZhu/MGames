
using System.Collections.Generic;

namespace Framework
{
    public static class Temp<T>
    {
        private static List<T> m_List = new List<T>();
        private static List<T> m_List2 = new List<T>();

        public static List<T> list
        {
            get
            {
                Temp<T>.m_List.Clear();
                return Temp<T>.m_List;
            }
        }

        public static List<T> list2
        {
            get
            {
                Temp<T>.m_List2.Clear();
                return Temp<T>.m_List2;
            }
        }
    }
}