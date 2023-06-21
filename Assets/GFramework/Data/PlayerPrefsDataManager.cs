using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;

namespace GFramework.Data
{
    [UsedImplicitly]
    public class PlayerPrefsDataManager: IDataManager
    {
        public void Save<T>(string key, T value) where T : class
        {
            var jsonData = JsonConvert.SerializeObject(value);
            
            PlayerPrefs.SetString(key, jsonData);
        }

        public T Get<T>(string key) where T : class
        {
            var savedString = PlayerPrefs.GetString(key);
            if (savedString == "")
            {
                return null;
            }

            var data = JsonConvert.DeserializeObject<T>(savedString);
            return data;
        }

    }
}