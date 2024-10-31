namespace Game
{
    public partial class EventID
    {
        #region GameSystem 1-1000
        public const int Game_LowMemory = 1;
        public const int Game_SceneLoaded = 2;
        public const int Game_ApplicationFocus = 3;
        public const int Game_ApplicationPause = 4;
        public const int Game_ApplicationQuit = 5;



        #endregion

        #region GameModule 1001-2001

        public const int UI_Create = 1001;
        public const int UI_Show = 1002;
        public const int UI_Hide = 1003;
        public const int UI_Destroy = 1004;

        //Net_ConnectSuccess,
        //Net_ConnectFailed,
        //Net_Disconnect,
        #endregion



        //GamePlay_Enter,
        //GamePlay_Leave,
    }
}
