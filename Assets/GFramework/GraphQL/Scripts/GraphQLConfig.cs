using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GFramework.GraphQL
{
    [CreateAssetMenu(fileName = "GraphQLConfig.asset", menuName = "GFramework/Settings/GraphQL Config")]
    public class GraphQLConfig: ScriptableObject
    {
        private const string FILE_NAME_REGEX =  @"^[a-zA-Z]+$";

        [SerializeField] 
        private List<GraphQLQuery> _queries;
        public List<GraphQLQuery> Queries => _queries;

        [SerializeField] 
        private string _host;
        public string Host => _host;

        [FolderPath(RequireExistingPath = true, ParentFolder = "Assets")]
        [SerializeField]
        private string _graphQLDefineFileSavePath;
        public string GraphQLDefineFileSavePath => _graphQLDefineFileSavePath;
        [ValidateInput("ValidFileName", "File name invalid.")]
        [SerializeField]
        private string _graphQLDefineFileName;
        public string GraphQLDefineFileName => _graphQLDefineFileName;

        [UsedImplicitly]
        private bool ValidFileName(string value)
        {
            return Regex.IsMatch(value,FILE_NAME_REGEX);
        }
    }
}