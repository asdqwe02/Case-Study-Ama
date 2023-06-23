using System;
using System.Collections.Generic;
using UnityEngine;

namespace CaseStudy.Scripts.MusicNightBattle
{
    public class Note : MonoBehaviour
    {
        private double _timeInstantiated;
        public double AssignedTime;
        private SpriteRenderer _spriteRenderer;
        [SerializeField] private List<Sprite> _sprites;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetUp(KeyCode input) // need improvement later
        {
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
        }

        private void Start()
        {
            _timeInstantiated = SongManager.GetAudioSourceTime();
        }

        private void Update()
        {
            double timeSinceInstantiated = SongManager.GetAudioSourceTime() - _timeInstantiated;
            float t = (float)(timeSinceInstantiated / (SongManager.Instance.NoteTime * 2));

            if (t > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.localPosition = Vector3.Lerp(Vector3.zero,
                    Vector3.up * SongManager.Instance.NoteDespawnY, t);
                _spriteRenderer.enabled = true;
            }
        }
    }
}