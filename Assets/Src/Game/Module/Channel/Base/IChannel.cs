namespace Game
{
    public interface IChannel
    {
        void Init();
        
        void ShowLogin(params object[] datas);
        

        void ShowPay(params object[] datas);

        
    }
}