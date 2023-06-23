using UnityEngine;

namespace CaseStudy.DesignPattern
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected MonoSingleton()
        {
        }

        private static T _instance;
        private static readonly object _lock = new object();

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (T)Object.FindObjectOfType(typeof(T));

                    if (Object.FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                                       " - there should never be more than 1 singleton!" +
                                       " Reopenning the scene might fix it.");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();

                        Object.DontDestroyOnLoad(singleton);

                        Debug.Log("[Singleton] An instance of " + typeof(T) +
                                  " is needed in the scene, so '" + singleton +
                                  "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        Debug.Log("[Singleton] Using instance already created: " +
                                  _instance.gameObject.name);
                    }
                }

                return _instance;
            }
        }
    }
}