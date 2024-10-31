
using System;
using System.Collections.Generic;
using App;
using Framework;


namespace Game
{
    public class ChannelModule : ModuleBase<ChannelModule>
    {
        private IChannel m_Channellmpl;

        private readonly Dictionary<int, Type> m_AllCustomChannel = new Dictionary<int, Type>()
        {
            {2,typeof(TanWanChannel)},
        };
        
        public ServerChannelInfo CurChannelInfo => ChannelConfig.ServerChannelInfo;

        public int ChannelId => ChannelConfig.ServerChannelInfo.Id;

        public IChannel Channellmpl
        {
            get
            {
                if (m_Channellmpl == null)
                {
                    if (m_AllCustomChannel.ContainsKey(ChannelId))
                    {
                        m_AllCustomChannel.TryGetValue(ChannelId, out Type channelType);
                        m_Channellmpl =  Activator.CreateInstance(channelType) as IChannel;
                    }
                    else
                    {
                        m_Channellmpl =  Activator.CreateInstance(typeof(CommonChannel)) as IChannel;
                    }
                }
                return m_Channellmpl;
            }
        }


        public override void Init()
        {
            base.Init();
            
            if (m_Channellmpl == null)
            {
                Channellmpl.Init();
            }
            
        }

        public override int Priority => ModulePriority.Channel_Priority;
        
        public override void OnUpdate(float detlaTime)
        {
            
        }

        public void Reset()
        {
            
        }


        public void ShowLogin()
        {
            Channellmpl.ShowLogin();
        }

        public void ShowPay()
        {
            Channellmpl.ShowPay();
        }
        
        public override void OnShutDown()
        {
           
        }
    }
}