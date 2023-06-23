using GFramework.Scene;
using Zenject;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

// ReSharper disable once CheckNamespace
namespace CaseStudy.Scenes.MusicNightBattle
{
	public class MusicNightBattleController : SceneController
	{
	    [Inject] private MusicNightBattleLogic _logic;
		//This function call when our scene is being loaded
		public override void OnSceneLoaded (object data)
		{
			base.OnSceneLoaded (data);
		}

		//This function call when our scene is being removed/unloaded
		public override void OnSceneUnloaded ()
		{
			base.OnSceneUnloaded ();
		}

		
#if UNITY_EDITOR
		[MenuItem("GFramework/Open Scene/MusicNightBattle")]
		public static void OpenSceneMusicNightBattle()
		{
			if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			{
				EditorSceneManager.OpenScene(@"Assets/CaseStudy/Scenes/MusicNightBattle/Scenes/MusicNightBattle.unity");
			}
		}
#endif
	}
}

