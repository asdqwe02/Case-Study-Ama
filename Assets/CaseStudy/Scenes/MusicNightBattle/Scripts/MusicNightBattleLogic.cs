using System;
using System.Collections.Generic;
using CaseStudy.Scripts.MusicNightBattle;
using CaseStudy.Scripts.MusicNightBattle.Signals;
using JetBrains.Annotations;
using GFramework.Runner;
using UnityEngine;
using Zenject;
using ILogger = GFramework.Logger.ILogger;

namespace CaseStudy.Scenes.MusicNightBattle
{
    [UsedImplicitly]
    [Serializable]
    public class MusicNightBattleLogic
    {
        [Inject] private ILogger _logger;
        [Inject] private IRunner _runner;
        [Inject] private SignalBus _signalBus;
        private bool _started = false;
        public bool Started => _started;
        private List<Lane> _laneFinished = new();


        public void Init()
        {
            _signalBus.Subscribe<CountDownSignal>(OnCountDownSignal);
            _signalBus.Subscribe<LaneFInishSingal>(OnLaneFinished);
        }

        private void OnLaneFinished(LaneFInishSingal obj)
        {
            if (!_laneFinished.Contains(obj.Lane))
            {
                _laneFinished.Add(obj.Lane);
            }

            if (_laneFinished.Count >= 4)
            {
                _logger.Information("game over");
                _started = false;
                _signalBus.Fire<GameOverSignal>();
            }
        }

        private void OnCountDownSignal(CountDownSignal obj)
        {
            if (obj.State == CountDownSignal.CountDownState.FINISH)
            {
                Reset();
                _signalBus.Fire<StartGameSIgnal>();
            }
        }

        public void Reset()
        {
            _laneFinished.Clear();
            _started = true;
        }
    }
}