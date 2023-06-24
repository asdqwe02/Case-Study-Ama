using System.Collections.Generic;
using CaseStudy.DesignPattern;
using CaseStudy.Scenes.MusicNightBattle;
using CaseStudy.Scenes.MusicNightBattle.Scripts;
using CaseStudy.Scripts.MusicNightBattle.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CaseStudy.Scripts.MusicNightBattle.Managers
{
    public class ScoreManager : MonoSingleton<ScoreManager>
    {
        [SerializeField] private AudioSource _hitSFX;
        [SerializeField] private AudioSource _missSFX;
        [SerializeField] private Slider _leftCharacterHPSlider;
        [SerializeField] private Slider _rightCharacterHPSlider;
        [SerializeField] private int _playerScore = 5;
        [SerializeField] private int _maxScore = 10;
        [SerializeField] private int _missPenalty = 4;

        [Inject] private SignalBus _signalBus;
        [Inject] private MusicNightBattleLogic _logic;
        private List<int> _initialValue = new();

        private void Awake()
        {
            CalculateHP();
            _initialValue.Add(_playerScore);
            _initialValue.Add(_maxScore);
            _initialValue.Add(_missPenalty);
            // _signalBus.Subscribe<GameOverSignal>(OnGameOver);
            _signalBus.Subscribe<GameState>(OnGameStateSignal);
        }

        private void OnGameStateSignal(GameState obj)
        {
            switch (obj)
            {
                case GameState.FINISH:
                    Reset();
                    break;
            }
        }

        // private void OnGameOver(GameOverSignal obj)
        // {
        //     Restart();
        // }

        public void MissSFX()
        {
            _missSFX.Play();
            // _playerScore--;
            _playerScore = Mathf.Clamp(_playerScore - _missPenalty, 0, _maxScore);
        }

        public void HitSFX()
        {
            _hitSFX.Play();
            // _playerScore--;
            _playerScore = Mathf.Clamp(++_playerScore, 0, _maxScore);
        }

        private void LateUpdate()
        {
            if (_logic.Started)
            {
                CalculateHP(); // not optimize
            }
        }

        public void Reset()
        {
            Debug.Log("Score reset");
            _playerScore = _initialValue[0];
            _maxScore = _initialValue[1];
            _missPenalty = _initialValue[2];
            _rightCharacterHPSlider.value = (float)_playerScore / _maxScore;
            _leftCharacterHPSlider.value = (float)(_maxScore - _playerScore) / _maxScore;
        }

        void CalculateHP()
        {
            var rightSliderValue = _rightCharacterHPSlider.value;
            var leftSliderValue = _leftCharacterHPSlider.value;
            var rightCharacterHP = (float)_playerScore / _maxScore;
            var leftCharacterHP = (float)(_maxScore - _playerScore) / _maxScore;
            _rightCharacterHPSlider.value = Mathf.Lerp(rightSliderValue, rightCharacterHP, 0.2f);
            _leftCharacterHPSlider.value = Mathf.Lerp(leftSliderValue, leftCharacterHP, 0.2f);
        }
    }
}