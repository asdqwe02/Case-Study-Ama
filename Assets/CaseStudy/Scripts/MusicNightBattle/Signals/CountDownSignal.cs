namespace CaseStudy.Scripts.MusicNightBattle.Signals
{
    public class CountDownSignal
    {
        public enum CountDownState
        {
            NONE,
            START,
            FINISH
        }
        public CountDownState State;
    }
}