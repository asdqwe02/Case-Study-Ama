using CaseStudy.Scripts.MusicNightBattle.Configs;
using CaseStudy.Scripts.MusicNightBattle.GameLogicControllers;
using CaseStudy.Scripts.MusicNightBattle.Signals;
using GFramework.Scene;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

// ReSharper disable once CheckNamespace
namespace CaseStudy.Scenes.MusicNightBattle.Scripts
{
    public class MusicNightBattleController : SceneController
    {
        [SerializeField] private AudioSource _songAudioSource;
        [SerializeField] private AudioSource _beatSFXAudioSource;
        [SerializeField] private AudioSource _missSFXAudioSource;

        [SerializeField] private GameObject _titleScreen;
        [SerializeField] private GameObject _mainGameUI;

        [SerializeField] private Button _startButton;
        [SerializeField] private Button _tryAgainButton;

        [Inject] private MusicNightBattleLogic _logic;
        [Inject] private ISongController _songController;
        [Inject] private SongConfig _songConfig;
        [Inject] private SignalBus _signalBus;

        private void Awake()
        {
            _songAudioSource.clip = _songConfig.SongAudioClip;
            _beatSFXAudioSource.clip = _songConfig.BeatSFX;
            _missSFXAudioSource.clip = _songConfig.MissSFX;

            _startButton.onClick.AddListener(StartButtonClick);
            _tryAgainButton.onClick.AddListener(StartButtonClick);

            // controller initialization
            _songController.Init(_songAudioSource);
            _logic.Init();

            // signal bus subscription
            _signalBus.Subscribe<GameState>(OnGameState);
            _signalBus.Subscribe<HitNoteSignal>(OnNoteHit);
            _signalBus.Subscribe<MissNoteSignal>(OnNoteMiss);
        }

        private void Start()
        {
            _mainGameUI.SetActive(false);
        }

        private void OnNoteMiss(MissNoteSignal obj)
        {
            _missSFXAudioSource.Play();
        }

        private void OnNoteHit(HitNoteSignal obj)
        {
            _beatSFXAudioSource.Play();
        }

        private void OnGameState(GameState obj)
        {
            switch (obj)
            {
                case GameState.START:
                    _titleScreen.SetActive(false);
                    _mainGameUI.SetActive(true);
                    Canvas.ForceUpdateCanvases();
                    _signalBus.Fire<UpdateLanePositionSignal>();
                    break;
                case GameState.FINISH:
                    _startButton.gameObject.SetActive(false);
                    _tryAgainButton.gameObject.SetActive(true);
                    _titleScreen.SetActive(true);
                    _mainGameUI.SetActive(false);
                    break;
            }
        }

        public void StartButtonClick()
        {
            _signalBus.Fire(CountDownState.START);
        }

        //This function call when our scene is being loaded
        public override void OnSceneLoaded(object data)
        {
            base.OnSceneLoaded(data);
        }

        //This function call when our scene is being removed/unloaded
        public override void OnSceneUnloaded()
        {
            base.OnSceneUnloaded();
        }


#if UNITY_EDITOR
        [MenuItem("GFramework/Open Scene/MusicNightBattle")]
        public static void OpenSceneMusicNightBattle()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(@"Assets/CaseStudy/Scenes/MusicNightBattle/Scenes/MusicNightBattle.unity");
            }
        }
#endif
    }
}