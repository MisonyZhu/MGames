using Framework;

namespace Game
{
    /// <summary>
    /// 游戏主模块
    /// </summary>
    public class UnityModule : Moduler
    {

        public static UnityModule CreateModule()
        {
            UnityModule module = new UnityModule();
            return module;
        }


        UnityModule()
        {
            
        }

        public  void Update(float detlaTime)
        {
            OnUpdate(detlaTime);
        }

        public  void ShutDown()
        {
           OnShutdown();
        }
    }
}