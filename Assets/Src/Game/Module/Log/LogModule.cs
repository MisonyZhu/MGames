using Framework;

namespace Game
{
    public class LogModule : ModuleBase<LogModule>
    {
        public override int Priority=> ModulePriority.Log_Priority;
        
        private static ILog Log;

        public override void Init()
        {
            base.Init();

            Log = UnityLog.Create();
        }

        public override void OnUpdate(float detlaTime)
        {

        }

        public override void OnShutDown()
        {

        }


        public static void LogMsg(string msg)
        {
            Log.LogMsg(msg);
        }
        
        public static void LogWorn(string msg)
        {
            Log.LogWarn(msg);
        }
        
        public static void LogError(string msg)
        {
            Log.LogError(msg);
        }
    }
}