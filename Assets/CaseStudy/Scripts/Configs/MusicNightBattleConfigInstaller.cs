using System;
using UnityEngine;
using Zenject;

namespace CaseStudy.Scripts.Configs
{
    [Serializable]
    [CreateAssetMenu(menuName = "MusicNightBattle/ConfigInstaller", fileName = "MusicNightBattleConfigInstaller.asset")]
    public class MusicNightBattleConfigInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private SongConfig _songConfig;
        [SerializeField] private HealthBarConfig _healthBarConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_songConfig);
            Container.BindInstance(_healthBarConfig);
        }
    }
}