using System.Collections.Generic;
using UnityEngine;

namespace CaseStudy.Scripts.MusicNightBattle
{
    public class CountdownController : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _sprites;
        [SerializeField] private List<SpriteRenderer> _countdownSpriteRenderers;
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
            GameManager.Instance.StartGame();
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
                CountdownFinish();
                return;
            }

            foreach (var spriteRenderer in _countdownSpriteRenderers)
            {
                spriteRenderer.sprite = _sprites[_spriteIndex];
            }
        }
    }
}