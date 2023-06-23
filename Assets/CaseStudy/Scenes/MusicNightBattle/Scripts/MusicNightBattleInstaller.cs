using CaseStudy.Scripts.MusicNightBattle;
using CaseStudy.Scripts.MusicNightBattle.Signals;
using Zenject;

namespace CaseStudy.Scenes.MusicNightBattle
{
    public class MusicNightBattleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MusicNightBattleLogic>().AsSingle();
            Container.Bind<SongController>().AsSingle();
            
            
            // Signal
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<RecievedNoteFromMidi>();
        }
    }
}