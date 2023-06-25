using CaseStudy.Scripts.MusicNightBattle.Signals;
using UnityEngine;
using Zenject;

namespace CaseStudy.Scripts.MusicNightBattle.VisualController
{
    public class ButtonEffectController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _hitEffect;
        [SerializeField] private KeyCode _input;
        [Inject] private SignalBus _signalBus;

        private void Awake()
        {
            _signalBus.Subscribe<HitNoteSignal>(OnNoteHit);
        }

        private void OnNoteHit(HitNoteSignal obj)
        {
            if (obj.Input == _input)
            {
                _hitEffect.Play();
            }
        }
    }
}