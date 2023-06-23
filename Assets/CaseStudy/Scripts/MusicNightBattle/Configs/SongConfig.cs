using System;
using Melanchall.DryWetMidi.Core;
using UnityEngine;

namespace CaseStudy.Scripts.MusicNightBattle.Configs
{
    [Serializable]
    [CreateAssetMenu(menuName = "MusicNightBattle/SongConfig", fileName = "SongConfig.asset")]
    public class SongConfig : ScriptableObject
    {
        public AudioClip SongAudioClip;
        public AudioClip BeatSFX;
        public AudioClip MissSFX;
        public float SongDelayInSeconds;
        public int InputDelayInMilliseconds;
        public float MarginOfError;
        public float NoteTime;
        
        public float NoteSpawnY;
        public float NoteTapY;
        
        public string FilePath;

        // public float NoteSpawnY;
        // public float NoteTapY;
    }
}