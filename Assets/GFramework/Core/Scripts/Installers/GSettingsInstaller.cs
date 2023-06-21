using GFramework.GraphQL;
using GFramework.Logger;
using GFramework.Scene;
using GFramework.Utils;
using UnityEngine;
using Zenject;
// using PKFramework.Lidar;
// using PKFramework.SerialPort.Scripts;

namespace GFramework.Core.Installers
{
    [CreateAssetMenu(fileName = "GSettingsInstaller", menuName = "GFramework/Installers/GSettingsInstaller")]
    public class GSettingsInstaller  : ScriptableObjectInstaller<GSettingsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInstance(ConfigHelper.GetConfig<LogConfig>());
            Container.BindInstance(ConfigHelper.GetConfig<GraphQLConfig>());
            Container.BindInstance(ConfigHelper.GetConfig<SceneConfig>());
// #if PK_USE_LIDAR_MODULE
//             Container.BindInstance(ConfigHelper.GetConfig<LidarConfig>());
// #endif
// #if PK_USE_SERIAL_PORT_MODULE
//             Container.BindInstance(ConfigHelper.GetConfig<SerialPortConfig>());
// #endif
        }
    }

}