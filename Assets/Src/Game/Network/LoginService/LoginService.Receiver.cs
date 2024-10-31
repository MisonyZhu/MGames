//using Game.Net;
//using Game.Net.User;

//namespace Game
//{
//    public partial class LoginService
//    {
//        public override void RegisterEvents()
//        {
//            ResLoginMessage.RegistEvent(LoginMessage);
//            ResEnterRoleMessage.RegistEvent(EnterRoleMessage);
//            ResCreateRoleMessage.RegistEvent(CreateRoleMessage);
//        }
        
//        void LoginMessage(LoginRes msg)
//        {
//            DnLoginData.LoginAck(msg);
//            GameEngine.EventManager.getSingletonPtr().FireNow(this, EventId.ID_Message_Login);            
//        }

//        void EnterRoleMessage(EnterRoleRes msg)
//        {
//            DnLoginData.EndRoleRes(msg);
//            GameEngine.EventManager.getSingletonPtr().FireNow(this, EventId.ID_Message_EnterRole);
//            RoomService.Instance.SendEntryRoom(DnLoginData.RoomType);
//        }

//        void CreateRoleMessage(CreateRoleRes msg)
//        {
//            DnLoginData.CreateRoleRes(msg);
//            GameEngine.EventManager.getSingletonPtr().FireNow(this, EventId.ID_Message_CreateRole);
//        }
//    }
//}