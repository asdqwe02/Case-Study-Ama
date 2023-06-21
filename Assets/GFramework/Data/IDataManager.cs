namespace GFramework.Data
{
    public interface IDataManager
    {
        void Save<T>(string key, T value) where T: class;
        T Get<T>(string key) where T: class;
    }
}