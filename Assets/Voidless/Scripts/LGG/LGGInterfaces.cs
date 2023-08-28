using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

/*===========================================================================
**
** Class:  LGGInterfaces
**
** Purpose: Extension methods & functions for interfaces defined inside the
** LGG's namespace.
**
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/
namespace LGG
{
    public static class LGGInterfaces
    {
#region IPoolObjectExtensions:
        /// <summary>Default Creation procedure for a MonoBehaviour implementing IPoolObject.</summary>
        /// <param name="_poolObject">Pool Object to extend.</param>
        public static void DefaultOnCreation<T>(this T _poolObject) where T : MonoBehaviour, IPoolObject
        {
            _poolObject.gameObject.SetActive(false);
            _poolObject.active = false;
        }

        /// <summary>Default Recycle procedure for a MonoBehaviour implementing IPoolObject.</summary>
        /// <param name="_poolObject">Pool Object to extend.</param>
        public static void DefaultOnRecycle<T>(this T _poolObject) where T : MonoBehaviour, IPoolObject
        {
            _poolObject.gameObject.SetActive(true);
            _poolObject.active = true;
        }

        /// <summary>Default Deactivation procedure for a MonoBehaviour implementing IPoolObject.</summary>
        /// <param name="_poolObject">Pool Object to extend.</param>
        public static void DefaultOnDeactivation<T>(this T _poolObject) where T : MonoBehaviour, IPoolObject
        {
            _poolObject.gameObject.SetActive(false);
            _poolObject.active = false;
        }

        /// <summary>Default Destruction procedure for a MonoBehaviour implementing IPoolObject.</summary>
        /// <param name="_poolObject">Pool Object to extend.</param>
        public static void DefaultOnDestruction<T>(this T _poolObject) where T : MonoBehaviour, IPoolObject
        {
            _poolObject.DefaultOnDeactivation();
            UnityEngine.Object.Destroy(_poolObject.gameObject);
        }

        /// <summary>Gets a string representing the Pool Object.</summary>
        /// <param name="_poolObject">Pool Object to debug.</param>
        /// <returns>String representing the extended Pool Object.</returns>
        public static string ToString<T>(this T _poolObject) where T : IPoolObject
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("Pool Object: ");
            builder.Append("\n");
            builder.Append("Don't Destroy on Load: ");
            builder.Append(_poolObject.dontDestroyOnLoad.ToString());
            builder.Append("\n");
            builder.Append("Active: ");
            builder.Append(_poolObject.active.ToString());

            return builder.ToString();
        }
#endregion
    }
}