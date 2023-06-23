using System.Collections;
using CaseStudy.Scripts.MusicNightBattle.Configs;
using CaseStudy.Scripts.MusicNightBattle.Signals;
using GFramework.Runner;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;
using Zenject;

namespace CaseStudy.Scripts.MusicNightBattle.Managers
{
    public class SongController : ISongController
    {
        private MidiFile _midiFile;

        // public MidiFile MidiFile => _midiFile;
        [Inject] private SongConfig _songConfig;
        [Inject] private IRunner _runner;
        [Inject] private SignalBus _signalBus;
        private AudioSource _audioSource;

        public void Init(AudioSource audioSource)
        {
            _audioSource = audioSource;
        }

        public void StartSong()
        {
            ReadMidiFromFile();
        }

        public void Restart()
        {
            _audioSource.Stop();
            _signalBus.Fire<SongRestartSignal>();
        }

        public void ReadMidiFromFile()
        {
            _midiFile = _midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + _songConfig.FilePath);
            GetDataFromMidiFile();
        }

        public MidiFile GetMidiFile()
        {
            return _midiFile;
        }

        public void GetDataFromMidiFile()
        {
            var notes = _midiFile.GetNotes();
            var noteArray = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
            notes.CopyTo(noteArray, 0);
            _signalBus.Fire(new ReceivedNotesFromMidi
            {
                Notes = noteArray
            });
            _runner.StartCoroutine(PlaySongDelay(_songConfig.SongDelayInSeconds));
        }

        public IEnumerator PlaySongDelay(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            _audioSource.Play();
        }

        public double GetAudioSourceTime()
        {
            return (double)_audioSource.timeSamples / _audioSource.clip.frequency;
        }
    }
}