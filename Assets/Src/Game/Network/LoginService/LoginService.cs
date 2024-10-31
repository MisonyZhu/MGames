using Game.Net;
//using Game.Net.User;
using UnityEngine;

namespace Game
{
    public partial class LoginService : NetServiceBase<LoginService>
    {
        private string account;
        
        public void TemporaryLogin(string account = "")
        {
            if (string.IsNullOrEmpty(account))
            {
                //MessageBoxUI.Show("账号不能为空","确定",null);
                return;
            }
            
            // TODO 临时写IP 后面读配置
            this.account = account;
            //NetManager.Connect("192.168.11.223", 8001, OnConnect);
        }
        
        public void OnConnect(bool success)
        {
            if (success)
            {
                Debug.Log("服务器连接成功！！");
                //SendLogin(account);
            }
            else
            {
                Debug.LogError("服务器连接失败！！");
            }
        }
        
        public override void Shutdown()
        {
        }
    }
}