using System.Collections.Generic;
using UnityEngine;

/*===========================================================================
**
** Class:  Singleton<T>
**
** Purpose: "Lazy" Singleton class, T must inherit from MonoBehaviour.
**
** NOTE: You must override custom callback OnAwake() if you want to call 
** Awake from your derivate class.
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/
namespace Voidless
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        [Header("Singleton's Attributes:")]
        [SerializeField] private bool _dontDestroyOnLoad;

        /// <summary>Gets and Sets dontDestroyOnLoad property.</summary>
        public bool dontDestroyOnLoad
        {
            get { return _dontDestroyOnLoad; }
            set { _dontDestroyOnLoad = value; }
        }

        /// <summary>Gets and Sets Instance property.</summary>
        public static T Instance
        {
            get { return _instance; }
            protected set { _instance = value; }
        }

        /// <summary>Callback invoked when the MonoBehaviour is instantiated.</summary>
        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);

            } else if(Instance == null)
            {
                Instance = FindObjectOfType(typeof(T)) as T;
                if(dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
                OnAwake();
            }
        }

        /// <summary>Callback called on Awake if this Object is the Singleton's Instance.</summary>
        protected virtual void OnAwake() { /*...*/ }
    }
}