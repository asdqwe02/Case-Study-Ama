using System.Collections.Generic;
using CaseStudy.Scripts.MusicNightBattle.Signals;
using UnityEngine;
using Zenject;
using ILogger = GFramework.Logger.ILogger;

namespace CaseStudy.Scripts.MusicNightBattle.VisualController
{
    public class CharacterVisualController : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _sprites;
        [SerializeField] private bool _isPlayer;
        private SpriteRenderer _spriteRenderer;
        [Inject] private SignalBus _signalBus;
        [Inject] private ILogger _logger;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_isPlayer)
            {
                _signalBus.Subscribe<ChangePlayerSprite>(OnChangePlayerSprite);
            }
            else
            {
                _signalBus.Subscribe<ChangeEnemySprite>(OnChangeEnemySprite);
            }
        }

        private void OnChangeEnemySprite(ChangeEnemySprite obj)
        {
            ChangeSprite(obj.Input);
        }

        // change character visual by changing sprite for now 
        // will switch to change animation state when have asset
        private void OnChangePlayerSprite(ChangePlayerSprite obj)
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