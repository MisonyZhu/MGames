using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 游戏主入口
    /// </summary>
    public class GameEntry : MonoSingleton<GameEntry>
    {
        private AppConfig m_Config;
        
        private UnityModule m_Module;

        public AppConfig Config => m_Config;
        
        protected override void Awake()
        {
            m_Config = AppEntry.Instance.Config;
            base.Awake();
            
            m_Module = UnityModule.CreateModule();
        }
        

        void Start()
        {
            
        }


        void Update()
        {
            m_Module?.Update(Time.deltaTime);
        }

        private void OnDestroy()
        {
            m_Module?.ShutDown();
        }

        public void BrigeConfig(AppConfig config)
        {
            m_Config = config;
        }
    }
}
