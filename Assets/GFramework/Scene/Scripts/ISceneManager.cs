namespace GFramework.Scene
{
    public interface ISceneManager
    {
        void PushScene(string sceneName, object data = null);
        void PopScene(object data = null);
    }
}