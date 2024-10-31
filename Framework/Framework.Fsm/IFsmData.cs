namespace Framework
{
    public interface IFsmData
    {
        void SetData(string key, object data);
        object GetData(string key);

        void Reset();
        
        void SetData<TO>(int value);
        void SetData<TO>(float value);
        void SetData<TO>(string value);
        void SetData<TO>(bool value);

        void SetData<TO, T1>(int value, int value1);
        void SetData<TO, T1>(float value, float value1);
        void SetData<TO, T1>(string value, string value1);
        void SetData<TO, T1>(bool value, bool value1);

        void SetData<TO, T1, T2>(int value, int value1, int value2);
        void SetData<TO, T1, T2>(float value, float value1, float value2);
        void SetData<TO, T1, T2>(string value, string value1, string value2);
        void SetData<TO, T1, T2>(bool value, bool value1, bool value2);
    }
}