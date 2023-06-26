using System.Collections;
using Melanchall.DryWetMidi.Core;
using UnityEngine;

namespace CaseStudy.Scripts.GameLogicControllers
{
    public interface ISongController
    {
        void Init(AudioSource audioSource);
        void StartSong();
        void Restart();
        void ReadMidiFromFile();
        IEnumerator ReadFromWebsite();
        void GetDataFromMidiFile();
        IEnumerator PlaySongDelay(float delayTime);
        double GetAudioSourceTime();
        MidiFile GetMidiFile();
    }
}