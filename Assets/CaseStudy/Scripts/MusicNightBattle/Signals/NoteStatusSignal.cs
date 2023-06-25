using UnityEngine;

namespace CaseStudy.Scripts.MusicNightBattle.Signals
{
    public enum NoteStatus
    {
        NONE,
        HIT,
        MISS,
        PERFECT_HIT,
    }

    public struct NoteStatusSignal
    {
        public NoteStatus NoteStatus;
        
    }

    public struct HitNoteSignal
    {
        public bool Perfect;
        public KeyCode Input;
    }

    public struct MissNoteSignal
    {
    }
}