using System.Collections.Generic;
using System.IO;
using System.Linq;
using GFramework.Core.Editor;
using GFramework.Utils;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GFramework.GraphQL.Editor
{
    public class GraphQLMenu: Menu<GraphQLConfig>
    {
        public override string MenuName => "Settings/GraphQL";
        
        [UsedImplicitly]
        [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        public GraphQLConfig GraphQLConfig;

        public GraphQLMenu()
        {
            GraphQLConfig = ConfigHelper.GetConfig<GraphQLConfig>();
        }
        
        public override bool UseCustomMenu => true;

        [Button(ButtonSizes.Gigantic), GUIColor(0, 1, 0)]
        public void GenerateGraphQLConfig()
        {
            var body = "";

            body = GraphQLConfig.Queries.Aggregate(body, (current, item) => current + ("\t\tpublic const string " + item.QueryName + " = \"" + item.QueryName + "\";\n"));
            
            var ns = GraphQLConfig.GraphQLDefineFileSavePath.Replace('/', '.').Replace(".Scripts", "");
            
            var template = TemplateHelper.GetTemplate("GraphQLDefine", new Dictionary<string, string>
            {
                {"body", body},
                {"className", GraphQLConfig.GraphQLDefineFileName},
                {"namespace", ns},
            });

            using (var sw = new StreamWriter(Path.Combine(Application.dataPath, GraphQLConfig.GraphQLDefineFileSavePath, $"{GraphQLConfig.GraphQLDefineFileName}.cs")))
            {
                sw.Write (template);
            }

            AssetDatabase.Refresh (ImportAssetOptions.ForceUpdate);
        }
    }
}