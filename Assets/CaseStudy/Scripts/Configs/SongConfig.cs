using System;
using UnityEngine;

namespace CaseStudy.Scripts.Configs
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
        public Vector2 PerfectHitMargin;
        public float NoteTime; // Time until note hit the tap zone

        public float NoteSpawnY;
        public float NoteTapY;

        public string FilePath;
        public int PlayerOctave;
        public float NoteDespawnY => NoteTapY - (NoteSpawnY - NoteTapY);
    }
}