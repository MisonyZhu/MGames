using System;
using System.Collections.Generic;

namespace Framework
{
    public class FsmData : IFsmData
    {
        private Dictionary<string, object> m_Data = new Dictionary<string, object>();

        public void SetData(string key, object data)
        {
            if (m_Data.ContainsKey(key))
            {
                m_Data[key] = data;
            }
            else
            {
                m_Data.Add(key,data);
            }

        }

        public object GetData(string key)
        {
            m_Data.TryGetValue(key, out object data);
            return data;
        }

        public void Reset()
        {
            m_Data.Clear();
        }

        #region AllowGC 
        
        private int m_IntValue1, m_IntValue2, m_IntValue3;
        private string m_StringValue1, m_StringValue2, m_StringValue3;
        private float m_FloatValue1, m_FloatValue2, m_FloatValue3;
        private bool m_BoolValue1, m_BoolValue2, m_BoolValue3;
        public void SetData<I0>(int value)
        {
            m_IntValue1 = value;
        }
        
        public void SetData<S0>(string value)
        {
            m_StringValue1 = value;
        }
        
        public void SetData<F0>(float value)
        {
            m_FloatValue1 = value;
        }
        
        public void SetData<B0>(bool value)
        {
            m_BoolValue1 = value;
        }
        
        public void SetData<I0,I1>(int value,int value1)
        {
            m_IntValue1 = value;
            m_IntValue2 = value1;
        }
        
        public void SetData<S0,S1>(string value,string value1)
        {
            m_StringValue1 = value;
            m_StringValue2 = value1;
        }
        
        public void SetData<F0,F1>(float value,float value1)
        {
            m_FloatValue1 = value;
            m_FloatValue2 = value1;
        }
        
        public void SetData<B0,B1>(bool value,bool value1)
        {
            m_BoolValue1 = value;
            m_BoolValue1 = value1;
        }
        
        public void SetData<I0,I1,I2>(int value,int value1,int value2)
        {
            m_IntValue1 = value;
            m_IntValue2 = value1;
            m_IntValue3 = value2;
        }
      
        public void SetData<S0,S1,S2>(string value,string value1,string value2)
        {
            m_StringValue1 = value;
            m_StringValue2 = value1;
            m_StringValue3 = value2;
        }

        public void SetData<F0,F1,F2>(float value,float value1,float value2)
        {
            m_FloatValue1 = value;
            m_FloatValue2 = value1;
            m_FloatValue3 = value2;
        }

        public void SetData<B0,B1,B2>(bool value,bool value1,bool value2)
        {
            m_BoolValue1 = value;
            m_BoolValue2 = value1;
            m_BoolValue3 = value2;
        }
        
        #endregion
        
        

    }
}