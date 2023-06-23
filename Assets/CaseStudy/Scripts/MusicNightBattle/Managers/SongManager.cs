using System;
using System.Collections;
using System.Collections.Generic;
using CaseStudy.DesignPattern;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Unity.VisualScripting;
using UnityEngine;

namespace CaseStudy.Scripts.MusicNightBattle
{
    public class SongManager : MonoSingleton<SongManager>
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private float _songDelayInSeconds;
        [SerializeField] private int _inputDelayInMilliseconds;
        [SerializeField] private float _marginOfError;

        [SerializeField] private string _filePath;
        [SerializeField] private float _noteSpawnY;
        [SerializeField] private float _noteTapY;

        [SerializeField] private List<Lane> _lanes;
        public float NoteTime;

        public float NoteDespawnY => _noteTapY - (_noteSpawnY - _noteTapY);
        public float MarginOfError => _marginOfError;
        public int InputDelayInMilliseconds => _inputDelayInMilliseconds;
        private MidiFile _midiFile;
        public MidiFile MidiFile => _midiFile;
        public double debugSongTime;
        public void StartSong()
        {
            ReadFromFile();
        }

        void ReadFromFile()
        {
            _midiFile = _midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + _filePath);
            GetDataFromMidi();
        }

        private void Update()
        {
            debugSongTime = GetAudioSourceTime();
        }

        public void Restart()
        {
            _audioSource.Stop();
            foreach (var lane in _lanes)
            {
                lane.Restart();
            }
        }

        void GetDataFromMidi()
        {
            var notes = _midiFile.GetNotes();
            var noteArray = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
            notes.CopyTo(noteArray, 0);
            foreach (var lane in _lanes)
            {
                lane.SetTimeStamp(noteArray);
            }

            StartCoroutine(PlaySongDelay(_songDelayInSeconds));
        }

        // async void PlaySongDelay(float delayInSeconds)
        // {
        //     await Task.Delay((int)(delayInSeconds * 1000));
        // }

        IEnumerator PlaySongDelay(float delayInSeconds)
        {
            yield return new WaitForSeconds(delayInSeconds);
            // playsong
            _audioSource.Play();
        }

        public static double GetAudioSourceTime()
        {
            return (double)Instance._audioSource.timeSamples / Instance._audioSource.clip.frequency;
        }
    }
}