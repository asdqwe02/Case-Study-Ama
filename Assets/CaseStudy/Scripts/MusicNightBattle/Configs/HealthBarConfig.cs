using System;
using UnityEngine;

namespace CaseStudy.Scripts.MusicNightBattle.Configs
{
    [Serializable]
    [CreateAssetMenu(menuName = "MusicNightBattle/HPConfig", fileName = "HPConfig.asset")]
    public class HealthBarConfig : ScriptableObject
    {
        [SerializeField] private int _playerHP = 10;
        [SerializeField] private int _maxHP = 20;
        [SerializeField] private int _missPenalty = 2;
    }
}