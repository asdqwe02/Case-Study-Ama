using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Win32;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GFramework.Preferences.Editor
{
    [UsedImplicitly]
    [Serializable]
    public class PlayerPrefsMenu: Core.Editor.Menu
    {
        [SerializeField]
        private List<PlayerPrefsPair> _playerPrefsPairs = new();
        
        private List<string> _currentKeys = new();

        public override string MenuName => "Player Prefs Editor";
        public override bool UseCustomMenu => true;

        public PlayerPrefsMenu()
        {
            LoadPlayerPrefs();
        }

        [Button(ButtonSizes.Gigantic), GUIColor(0, 1, 0)]
        public void Save()
        {
            foreach (var key in _currentKeys.Where(currentPair => _playerPrefsPairs.All(p => p.Key != currentPair)))
            {
                PlayerPrefs.DeleteKey(key);
            }

            foreach (var pair in _playerPrefsPairs)
            {
                switch (pair.Type)
                {
                    case PlayerPrefsType.INT:
                        PlayerPrefs.SetInt(pair.Key, pair.IntValue);
                        break;
                    case PlayerPrefsType.FLOAT:
                        PlayerPrefs.SetFloat(pair.Key, pair.FloatValue);
                        break;
                    case PlayerPrefsType.STRING:
                        PlayerPrefs.SetString(pair.Key, pair.StringValue);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            PlayerPrefs.Save();
            LoadPlayerPrefs();
        }

        [Button(ButtonSizes.Gigantic), GUIColor(0, 1, 0)]
        public void Reload()
        {
            LoadPlayerPrefs();
        }

        private void LoadPlayerPrefs()
        {
            var companyName = PlayerSettings.companyName;
            var productName = PlayerSettings.productName;
            
            _playerPrefsPairs.Clear();

            if (Application.platform != RuntimePlatform.WindowsEditor)
            {
                return;
            }
            var registryKey = Registry.CurrentUser.OpenSubKey($"Software\\Unity\\UnityEditor\\{companyName}\\{productName}");
            if (registryKey == null)
            {
                return;
            }
            var valueNames = registryKey.GetValueNames();
            foreach (var valueName in valueNames)
            {
                var lastUnderscoreIndex = valueName.LastIndexOf("_", StringComparison.Ordinal);
                var key = valueName.Remove(lastUnderscoreIndex, valueName.Length - lastUnderscoreIndex);

                _currentKeys.Add(key);
                var valueWithUnknownType = registryKey.GetValue(valueName);
                        
                switch (valueWithUnknownType)
                {
                    case int when PlayerPrefs.GetInt(key, -1) == -1 && PlayerPrefs.GetInt(key, 0) == 0:
                        _playerPrefsPairs.Add(new PlayerPrefsPair
                        {
                            Key = key,
                            FloatValue = PlayerPrefs.GetFloat(key),
                            Type = PlayerPrefsType.FLOAT,
                        });
                        break;
                    case int:
                        _playerPrefsPairs.Add(new PlayerPrefsPair
                        {
                            Key = key,
                            IntValue = PlayerPrefs.GetInt(key),
                            Type = PlayerPrefsType.INT,
                        });
                        break;
                    case byte[]:
                        _playerPrefsPairs.Add(new PlayerPrefsPair
                        {
                            Key = key,
                            StringValue = PlayerPrefs.GetString(key),
                            Type = PlayerPrefsType.STRING,
                        });
                        break;
                }
            }
        }
    }
}