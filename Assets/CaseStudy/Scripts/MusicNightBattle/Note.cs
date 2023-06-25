using System.Collections.Generic;
using CaseStudy.Scenes.MusicNightBattle.Scripts;
using CaseStudy.Scripts.MusicNightBattle.Configs;
using CaseStudy.Scripts.MusicNightBattle.GameLogicControllers;
using UnityEngine;
using Zenject;

namespace CaseStudy.Scripts.MusicNightBattle
{
    public class Note : MonoBehaviour
    {
        private double _timeInstantiated;
        public double AssignedTime;
        private SpriteRenderer _spriteRenderer;
        [SerializeField] private List<Sprite> _playerArrowSprites;
        [SerializeField] private List<Sprite> _enemyArrowSprites;
        [SerializeField] private float _horizontalScale;
        [Inject] private ISongController _songController;
        [Inject] private SongConfig _songConfig;
        [Inject] private MusicNightBattleLogic _logic;
        private Vector3 _spawnPos = Vector3.zero;
        private Vector3 _destinationPos = Vector3.zero;
        private bool _player;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetUp(KeyCode input, Vector3 parentPos, bool isPlayer = true) // need improvement later
        {
            _player = isPlayer;
            transform.localPosition = Vector3.zero;
            if (_logic.GetAspect() > 1)
            {
                transform.localScale = Vector3.one * _horizontalScale;
            }

            _spawnPos = parentPos;
            _destinationPos = new Vector3(parentPos.x, _songConfig.NoteDespawnY, 0);
            var spriteIndex = -1;
            switch (input)
            {
                case KeyCode.LeftArrow:
                    spriteIndex = 0;
                    break;
                case KeyCode.RightArrow:
                    spriteIndex = 1;
                    break;
                case KeyCode.UpArrow:
                    spriteIndex = 2;
                    break;
                case KeyCode.DownArrow:
                    spriteIndex = 3;
                    break;
            }

            _spriteRenderer.sprite = isPlayer ? _playerArrowSprites[spriteIndex] : _enemyArrowSprites[spriteIndex];

            _spriteRenderer.enabled = true;
        }

        private void Start()
        {
            _timeInstantiated = _songController.GetAudioSourceTime();
        }

        private void Update()
        {
            double timeSinceInstantiated = _songController.GetAudioSourceTime() - _timeInstantiated;
            float t = (float)(timeSinceInstantiated / (_songConfig.NoteTime * 2));
            if (t > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.position = Vector3.Lerp(_spawnPos, _destinationPos, t);
                // transform.localPosition =
                // Vector3.Lerp(Vector3.zero, Vector3.up * _songConfig.NoteDespawnY, t);
            }
        }
    }
}