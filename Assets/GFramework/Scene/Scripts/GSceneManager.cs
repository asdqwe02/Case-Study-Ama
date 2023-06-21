using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace GFramework.Scene
{
    public class GSceneManager: MonoBehaviour, ISceneManager
    {
        private object _data;
        private SceneModel _currentScene;
        private SceneController _currentSceneController;
        private readonly List<SceneModel> _sceneStack = new();

        private bool _isBusy = false;

        [Inject]
        private SceneConfig _config;

        private bool _activeSceneChanged;

        [SerializeField] private GameObject _shield;

        private void Start()
        {
            _currentScene = _config.Scenes.First(scene => scene.ScenePath == SceneManager.GetActiveScene().path);
            _sceneStack.Add(_currentScene);
            _currentSceneController = FindObjectOfType<SceneController>();
            _currentSceneController.OnSceneLoaded(null);

            SceneManager.activeSceneChanged += OnActiveSceneChanged;

        }

        private void OnActiveSceneChanged(UnityEngine.SceneManagement.Scene current, UnityEngine.SceneManagement.Scene next)
        {
            _activeSceneChanged = true;
        }

        public void PushScene(string sceneName, object data = null)
        {
            var scene = _config.Scenes.First(scene => scene.SceneName == sceneName);
            StartCoroutine(DoPushScene(scene, data));
        }

        public void PopScene(object data = null)
        {
            StartCoroutine(DoPopScene(data));
        }

        private IEnumerator DoPushScene(SceneModel scene, object data = null)
        {
            while (_isBusy)
            {
                yield return null;
            }

            _isBusy = true;
            //Show shield
            _shield.gameObject.SetActive(true);
            
            _currentSceneController.OnSceneUnloaded();
            var ops = SceneManager.LoadSceneAsync(scene.LoadPath, LoadSceneMode.Single);
            yield return ops;

            SceneController nextSceneController = null;
            do
            {
                if (_activeSceneChanged)
                {
                    nextSceneController = FindObjectOfType<SceneController>();
                }
                yield return null;
            } while (nextSceneController == null);
            
            _currentSceneController = nextSceneController;
            _currentSceneController.OnSceneLoaded(data);
            
            _sceneStack.Add(scene);
            _activeSceneChanged = false;
            _isBusy = false;
            _shield.gameObject.SetActive(false);
        }
        
        private IEnumerator DoPopScene(object data = null)
        {
            while (_isBusy)
            {
                yield return null;
            }

            _isBusy = true;
            //Show shield
            _shield.gameObject.SetActive(true);

            _currentSceneController.OnSceneUnloaded();
            var scene = _sceneStack[^2];
            var ops = SceneManager.LoadSceneAsync(scene.LoadPath, LoadSceneMode.Single);
            yield return ops;

            SceneController nextSceneController = null;
            do
            {
                if (_activeSceneChanged)
                {
                    nextSceneController = FindObjectOfType<SceneController>();
                }
                yield return null;
            } while (nextSceneController == null);
            
            _currentSceneController = nextSceneController;
            _currentSceneController.OnSceneLoaded(data);
            
            _sceneStack.RemoveAt(_sceneStack.Count - 1);
            _activeSceneChanged = false;
            _isBusy = false;
            _shield.gameObject.SetActive(false);
        }

    }
}