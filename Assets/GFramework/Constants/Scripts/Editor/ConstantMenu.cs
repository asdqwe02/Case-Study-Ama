using System.Collections.Generic;
using System.IO;
using GFramework.Core.Editor;
using GFramework.Utils;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GFramework.Constants.Editor
{
    [UsedImplicitly]
    public class ConstantMenu: Menu<ConstantData>
    {
        public override string MenuName => "Settings/Constants";
        
        [UsedImplicitly]
        [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        public ConstantData ConstantData;

        public ConstantMenu()
        {
            ConstantData = ConfigHelper.GetConfig<ConstantData>();
        }

        public override bool UseCustomMenu => true;

        [Button(ButtonSizes.Gigantic), GUIColor(0, 1, 0)]
        private void GenerateConstantsDefine()
        {
            var body = "";

            foreach (var item in ConstantData.Groups)
            {
                body += "\tpublic static class " + item.GroupName + "\n\t{\n";
                foreach (var data in item.Values)
                {
                    if (data.Type.IsStructType())
                    {
                        body += "\t\tpublic static readonly " + data.Type.GetTypeValue() + " " + data.ValueName + " = " + data.ValueString + ";\n";
                    }
                    else if (data.Type != DataValueType.STRING)
                    {
                        body += "\t\tpublic const " + data.Type.GetTypeValue() + " " + data.ValueName + " = " + data.ValueString + ";\n";
                    }
                    else
                    {
                        body += "\t\tpublic const " + data.Type.GetTypeValue() + " " + data.ValueName + " = \"" + data.ValueString + "\";\n";
                    }
                }
                body += "\n\t}\n";
            }

            var ns = ConstantData.ConstantDefineFileSavePath.Replace('/', '.').Replace(".Scripts", "");
            
            var template = TemplateHelper.GetTemplate("ConstantDefine", new Dictionary<string, string>
            {
                {"body", body},
                {"namespace", ns}
            });

            using (var sw = new StreamWriter(Path.Combine(Application.dataPath,ConstantData.ConstantDefineFileSavePath,$"{ConstantData.ConstantDefineFileName}.cs")))
            {
                sw.Write (template);
            }

            AssetDatabase.Refresh (ImportAssetOptions.ForceUpdate);
        }
    }
}