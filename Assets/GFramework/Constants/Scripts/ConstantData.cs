using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GFramework.Constants
{ 
    public class ConstantData: ScriptableObject
    {
        private const string FILE_NAME_REGEX =  @"^[a-zA-Z]+$";

        [SerializeField]
        private List<ConstantGroup> _groups;
        public List<ConstantGroup> Groups => _groups;
        [FolderPath(RequireExistingPath = true, ParentFolder = "Assets")]
        [SerializeField]
        private string _constantDefineFileSavePath;
        public string ConstantDefineFileSavePath => _constantDefineFileSavePath;
        [ValidateInput("ValidFileName", "File name invalid.")]
        [SerializeField]
        private string _constantDefineFileName;
        public string ConstantDefineFileName => _constantDefineFileName;

        [UsedImplicitly]
        private bool ValidFileName(string value)
        {
            return Regex.IsMatch(value,FILE_NAME_REGEX);
        }
    }
}