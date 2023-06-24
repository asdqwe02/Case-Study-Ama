using System.Collections.Generic;
using CaseStudy.Scripts.MusicNightBattle.Signals;
using UnityEngine;
using Zenject;

namespace CaseStudy.Scripts.MusicNightBattle
{
    public class CharacterVisualController : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _sprites;
        [SerializeField] private bool _player;
        private SpriteRenderer _spriteRenderer;
        [Inject] private SignalBus _signalBus;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_player)
            {
                _signalBus.Subscribe<ChangeCharacterSprite>(OnChangeCharacterSprite);
            }
        }

        // change character visual by changing sprite for now 
        // will switch to change animation state when have asset
        private void OnChangeCharacterSprite(ChangeCharacterSprite obj) 
        {
            ChangeSprite(obj.Input);
        }

        public void ChangeSprite(KeyCode input)
        {
            switch (input)
            {
                case KeyCode.RightArrow:
                    _spriteRenderer.sprite = _sprites[3];
                    break;
                case KeyCode.LeftArrow:
                    _spriteRenderer.sprite = _sprites[1];
                    break;
                case KeyCode.UpArrow:
                    _spriteRenderer.sprite = _sprites[4];
                    break;
                case KeyCode.DownArrow:
                    _spriteRenderer.sprite = _sprites[2];
                    break;
                default:
                    _spriteRenderer.sprite = _sprites[0];
                    break;
            }
        }
    }
}