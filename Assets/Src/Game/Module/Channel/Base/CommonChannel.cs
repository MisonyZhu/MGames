using App;

namespace Game
{
    public  class CommonChannel : IChannel
    {
        protected ServerChannelInfo m_ChannelInfo; 

        public virtual void Init()
        {
            m_ChannelInfo = ChannelModule.Instance.CurChannelInfo;
        }

        public virtual void ShowLogin(params object[] datas)
        {
            // LoginUI.instance.Show();
        }

        public virtual void ShowPay(params object[] datas)
        {
           //Todo
        }
    }
}