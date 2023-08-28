using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*===========================================================================
**
** Interface:  IPoolObject
**
** Purpose: Interface Pool-Objects. Contains callbacks usually invoked on.
** Object-Pooling pipelines, so Pool data-structures will only fabricate
** objects that implement this interface.
**
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/
namespace LGG
{
	public enum PoolObjectEvent { Recycled, Deactivated, Destroyed }

	/// <summary>Event invoked when a IPoolObject event occurs.</summary>
	/// <param name="_poolObject">PoolObject Invoker.</param>
	/// <param name="_event">Type of Event that occured.</param>
	public delegate void OnPoolObjectEvent(IPoolObject _poolObject, PoolObjectEvent _event);

	public interface IPoolObject
	{
		event OnPoolObjectEvent onPoolObjectEvent;

		/// <summary>TEMPORAL, SHH...</summary>
		Transform referenceTransform { get; }

		/// <summary>Is this Pool Object going to be destroyed when changing scene? [By default it destroys it].</summary>
		bool dontDestroyOnLoad { get; set; }
		/// <summary>Is this Pool Object active [preferibaly unavailable to recycle]?.</summary>
		bool active { get; set; }

		/// <summary>Independent Actions made when this Pool Object is being created.</summary>
		void OnObjectCreation();

		/// <summary>Callback invoked when this Pool Object is being recycled.</summary>
		void OnObjectRecycled();

		/// <summary>Callback invoked when the object is deactivated.</summary>
		void OnObjectDeactivation();

		/// <summary>Actions made when this Pool Object is being destroyed.</summary>
		void OnObjectDestruction();

		/// <summary>Invokes onPoolObjectEvent's callback. Use this only to invoke that event [this is used as a public method that would allow another class to invoke Pool-Object's events].</summary>
		/// <param name="_event">Event to invoke.</param>
		void InvokeEvent(PoolObjectEvent _event);
	}
}