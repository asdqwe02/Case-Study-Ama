using UnityEngine;

namespace GFramework.Scene
{
    public abstract class SceneController: MonoBehaviour
    {
        public virtual void OnSceneLoaded(object data)
        {
            
        }

        public virtual void OnSceneUnloaded()
        {
            
        }
    }
}