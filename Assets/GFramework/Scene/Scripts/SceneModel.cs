using System;
using Sirenix.OdinInspector;

namespace GFramework.Scene
{
    [Serializable]
    public class SceneModel
    {
        [ReadOnly]
        public string SceneName;
        [ReadOnly]
        public string ScenePath;
        [ReadOnly]
        public string LoadPath;
        [ReadOnly]
        public string DeletePath;

        public override string ToString()
        {
            return SceneName;
        }
    }
}