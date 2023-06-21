using System;
using Sirenix.OdinInspector;

namespace GFramework.Preferences.Editor
{
    [Serializable]
    public class PlayerPrefsPair
    {
        public string Key;
        public PlayerPrefsType Type;

        [ShowIf("Type", PlayerPrefsType.STRING)]
        public string StringValue;
        [ShowIf("Type", PlayerPrefsType.INT)]
        public int IntValue;
        [ShowIf("Type", PlayerPrefsType.FLOAT)]
        public float FloatValue;
    }
}