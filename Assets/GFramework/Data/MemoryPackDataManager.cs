using System.IO;
using JetBrains.Annotations;
using MemoryPack;
using UnityEngine;

namespace GFramework.Data
{
    [UsedImplicitly]
    public class MemoryPackDataManager: IDataManager
    {
        private static string SavePath => Path.Combine(Application.persistentDataPath, "data");
        
        public void Save<T>(string key, T value) where T : class
        {
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }
            var bytes = MemoryPackSerializer.Serialize(value);
            File.WriteAllBytes(Path.Combine(SavePath, key), bytes);
        }

        public T Get<T>(string key) where T : class
        {
            var path = Path.Combine(SavePath, key);
            if (!File.Exists(path))
            {
                return null;
            }

            var bytes = File.ReadAllBytes(Path.Combine(SavePath, key));
            var data = MemoryPackSerializer.Deserialize<T>(bytes);
            return data;
        }
    }
}