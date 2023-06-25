using CaseStudy.Scenes.MusicNightBattle.Scripts;
using CaseStudy.Scripts.MusicNightBattle.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CaseStudy.Scripts.MusicNightBattle.VisualController
{
    public class HealthBarController : MonoBehaviour
    {
        [SerializeField] private Slider _leftCharacterHPSlider;
        [SerializeField] private Slider _rightCharacterHPSlider;

        [Inject] private SignalBus _signalBus;
        [Inject] private MusicNightBattleLogic _logic;

        private float _rightHPPercent;
        private float _leftHPPercent;

        private void Awake()
        {
            _signalBus.Subscribe<UpdateHPSignal>(UpdateHPSignal);
        }

        private void UpdateHPSignal(UpdateHPSignal obj)
        {
            _rightHPPercent = obj.PercentRight;
            _leftHPPercent = obj.PercentLeft;
        }

        private void LateUpdate()
        {
            // NOTE: not optimize 
            if (_logic.Started)
            {
                var rightSliderValue = _rightCharacterHPSlider.value;
                var leftSliderValue = _leftCharacterHPSlider.value;
                _rightCharacterHPSlider.value = Mathf.Lerp(rightSliderValue, _rightHPPercent, 0.2f);
                _leftCharacterHPSlider.value = Mathf.Lerp(leftSliderValue, _leftHPPercent, 0.2f);
            }
        }
    }
}