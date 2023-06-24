using System.Collections.Generic;
using System.Linq;
using CaseStudy.DesignPattern;
using CaseStudy.Scripts.MusicNightBattle.Managers;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CaseStudy.Scripts.MusicNightBattle
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private GameObject _titleScreen;
        [SerializeField] private GameObject _mainGameUI;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _tryAgainButton;
        [Inject] private CountdownController _countdownController;

        [SerializeField] private CharacterSpriteController _rightCharacterSpriteController;
        private bool _started = false;
        public bool Started => _started;
        private List<Lane> _laneFinished = new();

        [Inject] private ISongController _songController;

        private void Awake()
        {
            _startButton.onClick.AddListener(StartButtonClick);
            _tryAgainButton.onClick.AddListener(StartButtonClick);
        }

        public void StartButtonClick()
        {
            _countdownController.StartCountDown();
        }

        public void StartGame()
        {
            _titleScreen.SetActive(false);
            _mainGameUI.SetActive(true);
            _songController.StartSong();
            _started = true;
        }

        public void GameOver()
        {
            _laneFinished.Clear();
            _started = false;
            _startButton.gameObject.SetActive(false);
            _tryAgainButton.gameObject.SetActive(true);
            _titleScreen.SetActive(true);
            _mainGameUI.SetActive(false);
            _songController.Restart();
            ScoreManager.Instance.Restart();
        }

        public void ChangeRightCharacterSprite(KeyCode input)
        {
            _rightCharacterSpriteController.ChangeSprite(input);
        }

        public void AddFinishedLane(Lane lane)
        {
            if (!_laneFinished.Contains(lane))
            {
                _laneFinished.Add(lane);
            }

            if (_laneFinished.Count() >= 4)
            {
                GameOver();
            }
        }
    }
}