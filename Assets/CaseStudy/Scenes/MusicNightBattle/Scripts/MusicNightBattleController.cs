using System;
using CaseStudy.Scripts.MusicNightBattle;
using CaseStudy.Scripts.MusicNightBattle.Configs;
using GFramework.Scene;
using Microsoft.Win32.SafeHandles;
using UnityEngine;
using Zenject;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

// ReSharper disable once CheckNamespace
namespace CaseStudy.Scenes.MusicNightBattle
{
    public class MusicNightBattleController : SceneController
    {
        [SerializeField] private AudioSource _songAudioSource;
        [SerializeField] private AudioSource _beatSFXAudioSource;
        [SerializeField] private AudioSource _missSFXAudioSource;
        [Inject] private MusicNightBattleLogic _logic;
        [Inject] private SongController _songController;
        [Inject] private SongConfig _songConfig;

        private void Awake()
        {
            _songAudioSource.clip = _songConfig.SongAudioClip;
            _beatSFXAudioSource.clip = _songConfig.BeatSFX;
            _missSFXAudioSource.clip = _songConfig.MissSFX;

            // controller initialization
            _songController.Init(_songAudioSource);
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