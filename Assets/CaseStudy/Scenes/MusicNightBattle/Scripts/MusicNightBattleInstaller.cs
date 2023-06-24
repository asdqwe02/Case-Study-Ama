using CaseStudy.Scripts.MusicNightBattle;
using CaseStudy.Scripts.MusicNightBattle.Managers;
using CaseStudy.Scripts.MusicNightBattle.Signals;
using UnityEngine;
using Zenject;

namespace CaseStudy.Scenes.MusicNightBattle
{
    public class MusicNightBattleInstaller : MonoInstaller
    {
        [SerializeField] private CountdownController _countdownController;

        public override void InstallBindings()
        {
            Container.Bind<MusicNightBattleLogic>().AsSingle();
            Container.Bind<ISongController>().To<SongController>().AsSingle();
            Container.BindInstance(_countdownController).AsSingle();

            // Signal
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<ReceivedNotesFromMidi>();
            Container.DeclareSignal<ChangePlayerSpriteSignal>();
            // Container.DeclareSignal<GameOverSignal>();
            Container.DeclareSignal<HitNoteSignal>();
            Container.DeclareSignal<MissNoteSignal>();
            Container.DeclareSignal<LaneFInishSingal>();
            Container.DeclareSignal<UpdateScoreSignal>();
            Container.DeclareSignal<GameState>();
            Container.DeclareSignal<CountDownState>();
        }
    }
}