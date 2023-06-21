using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GFramework.Scene
{
    [CreateAssetMenu(fileName = "SceneConfig.asset", menuName = "GFramework/Settings/Scene Config")]
    public class SceneConfig: ScriptableObject
    {
        private const string FILE_NAME_REGEX =  @"^[a-zA-Z]+$";

        [ReadOnly] 
        [SerializeField]
        private List<SceneModel> _scenes;
        public List<SceneModel> Scenes  => _scenes;

        [ValueDropdown("Scenes")] 
        [SerializeField]
        private SceneModel _startingScene;
        public SceneModel StartingScene => _startingScene;

        [FolderPath(RequireExistingPath = true, ParentFolder = "Assets")]
        [SerializeField]
        private string _sceneRootPath;
        public string SceneRootPath => _sceneRootPath;

        [FolderPath(RequireExistingPath = true, ParentFolder = "Assets")] 
        [SerializeField]
        private string _sceneDefineFileSavePath;
        public string SceneDefineFileSavePath => _sceneDefineFileSavePath;
        [ValidateInput("ValidFileName", "File name invalid.")]
        [SerializeField]
        private string _sceneDefineFileName;
        public string SceneDefineFileName => _sceneDefineFileName;

        [HideInInspector] public string SceneWaitingForComponents = "";
        

        [UsedImplicitly]
        private bool ValidFileName(string value)
        {
            return Regex.IsMatch(value,FILE_NAME_REGEX);
        }

        public void ResetStartingScene()
        {
            _startingScene = null;
        }
    }
}