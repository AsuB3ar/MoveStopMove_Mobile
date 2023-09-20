using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilitys
{
    public class Singleton<T> : MonoBehaviour
        where T : Component
    {
        protected static T inst;
        public static T Inst => inst;

        protected virtual void Awake()
        {
            if (inst == null)
            {
                inst = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public class SingletonPersistent<T> : MonoBehaviour
        where T : Component
    {
        protected static T inst;
        public static T Inst => inst;

        protected virtual void Awake()
        {
            if(inst == null)
            {
                inst = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
    }
}