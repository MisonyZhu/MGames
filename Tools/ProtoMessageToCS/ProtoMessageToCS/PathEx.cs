namespace ProtoMessageToCS
{
    class PathEx
    {
        static readonly char[] PATH_SLASH = "/\\".ToCharArray();

        public static string RemoveBack(string path, int time = 1)
        {
            int index = path.Length - 1;
            for (int i = 0; i < time; ++i)
            {
                int index2 = path.LastIndexOfAny(PATH_SLASH, index);
                if (index2 == -1)
                    return string.Empty;
                else if (index2 == index)
                    --i;

                index = index2 - 1;
            }
            return path.Substring(0, index + 1);
        }

        public static string Combine(string dir, string path)
        {
            if (string.IsNullOrEmpty(dir))
                return path;

            if (dir[dir.Length - 1] == '/')
                return dir + path;
            else
                return string.Concat(dir, "/", path);
        }
    }
}
