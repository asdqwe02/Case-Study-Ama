using System.Collections.Generic;
using System.Linq;
using DG.DemiEditor;
using GFramework.Core.Editor;
using GFramework.Utils;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GFramework.Macro.Editor
{
    [UsedImplicitly]
    public class MacroMenu : Menu<MacroData>
    {
        public override string MenuName => "Settings/Macro";

        [UsedImplicitly]
        [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        public MacroData MacroData;

        public MacroMenu()
        {
            MacroData = ConfigHelper.GetConfig<MacroData>();
            UseOdin = MacroData.UseOdin;
            OdinMacros = MacroData.OdinMacros;
        }

        public override bool UseCustomMenu => true;

        [Header("Modules")]
        [UsedImplicitly]
        [OnValueChanged("MacrosModulesChange")]
        public bool UseOdin;

        [ShowIf("UseOdin")]
        public List<string> OdinMacros;
        void MacrosModulesChange()
        {
            var macros = MacroData.Macros;
            MacroData.UseOdin = UseOdin;
            if (UseOdin)
            {
                foreach (var macro in MacroData.OdinMacros)
                {
                    if (!macros.Contains(macro))
                    {
                        macros.Add(macro);
                    }
                }
            }
            else
            {
                foreach (var macro in MacroData.OdinMacros)
                {
                    macros.Remove(macro);
                }
            }
        }

        [Button(ButtonSizes.Gigantic), GUIColor(0, 1, 0)]
        public void GenerateMacros()
        {
            // var macros = MacroData.Macros;
            // if (UseOdin)
            // {
            //     if (!macros.Contains("ODIN_INSPECTOR"))
            //     {
            //         macros.Add("ODIN_INSPECTOR");
            //     }
            //
            //     if (!macros.Contains("ODIN_INSPECTOR_3"))
            //     {
            //         macros.Add("ODIN_INSPECTOR_3");
            //     }
            //
            //     if (!macros.Contains("ODIN_INSPECTOR_3_1"))
            //     {
            //         macros.Add("ODIN_INSPECTOR_3_1");
            //     }
            // }

            var defines = MacroData.Macros.Aggregate("", (current, item) => $"{current}{item}; ");

            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, defines);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, defines);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, defines);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL, defines);

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }
    }
}