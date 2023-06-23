using System;
using System.Collections.Generic;
using System.Linq;
using CaseStudy.Scripts.MusicNightBattle.Configs;
using Melanchall.DryWetMidi.Interaction;
using ModestTree;
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
        private bool finished = false;
        [Inject] private SongController _songController;
        [Inject] private SongConfig _songConfig;
        [Inject] private SignalBus _signalBus;

        public void SetTimeStamp(Melanchall.DryWetMidi.Interaction.Note[] noteArray)
        {
            var landNotes = noteArray.Where(note => note.NoteName == _noteRestriction);
            foreach (var note in landNotes)
            {
                // note.Time depend on the Midi time so need to convert it back to metric time
                var metricTimeSpan =
                    TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.Instance.MidiFile.GetTempoMap());
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
                    if (SongManager.GetAudioSourceTime() >= TimeStamps[_spawnIndex] - _songConfig.NoteTime)
                    {
                        // spawn note
                        var note = Instantiate(NotePrefab, transform).GetComponent<Note>();
                        note.SetUp(_input);
                        _notes.Add(note);
                        note.AssignedTime = (float)TimeStamps[_spawnIndex];
                        _spawnIndex++;
                    }
                }

                if (_inputIndex < TimeStamps.Count)
                {
                    double timeStamp = TimeStamps[_inputIndex];
                    double marginOfError = SongManager.Instance.MarginOfError;
                    double audioTime = SongManager.GetAudioSourceTime() -
                                       (SongManager.Instance.InputDelayInMilliseconds / 1000.0);

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

                else if (transform.childCount == 0 && !finished)
                {
                    finished = true;
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
            finished = false;
            _notes.Clear();
            TimeStamps.Clear();
        }

        public void ProcessButtonInput()
        {
            if (_inputIndex < TimeStamps.Count)
            {
                double timeStamp = TimeStamps[_inputIndex];
                double marginOfError = SongManager.Instance.MarginOfError;
                double audioTime = SongManager.GetAudioSourceTime() -
                                   (SongManager.Instance.InputDelayInMilliseconds / 1000.0);
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