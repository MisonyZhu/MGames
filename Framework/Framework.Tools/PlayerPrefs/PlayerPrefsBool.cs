
using UnityEngine;

namespace Framework
{
    public class PlayerPrefsBool : PlayerPrefsValueT<bool>
    {
        public PlayerPrefsBool(string key, bool @default)
            : base(key, @default)
        {
        }

        protected override bool DoGet(bool @default) => PlayerPrefs.GetInt(this.key, @default ? 1 : 0) != 0;

        protected override void DoSet() => PlayerPrefs.SetInt(this.key, this.value ? 1 : 0);
    }
}