using System;
using System.Collections.Generic;
using UnityEngine;

namespace CaseStudy.Scripts
{
    public class CharacterSpriteController : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _sprites;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void ChangeSprite(KeyCode input)
        {
            switch (input)
            {
                case KeyCode.RightArrow:
                    _spriteRenderer.sprite = _sprites[1];
                    break;
                case KeyCode.LeftArrow:
                    _spriteRenderer.sprite = _sprites[2];
                    break;
                case KeyCode.UpArrow:
                    _spriteRenderer.sprite = _sprites[3];
                    break;
                case KeyCode.DownArrow:
                    _spriteRenderer.sprite = _sprites[4];
                    break;
                default:
                    _spriteRenderer.sprite = _sprites[0];
                    break;
            }
        }
    }
}