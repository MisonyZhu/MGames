namespace Game
{
    partial class UILogin : SingletonUI<UILogin>
    {
        // 初始化
        void Init()
        {
        }

        // 界面加载完毕
        protected override void OnCreate()
        {
            InitControls();
            RegistUIEvents();
        }

        // 界面显示
        protected override void OnShow()
        {
            RegistEvents();
            Refresh();
        }

        // 界面隐藏
        protected override void OnHide()
        {
        }

        // 界面销毁
        protected override void OnDestroy()
        {
        }

        #region UI Event
        void RegistUIEvents()
        {
            Button_Login.onClick = Button_LoginOnClick;
        }

        void Button_LoginOnClick(UIControl control)
        {
        }
        #endregion

        #region Game Event
        void RegistEvents()
        {
        }
        #endregion

        #region Refresh
        // 刷新界面
        public override void Refresh()
        {
        }
        #endregion
    }
}