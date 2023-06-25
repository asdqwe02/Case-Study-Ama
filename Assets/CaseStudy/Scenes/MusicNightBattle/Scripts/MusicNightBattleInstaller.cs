using CaseStudy.Scripts.MusicNightBattle;
using CaseStudy.Scripts.MusicNightBattle.GameLogicControllers;
using CaseStudy.Scripts.MusicNightBattle.Signals;
using CaseStudy.Scripts.MusicNightBattle.VisualController;
using UnityEngine;
using Zenject;
using Zenject.Internal;

namespace CaseStudy.Scenes.MusicNightBattle.Scripts
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
            Container.DeclareSignal<ChangePlayerSprite>();
            Container.DeclareSignal<HitNoteSignal>();
            Container.DeclareSignal<MissNoteSignal>();
            Container.DeclareSignal<LaneFinishedSignal>();
            Container.DeclareSignal<UpdateScoreSignal>();
            Container.DeclareSignal<UpdateHPSignal>();
            Container.DeclareSignal<UpdateLanePositionSignal>();
            Container.DeclareSignal<ChangeEnemySprite>();
            Container.DeclareSignal<GameState>();
            Container.DeclareSignal<CountDownState>();
        }
    }
}