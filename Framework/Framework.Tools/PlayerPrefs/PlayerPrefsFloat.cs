
using UnityEngine;

namespace Framework
{
    public class PlayerPrefsFloat : PlayerPrefsValueT<float>
    {
        public PlayerPrefsFloat(string key, float @default)
            : base(key, @default)
        {
        }

        protected override float DoGet(float @default) => PlayerPrefs.GetFloat(this.key, @default);

        protected override void DoSet() => PlayerPrefs.SetFloat(this.key, this.value);
    }
}