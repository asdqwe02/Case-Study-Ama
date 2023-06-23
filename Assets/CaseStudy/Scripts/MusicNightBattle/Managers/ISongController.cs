using System.Collections;
using Melanchall.DryWetMidi.Core;
using UnityEngine;

namespace CaseStudy.Scripts.MusicNightBattle.Managers
{
    public interface ISongController
    {
        public void Init(AudioSource audioSource);
        public void StartSong();
        public void Restart();
        void ReadMidiFromFile();
        void GetDataFromMidiFile();
        IEnumerator PlaySongDelay(float delayTime);
        double GetAudioSourceTime();
        public MidiFile GetMidiFile();
    }
}