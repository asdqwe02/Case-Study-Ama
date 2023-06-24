using System.Collections.Generic;
using CaseStudy.Scripts.MusicNightBattle.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CaseStudy.Scripts.MusicNightBattle
{
    public class ScoreEffectController : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _sprites;
        private Image _effectImage;
        private Animator _animator;

        [Inject] private SignalBus _signalBus;
        private static readonly int PopEffect = Animator.StringToHash("PopEffect");

        private void Awake()
        {
            _effectImage = GetComponent<Image>();
            _animator = GetComponent<Animator>();

            _signalBus.Subscribe<HitNoteSignal>(OnNoteHit);
            _signalBus.Subscribe<MissNoteSignal>(OnNoteMiss);
        }

        private void OnNoteMiss(MissNoteSignal obj)
        {
            _effectImage.sprite = _sprites[0];
            _animator.SetTrigger(PopEffect);
        }

        private void OnNoteHit(HitNoteSignal obj)
        {
            _effectImage.sprite = _sprites[1];
            _animator.SetTrigger(PopEffect);

        }
    }
}