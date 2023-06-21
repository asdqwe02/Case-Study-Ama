using GFramework.Data;
using GFramework.GraphQL;
using GFramework.Logger;
using GFramework.Runner;
using GFramework.Scene;
using UnityEngine;
using Zenject;
// using PKFramework.Lidar;
// using PKFramework.SerialPort.Scripts;
using ILogger = GFramework.Logger.ILogger;

namespace GFramework.Core.Installers
{
    public class GInstaller : MonoInstaller
    {
        [SerializeField]
        private GRunner gRunner;
        [SerializeField]
        private GGraphQLCaller gGraphQlCaller;
        [SerializeField] 
        private GSceneManager gSceneManager;
        // [SerializeField] 
        // // private PKLidarManager _pkLidarManager;
        // [SerializeField] 
        // private PKSerialPortManager _pkSerialPortManager;

        public override void InstallBindings()
        {
            Container.Bind<ILogger>().To<GLogger>().AsSingle();
            Container.Bind<ISceneManager>().FromComponentInNewPrefab(gSceneManager).AsSingle().NonLazy();
            Container.Bind<IDataManager>().To<MemoryPackDataManager>().AsSingle();
            Container.Bind<IGraphQLCaller>().FromComponentInNewPrefab(gGraphQlCaller).AsSingle();
            Container.Bind<IRunner>().FromComponentInNewPrefab(gRunner).AsSingle();
// #if PK_USE_LIDAR_MODULE
//             Container.Bind<ILidarManager>().FromComponentInNewPrefab(_pkLidarManager).AsSingle();
// #endif
// #if PK_USE_SERIAL_PORT_MODULE
//             Container.Bind<ISerialPortManager>().FromComponentInNewPrefab(_pkSerialPortManager).AsSingle();
// #endif
        }
    }
}

