using CaseStudy.Scenes.MusicNightBattle.Scripts;
using UnityEngine;
using Zenject;

namespace CaseStudy.Scripts.MusicNightBattle
{
    public class ScaleWithScreenLayout : MonoBehaviour
    {
        [SerializeField] private Vector3 _horizontalScale;
        [Inject] private MusicNightBattleLogic _logic;

        private void Start()
        {
            if (_logic.GetAspect() > 1)
            {
                transform.localScale = _horizontalScale;
            }
        }
    }
}