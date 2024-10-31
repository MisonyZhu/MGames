//using Game.Net.Room;

//namespace Game
//{
//    public partial class RoomService 
//    {
//        public override void RegisterEvents()
//        {
//            ResUpDataMessage.RegistEvent(OnResUpDataMessage);
//            ResEnterRoomMessage.RegistEvent(OnResEnterRoomMessage);
//            ResDestroyRoomMessage.RegistEvent(OnResDestroyRoomMessage);            
//        }

//        private void OnResUpDataMessage(ResUpData obj)
//        {
//            GameLogic.getSingletonPtr().OnHandler(obj);
//            GameEngine.EventManager.getSingletonPtr().FireNow(this, EventId.ID_Message_UpData);
//        }

//        private void OnResEnterRoomMessage(ResEnterRoom obj)
//        {
//            GameLogic.getSingletonPtr().EnterRoomRes(obj);
//            GameEngine.EventManager.getSingletonPtr().FireNow(this, EventId.ID_Message_EnterRoom);
//        }

//        private void OnResDestroyRoomMessage(ResDestroyRoom obj)
//        {
//            GameLogic.getSingletonPtr().DestroyRoomRes(obj);
//            GameEngine.EventManager.getSingletonPtr().FireNow(this, EventId.ID_Message_DestroyRoom);
//        }
//    }
//}