using System.Collections;

namespace CaseStudy.Scripts.MusicNightBattle
{
    public interface ISongController
    {
        public void StartSong();
        public void Restart();
        void ReadMidiFromFile();
        void GetDataFromMidiFile();
        IEnumerator PlaySongDelay(float delayTime);
        double GetAudioSourceTime();
    }
}