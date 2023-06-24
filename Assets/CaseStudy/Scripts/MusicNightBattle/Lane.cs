using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CaseStudy.Scenes.MusicNightBattle;
using CaseStudy.Scripts.MusicNightBattle.Configs;
using CaseStudy.Scripts.MusicNightBattle.Managers;
using CaseStudy.Scripts.MusicNightBattle.Signals;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;
using Zenject;

namespace CaseStudy.Scripts.MusicNightBattle
{
    public class Lane : MonoBehaviour
    {
        [SerializeField] private Melanchall.DryWetMidi.MusicTheory.NoteName _noteRestriction;
        [SerializeField] private KeyCode _input;
        public GameObject NotePrefab;
        private List<Note> _notes = new();
        public List<double> TimeStamps; // in second when note will spawn at a particular time in the song
        private int _spawnIndex = 0;
        private int _inputIndex = 0;
        private bool _finished = false;
        [Inject] private ISongController _songController;
        [Inject] private SongConfig _songConfig;
        [Inject] private SignalBus _signalBus;
        [Inject] private DiContainer _container;
        [Inject] private MusicNightBattleLogic _logic;

        private void Awake()
        {
            _signalBus.Subscribe<ReceivedNotesFromMidi>(OnReceivedNotes);
            // _signalBus.Subscribe<GameOverSignal>(OnGameOver);
            _signalBus.Subscribe<GameState>(OnGameStateSignal);
        }

        private void OnGameStateSignal(GameState obj)
        {
            switch (obj)
            {
                case GameState.FINISH:
                    StopAllCoroutines();
                    break;
            }
        }

        // private void OnGameOver(GameOverSignal obj)
        // {
        //     StopAllCoroutines();
        //     // Reset();
        // }


        private void OnReceivedNotes(ReceivedNotesFromMidi obj)
        {
            SetTimeStamp(obj.Notes);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<ReceivedNotesFromMidi>(OnReceivedNotes);
        }

        public void SetTimeStamp(Melanchall.DryWetMidi.Interaction.Note[] noteArray)
        {
            Reset();
            var landNotes = noteArray.Where(note => note.NoteName == _noteRestriction);
            foreach (var note in landNotes)
            {
                // note.Time depend on the Midi time so need to convert it back to metric time
                var metricTimeSpan =
                    TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, _songController.GetMidiFile().GetTempoMap());
                TimeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds +
                               (double)metricTimeSpan.Milliseconds / 1000f);
            }

            StartCoroutine(SpawnNote());
        }

        private void Update()
        {
            if (_logic.Started)
            {
                // input process
                if (_inputIndex < TimeStamps.Count)
                {
                    double timeStamp = TimeStamps[_inputIndex];
                    double marginOfError = _songConfig.MarginOfError;
                    double audioTime = _songController.GetAudioSourceTime() -
                                       (_songConfig.InputDelayInMilliseconds / 1000.0);

                    // Process keyboard input
                    if (Input.GetKeyDown(_input))
                    {
                        if (Math.Abs(audioTime - timeStamp) < marginOfError)
                        {
                            Hit();
                            Debug.Log($"Hit on {_inputIndex} note");
                            Destroy(_notes[_inputIndex].gameObject);
                            _inputIndex++;
                        }
                        else
                        {
                            Debug.Log(
                                $"Hit inaccurate on {_inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                        }

                        _signalBus.Fire(new ChangePlayerSpriteSignal
                        {
                            Input = _input
                        });
                        // GameManager.Instance.ChangeRightCharacterSprite(_input);
                    }

                    if (timeStamp + marginOfError <= audioTime)
                    {
                        Miss();
                        _signalBus.Fire(new ChangePlayerSpriteSignal
                        {
                            Input = KeyCode.None
                        });
                        Debug.Log($"miss position: {_notes[_inputIndex].transform.localPosition}");
                        Debug.Log($"Missed {_inputIndex} note");
                        _inputIndex++;
                    }
                }

                else if (transform.childCount == 0 && !_finished) // very unoptimized
                {
                    _finished = true;
                    StopAllCoroutines();
                    _signalBus.Fire(new LaneFInishSingal
                    {
                        Lane = this
                    });
                    // GameManager.Instance.AddFinishedLane(this);
                }
            }
        }

        private void Miss()
        {
            // miss note implementation
            ScoreManager.Instance.MissSFX();
        }

        private void Hit()
        {
            // hit note implementation
            ScoreManager.Instance.HitSFX();
        }

        public void Reset()
        {
            _spawnIndex = 0;
            _inputIndex = 0;
            _finished = false;
            _notes.Clear();
            TimeStamps.Clear();
        }

        IEnumerator SpawnNote()
        {
            while (_spawnIndex < TimeStamps.Count && !_finished)
            {
                // spawn note
                // var note = Instantiate(NotePrefab, transform).GetComponent<Note>();
                var timeWait = _spawnIndex == 0
                    ? (float)TimeStamps[_spawnIndex] - _songConfig.NoteTime
                    : (float)(TimeStamps[_spawnIndex] - TimeStamps[_spawnIndex - 1]);
                yield return new WaitForSeconds(timeWait);
                Debug.Log("spawn note");
                var note = _container.InstantiatePrefab(NotePrefab, transform)
                    .GetComponent<Note>(); // could cause error
                note.SetUp(_input);
                _notes.Add(note);
                note.AssignedTime = (float)TimeStamps[_spawnIndex];
                _spawnIndex++;
            }
        }

        public void ProcessButtonInput()
        {
            if (_inputIndex < TimeStamps.Count)
            {
                double timeStamp = TimeStamps[_inputIndex];
                double marginOfError = _songConfig.MarginOfError;
                double audioTime = _songController.GetAudioSourceTime() -
                                   (_songConfig.InputDelayInMilliseconds / 1000.0);
                if (Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    Hit();
                    Debug.Log($"Hit on {_inputIndex} note");
                    Destroy(_notes[_inputIndex].gameObject);
                    _inputIndex++;
                }
                else
                {
                    Debug.Log(
                        $"Hit inaccurate on {_inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                }
            }

            _signalBus.Fire(new ChangePlayerSpriteSignal
            {
                Input = _input
            });
            // GameManager.Instance.ChangeRightCharacterSprite(_input);
        }
    }
}