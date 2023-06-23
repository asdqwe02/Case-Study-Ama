using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<double> TimeStamps; // in second
        private int _spawnIndex = 0;
        private int _inputIndex = 0;
        private bool _finished = false;
        [Inject] private ISongController _songController;
        [Inject] private SongConfig _songConfig;
        [Inject] private SignalBus _signalBus;
        [Inject] private DiContainer _container;

        private void Awake()
        {
            _signalBus.Subscribe<ReceivedNotesFromMidi>(OnReceivedNotes);
            _signalBus.Subscribe<SongRestartSignal>(OnSongRestart);
        }

        private void OnSongRestart(SongRestartSignal obj)
        {
            Restart();
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
            var landNotes = noteArray.Where(note => note.NoteName == _noteRestriction);
            foreach (var note in landNotes)
            {
                // note.Time depend on the Midi time so need to convert it back to metric time
                var metricTimeSpan =
                    TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, _songController.GetMidiFile().GetTempoMap());
                TimeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds +
                               (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }

        private void Update()
        {
            if (GameManager.Instance.Started)
            {
                if (_spawnIndex < TimeStamps.Count)
                {
                    if (_songController.GetAudioSourceTime() >= TimeStamps[_spawnIndex] - _songConfig.NoteTime)
                    {
                        // spawn note
                        // var note = Instantiate(NotePrefab, transform).GetComponent<Note>();
                        var note = _container.InstantiatePrefab(NotePrefab, transform)
                            .GetComponent<Note>(); // could cause error
                        note.SetUp(_input);
                        _notes.Add(note);
                        note.AssignedTime = (float)TimeStamps[_spawnIndex];
                        _spawnIndex++;
                    }
                }

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

                        GameManager.Instance.ChangeRightCharacterSprite(_input);
                    }

                    if (timeStamp + marginOfError <= audioTime)
                    {
                        Miss();
                        GameManager.Instance.ChangeRightCharacterSprite(KeyCode.None);
                        Debug.Log($"miss position: {_notes[_inputIndex].transform.localPosition}");
                        Debug.Log($"Missed {_inputIndex} note");
                        _inputIndex++;
                    }
                }

                else if (transform.childCount == 0 && !_finished)
                {
                    _finished = true;
                    GameManager.Instance.AddFinishedLane(this);
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

        public void Restart()
        {
            _spawnIndex = 0;
            _inputIndex = 0;
            _finished = false;
            _notes.Clear();
            TimeStamps.Clear();
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

            GameManager.Instance.ChangeRightCharacterSprite(_input);
        }
    }
}