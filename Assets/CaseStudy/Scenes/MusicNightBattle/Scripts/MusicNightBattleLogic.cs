using System;
using System.Collections;
using System.Collections.Generic;
using CaseStudy.Scripts.MusicNightBattle.Configs;
using CaseStudy.Scripts.MusicNightBattle.GameLogicControllers;
using CaseStudy.Scripts.MusicNightBattle.Signals;
using GFramework.Runner;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
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
        [Inject] private ISongController _songController;
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
            if (obj.IsPlayer)
            {
                PlayerHp = Mathf.Clamp(_playerHP + _healthBarConfig.HitIncrease, 0, _healthBarConfig.MaxHP);
            }
            else
            {
                PlayerHp = Mathf.Clamp(_playerHP - _healthBarConfig.EnemyHitPenalty,
                    _healthBarConfig.PlayerMinHPInEnemyTurn,
                    _healthBarConfig.MaxHP);
            }
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
#if UNITY_WEBGL
                _runner.StartCoroutine(WebGLStart());
#else
                _signalBus.Fire(GameState.START);
                _started = true;
#endif
            }
        }

        IEnumerator WebGLStart()
        {
            yield return _songController.ReadFromWebsite();
            _signalBus.Fire(GameState.START);
            _started = true;
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
            _started = false;
        }

        // Screen to world point function
        public Vector3 GetWorldPosFromCanvasPos(RectTransform rectTransform) // quite heavy calculation
        {
            // GOAL: Get x and y screen position of rectTransform and convert them to world space

            var rootCanvas = rectTransform.transform.root.GetComponent<Canvas>();
            var canvasScaler = rootCanvas.GetComponent<CanvasScaler>();

            /*
                The WIDTH of layout container for the button is the size of the screen WIDTH
                so we can just take the x position from rectTransform anchored position divide by canvas scaler reference x
                then we use that ratio multiply by camera pixel width to get our screen x position
                
                Note: Reference resolution change (get flip) when layout change from horizontal to vertical
            */
            var xRefResolution = _mainCamera.aspect > 1
                ? canvasScaler.referenceResolution.x
                : canvasScaler.referenceResolution.y;
            var xRatio = rectTransform.anchoredPosition.x / xRefResolution;
            // var x = rectTransform.anchoredPosition.x;
            var x = xRatio * _mainCamera.pixelWidth;
            _logger.Information($"camera resolution {_mainCamera.pixelWidth}, {_mainCamera.pixelHeight}");
            /*
                The HEIGHT layout container for the button is NOT the size of the screen HEIGHT
                so we can't just take the y position from rectTransform anchored position
                So to get the y screen position we need 
                    + First get the rectTransform position (middle center anchor) on the root canvas
                    + Second we calculate the y screen position by using the above y position and subtract it by half of the rectTransform y size   
                    + Third we calculate y ratio and multiply it with camera resolution y to get our screen y position
                
                Note: 
                    + will need to use the absolute value of y of rectTransform position on the root canvas      
                    + Reference resolution change (get flip) when layout change from horizontal to vertical
           */
            // This position have center middle anchor
            var uiPosOnCanvas =
                rootCanvas.transform.InverseTransformPoint(
                    rectTransform.TransformPoint(rectTransform.transform.position));
            var yCanvasPos = Mathf.Abs(uiPosOnCanvas.y) - rectTransform.rect.size.y / 2;
            var yRefResolution = _mainCamera.aspect > 1
                ? canvasScaler.referenceResolution.y
                : canvasScaler.referenceResolution.x;
            var yRatio = yCanvasPos / yRefResolution;

            // var y = Mathf.Abs(uiPosOnCanvas.y) - rectTransform.rect.size.y / 2;
            var y = yRatio * _mainCamera.pixelHeight;
            return _mainCamera.ScreenToWorldPoint(new Vector3(x, y, 0));
        }

        public float GetAspect()
        {
            return _mainCamera.aspect;
        }
    }
}