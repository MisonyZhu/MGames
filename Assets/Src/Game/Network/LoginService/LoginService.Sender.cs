//using Game.Net;
//using Game.Net.User;

//namespace Game
//{
//    public partial class LoginService
//    {
//        public void SendLogin(string account)
//        {
//            var msg = ReqLoginMessage.instance;
//            msg.LoginName = account;
//            ReqLoginMessage.Send(msg);
//        }

//        public void SendEnterRole(long rid)
//        {
//            var msg = Game.Net.User.ReqEnterRoleMessage.instance;
//            msg.Rid = rid;
//            ReqEnterRoleMessage.Send(msg);
//        }
//    }    
//}