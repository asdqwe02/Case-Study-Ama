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
    }
}