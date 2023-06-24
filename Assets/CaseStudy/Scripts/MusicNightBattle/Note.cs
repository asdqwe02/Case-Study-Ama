using System.Collections.Generic;
using CaseStudy.Scenes.MusicNightBattle.Scripts;
using CaseStudy.Scripts.MusicNightBattle.Configs;
using CaseStudy.Scripts.MusicNightBattle.Managers;
using UnityEngine;
using Zenject;

namespace CaseStudy.Scripts.MusicNightBattle
{
    public class Note : MonoBehaviour
    {
        private double _timeInstantiated;
        public double AssignedTime;
        private SpriteRenderer _spriteRenderer;
        [SerializeField] private List<Sprite> _sprites;
        [SerializeField] private float _horizontalScale;
        [Inject] private ISongController _songController;
        [Inject] private SongConfig _songConfig;
        [Inject] private MusicNightBattleLogic _logic;
        private float time = 0;
        private Vector3 _spawnPos = Vector3.zero;
        private Vector3 _destinationPos = Vector3.zero;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetUp(KeyCode input, Vector3 parentPos) // need improvement later
        {
            transform.localPosition = Vector3.zero;
            if (_logic.GetAspect() > 1)
            {
                transform.localScale = Vector3.one * _horizontalScale;
            }

            _spawnPos = parentPos;
            _destinationPos = new Vector3(parentPos.x, _songConfig.NoteDespawnY, 0);
            var index = -1;
            switch (input)
            {
                case KeyCode.LeftArrow:
                    index = 0;
                    break;
                case KeyCode.RightArrow:
                    index = 1;
                    break;
                case KeyCode.UpArrow:
                    index = 2;
                    break;
                case KeyCode.DownArrow:
                    index = 3;
                    break;
            }

            _spriteRenderer.sprite = _sprites[index];
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
            time = t;
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