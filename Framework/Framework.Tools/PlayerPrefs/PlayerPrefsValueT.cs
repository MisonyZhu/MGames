
namespace Framework
{
    public abstract class PlayerPrefsValueT<T> : PlayerPrefsValue
    {
        public string key;
        private T m_Value;
        private bool m_ValueGetted = false;

        public T value
        {
            get
            {
                if (!this.m_ValueGetted)
                {
                    this.m_Value = this.DoGet(this.m_Value);
                    this.m_ValueGetted = true;
                }
                return this.m_Value;
            }
            set
            {
                if (object.Equals((object) this.value, (object) value))
                    return;
                this.m_Value = value;
                this.DoSet();
                PlayerPrefsValue.m_Dirty = true;
                PlayerPrefsValue.Save();
            }
        }

        public PlayerPrefsValueT(string key, T @default)
        {
            this.key = key;
            this.m_Value = @default;
            this.m_ValueGetted = false;
        }

        public static implicit operator T(PlayerPrefsValueT<T> prefsValue) => prefsValue.value;

        public void Set(T value) => this.value = value;

        protected abstract T DoGet(T @default);

        protected abstract void DoSet();

        public override string ToString() => this.value.ToString();
    }
}