using CaseStudy.DesignPattern;
using UnityEngine;

namespace CaseStudy.Scripts.MusicNightBattle
{
    public class ScoreManager : MonoSingleton<ScoreManager>
    {
        [SerializeField] private AudioSource _hitSFX;
        [SerializeField] private AudioSource _missSFX;

        public void MissSFX()
        {
            _missSFX.Play();
        }

        public void HitSFX()
        {
            _hitSFX.Play();
        }
    }
}