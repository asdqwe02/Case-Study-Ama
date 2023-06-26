using System.Collections.Generic;
using CaseStudy.Scripts.Signals;
using UnityEngine;
using Zenject;

namespace CaseStudy.Scripts.VisualController
{
    public class CountdownController : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _sprites;
        [SerializeField] private List<SpriteRenderer> _countdownSpriteRenderers;

        [Inject] private SignalBus _signalBus;
        private Animator _animator;
        private int _spriteIndex;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _animator.enabled = false;
            foreach (var countdownSpriteRenderer in _countdownSpriteRenderers)
            {
                countdownSpriteRenderer.sprite = _sprites[0];
                countdownSpriteRenderer.enabled = false;
            }
            
            _signalBus.Subscribe<CountDownState>(OnCountDownStateSignal);
        }

        private void OnCountDownStateSignal(CountDownState obj)
        {
            switch (obj)
            {
                case CountDownState.START:
                    StartCountDown();
                    break;
                case CountDownState.FINISH:
                    CountdownFinish();
                    break;
            }
        }
        public void CountdownFinish()
        {
            foreach (var countdownSpriteRenderer in _countdownSpriteRenderers)
            {
                countdownSpriteRenderer.enabled = false;
                countdownSpriteRenderer.sprite = _sprites[0];
            }

            _animator.enabled = false;
            _spriteIndex = 0;
        }

        public void StartCountDown()
        {
            foreach (var countdownSpriteRenderer in _countdownSpriteRenderers)
            {
                countdownSpriteRenderer.enabled = true;
            }

            _animator.enabled = true;
        }

        public void ChangeSprite()
        {
            _spriteIndex++;
            if (_spriteIndex >= _sprites.Count)
            {
                _signalBus.Fire(CountDownState.FINISH);
                return;
            }

            foreach (var spriteRenderer in _countdownSpriteRenderers)
            {
                spriteRenderer.sprite = _sprites[_spriteIndex];
            }
        }
    }
}