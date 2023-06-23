using System;
using UnityEngine;
using Zenject;

namespace CaseStudy.Scripts.MusicNightBattle.Configs
{
    [Serializable]
    [CreateAssetMenu(menuName = "MusicNightBattle/ConfigInstaller", fileName = "MusicNightBattleConfigInstaller.asset")]
    public class MusicNightBattleConfigInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private SongConfig _songConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_songConfig);
        }
    }
}