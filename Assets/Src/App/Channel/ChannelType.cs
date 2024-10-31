using UnityEngine;

namespace App
{
    //存放在本地的，渠道请求配置
    public class LocalChannelConfig
    {
        public int Id;

        public string Name;
        
        public string ChannelUrl;
    }

    //存放在远端的 渠道信息配置
    public struct ServerChannelInfo 
    {
        public int Id;

        public string Name;
        
        public string ChannelUrl;

        public string ResourceUrl;
    }

  
    /***定义的渠道ID
    public enum EChannelId
    {
        Test,
        TestUpdate,
        
        新增xxx,
    }
    ****/
}