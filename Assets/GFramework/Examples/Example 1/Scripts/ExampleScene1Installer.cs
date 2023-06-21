using GFramework.Examples.Signals;
using Zenject;

namespace GFramework.Examples
{
    public class ExampleScene1Installer : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ExampleScene1Logic>().AsSingle();

            // Need to install the container for signal bus to fire and subscribe signal
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<ExampleSignal>();
        }
    }
}