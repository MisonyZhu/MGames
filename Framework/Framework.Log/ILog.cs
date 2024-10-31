using System.Diagnostics;

namespace Framework
{
    public interface ILog
    {
        void LogMsg(string msg);

        void LogWarn(string msg);

        void LogError(string msg);
        
    }
}