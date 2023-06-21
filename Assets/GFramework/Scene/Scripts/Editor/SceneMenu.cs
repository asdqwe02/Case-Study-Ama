using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GFramework.Core.Editor;
using GFramework.Utils;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using static UnityEditor.SceneManagement.EditorSceneManager;
using Object = UnityEngine.Object;

namespace GFramework.Scene.Editor
{
    public class SceneMenu: Menu<SceneConfig>
    {
        private const string SCENE_NAME_REGEX = @"^[A-Z][a-zA-Z0-9]+$";

        public override string MenuName => "Settings/Scenes";
        
        [UsedImplicitly]
        [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        public SceneConfig SceneConfig;

        [Header("Create new scene")] 
        [ValidateInput("ValidSceneName", "Scene name invalid.")]
        [ValidateInput("SceneNameUnique", "Scene name already exists.")]
        public string SceneName  = "";

        public List<SceneModel> Scenes => SceneConfig.Scenes;
        public SceneMenu()
        {
            SceneConfig = ConfigHelper.GetConfig<SceneConfig>();
            if (SceneConfig.SceneWaitingForComponents != "")
            {
                AddComponentsToScene();
            }
        }
        
        public override bool UseCustomMenu => true;

        [Button(ButtonSizes.Gigantic), GUIColor(0, 1, 0)]
        public void CreateNewScene()
        {
            if (!ValidSceneName(SceneName) || !SceneNameUnique(SceneName)) return;
            WriteSceneScripts();
            //create scene
            var scene = NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            InitScene();
            var success = SaveScene(scene, SavePath);
            if (!success) return;
            SceneConfig.Scenes.Add(new SceneModel
            {
                SceneName = SceneName,
                ScenePath = SavePath,
                LoadPath = SavePath.Replace("Assets/", "").Replace(".unity", ""),
                DeletePath = Path.Combine("Assets", SceneConfig.SceneRootPath, SceneName),
            });
            SceneConfig.SceneWaitingForComponents = SceneName;
            EditorUtility.SetDirty(SceneConfig); 
            UpdateEditorBuildSettings();
            WriteSceneDefineScript();
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            AssetDatabase.SaveAssets();
        }
        
        [Button(ButtonSizes.Gigantic), GUIColor(0, 1, 0)]
        public void UpdateEditorBuildSettings()
        {
            var scenes = SceneConfig.Scenes.ToList();
            var buildScenes = new List<SceneModel>();
            if (SceneConfig.StartingScene != null)
            {
                var firstScene = scenes.FirstOrDefault(scene => scene.SceneName == SceneConfig.StartingScene.SceneName);
                if (firstScene != null)
                {
                    buildScenes.Add(firstScene);
                }
            }
            buildScenes.AddRange(scenes.Where(scene => scene != buildScenes.FirstOrDefault()));
            EditorBuildSettings.scenes = buildScenes.Select(sceneConfig => new EditorBuildSettingsScene(sceneConfig.ScenePath, true)).ToArray();
        }

        
        [Header("Delete scene")] 
        [ValueDropdown("Scenes")]
        public SceneModel SceneToDelete;

        [Button(ButtonSizes.Gigantic), GUIColor(0, 1, 0)]
        public void DeleteScene()
        {
            Directory.Delete(SceneToDelete.DeletePath, true);
            SceneConfig.Scenes.Remove(SceneConfig.Scenes.First(s => s.SceneName == SceneToDelete.SceneName));
            if (SceneConfig.StartingScene != null && SceneConfig.StartingScene.SceneName == SceneToDelete.SceneName)
            {
                SceneConfig.ResetStartingScene();
            }
            EditorUtility.SetDirty(SceneConfig);
            AssetDatabase.SaveAssets();
            UpdateEditorBuildSettings();
            WriteSceneDefineScript();
        }

        private void InitScene()
        {
            var sceneContextObject = Object.Instantiate(
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GFramework/Scene/Prefabs/SceneContext.prefab"));
            sceneContextObject.name = "SceneContext";
            
            var sceneObject = Object.Instantiate(
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GFramework/Scene/Prefabs/NewScene.prefab"));
            sceneObject.name = SceneName;

            sceneContextObject.transform.position = Vector3.zero;
            sceneObject.transform.position = Vector3.zero;
        }

        private void AddComponentsToScene()
        {
            if (EditorApplication.isCompiling)
            {
                return;
            }
            var sceneName = SceneConfig.SceneWaitingForComponents;
            var sceneObject = GameObject.Find(sceneName);
            var sceneContextObject = GameObject.Find("SceneContext");

            var ns = $"{SceneConfig.SceneRootPath.Replace('/', '.').Replace(".Scripts", "")}.{sceneName}";
            var sceneType = Type.GetType($"{ns}.{sceneName}Controller,Assembly-CSharp");
            sceneObject.AddComponent(sceneType);
                
            var sceneInstallerType = Type.GetType($"{ns}.{sceneName}Installer,Assembly-CSharp");
            var installer = sceneContextObject.AddComponent(sceneInstallerType) as MonoInstaller;

            var sceneContext = sceneContextObject.GetComponent<SceneContext>();
            sceneContext.Installers = new[] { installer };
            MarkSceneDirty(SceneManager.GetActiveScene());
            var success = SaveOpenScenes();
            
            if (!success) return;
            SceneConfig.SceneWaitingForComponents = "";
            EditorUtility.SetDirty(SceneConfig); 
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            AssetDatabase.SaveAssets();
        }

        private void WriteSceneDefineScript()
        {
            var body = "";
            body = SceneConfig.Scenes.Aggregate(body, (current, item) => current + ("\t\tpublic const string " + item.SceneName + " = @\"" + item.SceneName + "\";\n"));

            var ns = SceneConfig.SceneDefineFileSavePath.Replace('/', '.').Replace(".Scripts", "");
            
            var template = TemplateHelper.GetTemplate("SceneDefine", new Dictionary<string, string>
            {
                {"body", body},
                {"className", SceneConfig.SceneDefineFileName},
                {"namespace", ns},
            });
            
            using (var sw = new StreamWriter(Path.Combine(Application.dataPath, SceneConfig.SceneDefineFileSavePath, $"{SceneConfig.SceneDefineFileName}.cs")))
            {
                sw.Write (template);
            }
            
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        private void WriteSceneScripts()
        {
            //Scene Controller script
            var nameSpace = SceneConfig.SceneRootPath.Replace('/', '.').Replace(".Scripts", "");
            nameSpace = $"{nameSpace}.{SceneName}";
            var sceneScript = TemplateHelper.GetTemplate("SceneController", new Dictionary<string, string>
            {
                {"sceneName", SceneName},
                {"scenePath", SavePath},
                {"namespace", nameSpace},
            });

            var writePath = Path.Combine("Assets", SceneConfig.SceneRootPath, SceneName, "Scripts");
            if (!Directory.Exists(writePath))
            {
                Directory.CreateDirectory(writePath);
            }

            using (var sw = new StreamWriter(Path.Combine(writePath, $"{SceneName}Controller.cs")))
            {
                sw.Write(sceneScript);
            }

            //Scene Logic script
            var logicScript = TemplateHelper.GetTemplate("SceneLogic", new Dictionary<string, string>
            {
                {"sceneName", SceneName},
                {"namespace", nameSpace},
            });

            using (var sw = new StreamWriter(Path.Combine(writePath, $"{SceneName}Logic.cs")))
            {
                sw.Write(logicScript);
            }

            //Installer script
            var installerScript = TemplateHelper.GetTemplate("SceneInstaller", new Dictionary<string, string>
            {
                {"sceneName", SceneName},
                {"namespace", nameSpace},
            });

            using (var sw = new StreamWriter(Path.Combine(writePath, $"{SceneName}Installer.cs")))
            {
                sw.Write(installerScript);
            }

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        private string SavePath
        {
            get
            {
                var savePath = Path.Combine("Assets", SceneConfig.SceneRootPath, SceneName, "Scenes");
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                savePath = Path.Combine(savePath, $"{SceneName}.unity");
                savePath = savePath.Replace("\\", "/");
                return savePath;
            }
        }
        
        [UsedImplicitly]
        private bool ValidSceneName(string value)
        {
            return Regex.IsMatch(value,SCENE_NAME_REGEX);
        }
        
        [UsedImplicitly]
        private bool SceneNameUnique(string value)
        {
            return SceneConfig.Scenes.All(s => !string.Equals(s.SceneName, value, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}