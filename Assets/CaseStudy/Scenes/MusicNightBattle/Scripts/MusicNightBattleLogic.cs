using System;
using System.Collections.Generic;
using CaseStudy.Scripts.MusicNightBattle;
using CaseStudy.Scripts.MusicNightBattle.Configs;
using CaseStudy.Scripts.MusicNightBattle.Signals;
using GFramework.Runner;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using ILogger = GFramework.Logger.ILogger;

namespace CaseStudy.Scenes.MusicNightBattle.Scripts
{
    [UsedImplicitly]
    [Serializable]
    public class MusicNightBattleLogic
    {
        [Inject] private ILogger _logger;
        [Inject] private IRunner _runner;
        [Inject] private SignalBus _signalBus;
        [Inject] private HealthBarConfig _healthBarConfig;
        private bool _started = false;
        public bool Started => _started;
        private List<Lane> _laneFinished = new();
        private int _playerHP;
        private Camera _mainCamera;

        public int PlayerHp
        {
            get => _playerHP;
            set
            {
                _playerHP = value;
                CalculateHPPercent();
            }
        }

        public void Init()
        {
            _signalBus.Subscribe<CountDownState>(OnCountDownStateSignal);
            _signalBus.Subscribe<LaneFinishedSignal>(OnLaneFinished);
            _signalBus.Subscribe<MissNoteSignal>(OnMissNote);
            _signalBus.Subscribe<HitNoteSignal>(OnHitNote);

            PlayerHp = _healthBarConfig.PlayerInitalHP;
            _mainCamera = Camera.main;
        }

        private void OnHitNote(HitNoteSignal obj)
        {
            PlayerHp = Mathf.Clamp(_playerHP + _healthBarConfig.HitIncrease, 0, _healthBarConfig.MaxHP);
        }

        void CalculateHPPercent()
        {
            _signalBus.Fire(new UpdateHPSignal
            {
                PercentRight = (float)_playerHP / _healthBarConfig.MaxHP,
                PercentLeft = (float)(_healthBarConfig.MaxHP - _playerHP) / _healthBarConfig.MaxHP
            });
        }

        private void OnMissNote(MissNoteSignal obj)
        {
            PlayerHp = Mathf.Clamp(_playerHP - _healthBarConfig.MissPenalty, 0, _healthBarConfig.MaxHP);
        }

        private void OnCountDownStateSignal(CountDownState obj)
        {
            if (obj == CountDownState.FINISH)
            {
                Reset();
                _signalBus.Fire(GameState.START);
            }
        }

        private void OnLaneFinished(LaneFinishedSignal obj)
        {
            if (!_laneFinished.Contains(obj.Lane))
            {
                _laneFinished.Add(obj.Lane);
            }

            if (_laneFinished.Count >= 4)
            {
                _logger.Information("Game Over");
                _started = false;
                _signalBus.Fire(GameState.FINISH);
            }
        }

        public void Reset()
        {
            PlayerHp = _healthBarConfig.PlayerInitalHP;
            _laneFinished.Clear();
            _started = true;
        }

        public Vector3 ScreenToWorldPoint(Vector3 screenPos)
        {
            return _mainCamera.ScreenToWorldPoint(screenPos);
        }

        public Vector3 GetLanePosition(RectTransform rectTransform)
        {
            // GOAL: Get x and y screen position of rectTransform and convert them to world space

            /*
                The WIDTH of layout container for the button is the size of the screen WIDTH
                so we can just take the x position from rectTransform anchored position for our x screen position
            */
            var x = rectTransform.anchoredPosition.x;
            /*
                The HEIGHT layout container for the button is NOT the size of the screen HEIGHT
                so we can't just take the y position from rectTransform anchored position
                So to get the y screen position we need 
                    + First get the rectTransform position (middle center anchor) on the root canvas
                    + Second we calculate the y screen position by using the above y position and subtract it by half of the rectTransform y size   
                
                Note: will need to use the absolute value of y of rectTransform position on the root canvas      
           */
            var rootCanvas = rectTransform.transform.root;
            // This position have center middle anchor
            var uiPosOnCanvas =
                rootCanvas.InverseTransformPoint(rectTransform.TransformPoint(rectTransform.transform.position));
            // _logger.Information($"ui pos canvas: {uiPosOnCanvas}");
            // _logger.Information($"{Input.mousePosition}");
            var y = Mathf.Abs(uiPosOnCanvas.y) - rectTransform.rect.size.y / 2;
            // _logger.Information($"screen pos: {x}, {y}");
            var debug = _mainCamera.ScreenToWorldPoint(new Vector3(607.9053f, 215.9418f, 0));
            // _logger.Debug($"{debug}");
            return _mainCamera.ScreenToWorldPoint(new Vector3(x, y, 0));
        }

        public float GetAspect()
        {
            return _mainCamera.aspect;
        }
    }
}