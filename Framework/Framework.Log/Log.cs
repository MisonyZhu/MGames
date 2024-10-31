
namespace Framework
{
    public class Log 
    {
        public static ILog LogAdapter;
      
        public static void SetLog(ILog log)
        {
            LogAdapter = log;
        }


        internal static void LogMsg(string msg)
        {
            LogAdapter.LogMsg(msg);
        }


        internal static void LogWarn(string msg)
        {
            LogAdapter.LogWarn(msg);
        }


        internal static void LogError(string msg)
        {
            LogAdapter.LogError(msg);
        }
    }
}