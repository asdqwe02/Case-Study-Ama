using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CaseStudy.Scenes.MusicNightBattle.Scripts;
using CaseStudy.Scripts.MusicNightBattle.Configs;
using CaseStudy.Scripts.MusicNightBattle.Managers;
using CaseStudy.Scripts.MusicNightBattle.Signals;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using ILogger = GFramework.Logger.ILogger;

namespace CaseStudy.Scripts.MusicNightBattle
{
    public class Lane : MonoBehaviour
    {
        [SerializeField] private Melanchall.DryWetMidi.MusicTheory.NoteName _noteRestriction;
        [SerializeField] private KeyCode _input;
        [SerializeField] private Button _laneInputButton;
        public GameObject NotePrefab;
        private List<Note> _notes = new();
        private List<double> _timeStamps = new(); // in second when note will spawn at a particular time in the song
        private int _spawnIndex = 0;
        private int _inputIndex = 0;
        private bool _finished = false;

        [Inject] private ISongController _songController;
        [Inject] private SongConfig _songConfig;
        [Inject] private SignalBus _signalBus;
        [Inject] private DiContainer _container;
        [Inject] private MusicNightBattleLogic _logic;
        [Inject] private ILogger _logger;

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
                _timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds +
                                (double)metricTimeSpan.Milliseconds / 1000f);
            }

            UpdateLanePosition();

            // StartCoroutine(SpawnNoteCoroutine());
        }

        private void Update()
        {
            if (_logic.Started)
            {
                // spawn process
                SpawnNote();
                // input process
                ProcessKeyboardInput();
            }
        }

        private void Miss()
        {
            // miss note implementation
            _signalBus.Fire<MissNoteSignal>();
        }

        private void Hit()
        {
            // hit note implementation
            // ScoreManager.Instance.HitSFX();
            _signalBus.Fire<HitNoteSignal>();
        }

        public void Reset()
        {
            _spawnIndex = 0;
            _inputIndex = 0;
            _finished = false;
            _notes.Clear();
            _timeStamps.Clear();
        }

        // NOTE: have problem when the game first run the song and the note spawn won't sync together
        IEnumerator SpawnNoteCoroutine()
        {
            while (_spawnIndex < _timeStamps.Count && !_finished)
            {
                // spawn note
                // var note = Instantiate(NotePrefab, transform).GetComponent<Note>();
                var timeWait = _spawnIndex == 0
                    ? (float)_timeStamps[_spawnIndex] - _songConfig.NoteTime
                    : (float)(_timeStamps[_spawnIndex] - _timeStamps[_spawnIndex - 1]);
                yield return new WaitForSeconds(timeWait);
                var note = _container.InstantiatePrefab(NotePrefab, transform)
                    .GetComponent<Note>();
                note.SetUp(_input, transform.position);
                _notes.Add(note);
                note.AssignedTime = (float)_timeStamps[_spawnIndex];
                _spawnIndex++;
            }
        }

        void SpawnNote()
        {
            if (_spawnIndex < _timeStamps.Count)
            {
                if (_songController.GetAudioSourceTime() >= _timeStamps[_spawnIndex] - _songConfig.NoteTime)
                {
                    var note = _container.InstantiatePrefab(NotePrefab, transform)
                        .GetComponent<Note>();
                    note.SetUp(_input, transform.position);
                    _notes.Add(note);
                    note.AssignedTime = (float)_timeStamps[_spawnIndex];
                    _spawnIndex++;
                }
            }
        }

        void ProcessKeyboardInput()
        {
            UpdateLanePosition();
            if (_inputIndex < _timeStamps.Count)
            {
                double timeStamp = _timeStamps[_inputIndex];
                double marginOfError = _songConfig.MarginOfError;
                double audioTime = _songController.GetAudioSourceTime() -
                                   (_songConfig.InputDelayInMilliseconds / 1000.0);

                // Process keyboard input
                if (Input.GetKeyDown(_input))
                {
                    if (Math.Abs(audioTime - timeStamp) < marginOfError)
                    {
                        Hit();
                        _logger.Debug($"Hit on {_inputIndex} note");
                        Destroy(_notes[_inputIndex].gameObject);
                        _inputIndex++;
                    }
                    else
                    {
                        _logger.Debug(
                            $"Hit inaccurate on {_inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                    }

                    _signalBus.Fire(new ChangeCharacterSprite
                    {
                        Input = _input
                    });
                    // GameManager.Instance.ChangeRightCharacterSprite(_input);
                }

                if (timeStamp + marginOfError <= audioTime)
                {
                    Miss();
                    _signalBus.Fire(new ChangeCharacterSprite
                    {
                        Input = KeyCode.None
                    });
                    _logger.Debug($"miss position: {_notes[_inputIndex].transform.localPosition}");
                    _logger.Debug($"Missed {_inputIndex} note");
                    _inputIndex++;
                }
            }

            else if (transform.childCount == 0 && !_finished) // very unoptimized
            {
                _finished = true;
                StopAllCoroutines();
                _signalBus.Fire(new LaneFinishedSignal
                {
                    Lane = this
                });
            }
        }

        public void ProcessButtonInput()
        {
            if (_inputIndex < _timeStamps.Count)
            {
                double timeStamp = _timeStamps[_inputIndex];
                double marginOfError = _songConfig.MarginOfError;
                double audioTime = _songController.GetAudioSourceTime() -
                                   (_songConfig.InputDelayInMilliseconds / 1000.0);
                if (Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    Hit();
                    _logger.Debug($"Hit on {_inputIndex} note");

                    Destroy(_notes[_inputIndex].gameObject);
                    _inputIndex++;
                }
                else
                {
                    _logger.Debug(
                        $"Hit inaccurate on {_inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                }
            }

            _signalBus.Fire(new ChangeCharacterSprite
            {
                Input = _input
            });
            // GameManager.Instance.ChangeRightCharacterSprite(_input);
        }

        void UpdateLanePosition()
        {
            var buttonWorldPos = _logic.GetLanePosition(_laneInputButton.GetComponent<RectTransform>());
            var pos = transform.position;
            pos.x = buttonWorldPos.x;
            transform.position = pos;
            // _logger.Information($"{buttonWorldPos}");
            _songConfig.NoteTapY = buttonWorldPos.y;
            _songConfig.NoteSpawnY = transform.position.y;
        }
    }
}