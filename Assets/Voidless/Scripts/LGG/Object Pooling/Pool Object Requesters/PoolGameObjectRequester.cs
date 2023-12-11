using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/*===========================================================================
**
** Class:  PoolGameObjectRequester
**
** Purpose: A PoolObjectRequester with the generic type T representing a
** PoolGameObject.
**
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/
namespace LGG
{
	public class PoolGameObjectRequester : PoolObjectRequester<PoolGameObject>
	{
		/// <summary>This function is called when the object becomes enabled and active.</summary>
        protected override void OnEnable()
    	{
    		base.OnEnable();
            GameObjectPoolManager.AddRequester(this);
    	}

        /// <summary>This function is called when the behaviour becomes disabled.</summary>
        protected override void OnDisable()
    	{
    		base.OnDisable();
            GameObjectPoolManager.RemoveRequester(this);
    	}

    	/// <summary>Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.</summary>
        protected override void Start()
    	{
    		base.Start();
    		//GameObjectPoolManager.AddRequester(this);
    	}

        /// <summary>Destroying the attached Behaviour will result in the game or Scene receiving OnDestroy.</summary>
        protected override void OnDestroy()
    	{
    		base.OnDestroy();
    		//GameObjectPoolManager.RemoveRequester(this);
    	}

		/// \TODO Shit's temporal
        /// <returns>Pool-Object's Position.</returns>
        public override Vector3 GetPoolObjectPosition(Vector3 _default = default(Vector3))
        {
        	return poolObject != null ? poolObject.referenceTransform.position : Vector3.zero;
        }

		/// <returns>Retrieved Pool-Object from pool.</returns>
    	public override PoolGameObject RequestPoolObject()
    	{
    		//Debug.Log("[PoolObjectRequester] Requesting, current pool state: " + sharedPool.ToString());
    		return GameObjectPoolManager.GetPool(requestedPoolObject).Recycle(spawnPoint.position, spawnPoint.rotation);
    	}
	}
}