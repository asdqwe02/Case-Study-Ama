using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace GFramework.Core.Editor
{
    public class GMenuWindow: OdinMenuEditorWindow
    {
        [MenuItem("GFramework/Main Menu")]
        private static void OpenWindow()
        {
            GetWindow<GMenuWindow>().Show();
        }

        private readonly List<Menu> _menus = new();

        protected override void OnEnable()
        {
            base.OnEnable();
            _menus.Clear();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!type.IsSubclassOf(typeof(Menu)) || type.IsAbstract)
                    {
                        continue;
                    }
                    var menu = Activator.CreateInstance(type);
                    _menus.Add(menu as Menu);
                }
            }
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree
            {
                Selection =
                {
                    SupportsMultiSelect = false
                }
            };
            foreach (var menu in _menus)
            {
                if (menu.UseCustomMenu)
                {
                    tree.Add($"{menu.MenuName}", menu);
                }
                else
                {
                    tree.AddAssetAtPath($"{menu.MenuName}",menu.AssetPath);
                }
                
            }
            tree.Add("Settings/Save Settings", new SaveButton());
            return tree;
        }

        private class SaveButton
        {
            [Button(ButtonSizes.Gigantic), GUIColor(0, 1, 0)]
            public void Save()
            {
                AssetDatabase.SaveAssets();
            }
        }
    }
}