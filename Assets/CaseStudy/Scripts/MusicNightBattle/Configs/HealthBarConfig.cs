using System;
using UnityEngine;

namespace CaseStudy.Scripts.MusicNightBattle.Configs
{
    [Serializable]
    [CreateAssetMenu(menuName = "MusicNightBattle/HPConfig", fileName = "HPConfig.asset")]
    public class HealthBarConfig : ScriptableObject
    {
        public int PlayerInitalHP = 10;
        public int MaxHP = 20;
        public int MissPenalty = 2;
        public int HitIncrease = 1;
    }
}