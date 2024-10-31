using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App
{
    [CreateAssetMenu(fileName = "AppChannelSettingConfig", menuName = "创建渠道配置", order = 0)]
    public class ChannelConfig : ScriptableObject
    {
        [SerializeField,LabelText("当前渠道"),ValueDropdown("EditorGetAllInfo")]
        private  int m_CurChannelId;

        private   IEnumerable EditorGetAllInfo()
        {
            return ChannelConfigs.Select(x => new ValueDropdownItem(x.Key +"("+x.Value.Name+")", x.Key));
        }

        public int ChannelId => m_CurChannelId;

        [DictionaryDrawerSettings(KeyLabel = "渠道ID", ValueLabel = "渠道信息")]
        [ShowInInspector,LabelText("渠道配置")]
        public Dictionary<int, LocalChannelConfig> ChannelConfigs = new Dictionary<int, LocalChannelConfig>()
        {
            {0,new LocalChannelConfig(){Id = 1,Name = "测试"}},
            {1,new LocalChannelConfig(){Id = 2,Name = "测试热更"}},
        };

        private LocalChannelConfig GetChannelInfoById(int id)
        {
            ChannelConfigs.TryGetValue(id, out LocalChannelConfig info);
            if (info==null)
            {
                Debug.LogError("读取渠道信息失败");
                UIMessage.Instance.ShowTip("读取渠道信息失败，请检查渠道配置！", Application.Quit);
                //TODO 处理渠道信息失败
            }
            return info;
        }

#if UNITY_EDITOR
        public void ChangeChannel(int id)
        {
            m_CurChannelId = id;
        }
#endif

        private static ChannelConfig m_ChannelData;

        public static LocalChannelConfig LocalChannelConfig { get; private set; }
        

        public static ServerChannelInfo ServerChannelInfo;


        public static void LoadConfig()
        {
            m_ChannelData = Resources.Load<ChannelConfig>("AppChannelSettingConfig");
            LocalChannelConfig = m_ChannelData.GetChannelInfoById(m_ChannelData.ChannelId);
        }
    }
}