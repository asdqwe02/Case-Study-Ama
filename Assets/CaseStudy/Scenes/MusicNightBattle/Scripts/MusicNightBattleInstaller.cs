using Zenject;

namespace CaseStudy.Scenes.MusicNightBattle
{
    public class MusicNightBattleInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MusicNightBattleLogic>().AsSingle();
        }
    }
   
}