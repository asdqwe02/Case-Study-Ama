using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace GFramework.Utils
{
    public static class ConfigHelper
    {
        private const string CONFIG_PATH = "Assets/GFramework/Resources/GSettings";
        // ReSharper disable once UnusedMember.Local
        private const string RUNTIME_CONFIG_PATH = "GSettings";

        public static T GetConfig<T>() where T : ScriptableObject
        {
#if UNITY_EDITOR
            var objectName = $"{typeof(T).Name}.asset";
            if (!Directory.Exists (CONFIG_PATH))
            {
                Directory.CreateDirectory(CONFIG_PATH);
            }

            var item = AssetDatabase.LoadAssetAtPath<T>($"{CONFIG_PATH}/{objectName}");
            if (item)
            {
                return item;
            }

            item = ScriptableObject.CreateInstance<T>();

            var savePath = $"{CONFIG_PATH}/{objectName}";
            AssetDatabase.CreateAsset(item,savePath);
            AssetDatabase.SaveAssets();

            return item;
#else
            var objectName = $"{typeof(T).Name}";
            var item = UnityEngine.Resources.Load<T>($"{RUNTIME_CONFIG_PATH}/{objectName}");
            return item;
#endif
        }
      
#if UNITY_EDITOR
        public static string GetConfigPath<T>() where T : ScriptableObject
        {
            var objectName = $"{typeof(T).Name}.asset";
            if (!Directory.Exists (CONFIG_PATH))
            {
                Directory.CreateDirectory(CONFIG_PATH);
            }

            var item = AssetDatabase.LoadAssetAtPath<T>($"{CONFIG_PATH}/{objectName}");
            if (item)
            {
                return $"{CONFIG_PATH}/{objectName}";
            }
            
            item = ScriptableObject.CreateInstance<T>();

            var savePath = $"{CONFIG_PATH}/{objectName}";
            AssetDatabase.CreateAsset(item,savePath);
            AssetDatabase.SaveAssets();

            return $"{CONFIG_PATH}/{objectName}";
        }
#endif

    }
}