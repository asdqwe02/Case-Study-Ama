using CaseStudy.Scenes.MusicNightBattle.Scripts;
using CaseStudy.Scripts.MusicNightBattle.Signals;
using UnityEngine;
using Zenject;

namespace CaseStudy.Scripts.MusicNightBattle
{
    public class ScaleWithScreenLayout : MonoBehaviour
    {
        [SerializeField] private Vector3 _horizontalScale;
        [Inject] private MusicNightBattleLogic _logic;
        [Inject] private SignalBus _signalBus;

        private void Start()
        {
            ScaleObject();
            _signalBus.Subscribe<GameState>(OnGameStateChange);
        }

        private void OnGameStateChange(GameState obj)
        {
            switch (obj)
            {
                case GameState.START:
                    ScaleObject();
                    break;
            }
        }

        void ScaleObject()
        {
            if (_logic.GetAspect() > 1)
            {
                transform.localScale = _horizontalScale;
            }
        }
    }
}