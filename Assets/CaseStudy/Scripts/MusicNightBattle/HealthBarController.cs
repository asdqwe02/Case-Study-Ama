using CaseStudy.Scenes.MusicNightBattle;
using CaseStudy.Scripts.MusicNightBattle.Configs;
using CaseStudy.Scripts.MusicNightBattle.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CaseStudy.Scripts.MusicNightBattle
{
    public class HealthBarController : MonoBehaviour
    {
        [SerializeField] private AudioSource _hitSFX;
        [SerializeField] private AudioSource _missSFX;
        [SerializeField] private Slider _leftCharacterHPSlider;
        [SerializeField] private Slider _rightCharacterHPSlider;

        [Inject] private SignalBus _signalBus;
        [Inject] private HealthBarConfig _healthBarConfig;
        [Inject] private MusicNightBattleLogic _logic;

        private void Awake()
        {
            CalculateHP();

            // _signalBus.Subscribe<GameOverSignal>(OnGameOver);
            _signalBus.Subscribe<GameState>(OnGameStateSignal);
            _signalBus.Subscribe<UpdateHPSignal>(UpdateSignal);
        }

        private void UpdateSignal(UpdateHPSignal obj)
        {
            throw new System.NotImplementedException();
        }

        private void OnGameStateSignal(GameState obj)
        {
            // switch (obj)
            // {
            //     case GameState.FINISH:
            //         Reset();
            //         break;
            // }
        }

        private void CalculateHP()
        {
            throw new System.NotImplementedException();
        }

        public void MissSFX()
        {
            _missSFX.Play();
        }

        public void HitSFX()
        {
            _hitSFX.Play();
        }

        private void LateUpdate()
        {
            if (_logic.Started)
            {
                CalculateHP(); // not optimize
            }
        }
    }
}