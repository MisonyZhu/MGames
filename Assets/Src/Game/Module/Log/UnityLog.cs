using Framework;

namespace Game
{
    public class UnityLog : ILog
    {
        public static UnityLog Create()
        {
            var log = new UnityLog();
            Log.SetLog(log);
            return log;
        }

        public void LogMsg(string msg)
        {
            UnityEngine.Debug.Log(msg);
        }

        public void LogWarn(string msg)
        {
            UnityEngine.Debug.LogWarning(msg);
        }

        public void LogError(string msg)
        {
            UnityEngine.Debug.LogError(msg);
        }
    }
}