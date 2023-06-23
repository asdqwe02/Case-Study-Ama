using JetBrains.Annotations;
using GFramework.Runner;
using GFramework.Logger;
using Zenject;

namespace CaseStudy.Scenes.MusicNightBattle
{
    [UsedImplicitly]
    public class MusicNightBattleLogic
    {
        [Inject] private ILogger _logger;
        [Inject] private IRunner _runner;
        private bool _started = false;
        public bool Started => _started;
        
    }
}