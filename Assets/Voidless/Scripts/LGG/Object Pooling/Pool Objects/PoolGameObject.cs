using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*===========================================================================
**
** Class:  PoolGameObject
**
** Purpose: Template Pool-Object class that already implements the
** IPoolObject interface with the correct callback functionality.
**
** It also inherits from LGGMonoBehaviour, so the class is ready for
** HashSets and Dictionaries too.
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/
namespace LGG
{
	public class PoolGameObject : LGGMonoBehaviour, IPoolObject
	{
		public event OnPoolObjectEvent onPoolObjectEvent;

		[SerializeField] private Transform _referenceTransform;
		[SerializeField] private bool _dontDestroyOnLoad;
		private bool _active;

		/// <summary>Gets and Sets referenceTransform property.</summary>
		public Transform referenceTransform
		{
			get
			{
				if(_referenceTransform == null) _referenceTransform = transform;
				return _referenceTransform;
			}
			set { _referenceTransform = value; }
		}

		/// <summary>Gets and Sets dontDestroyOnLoad property.</summary>
		public bool dontDestroyOnLoad
		{
			get { return _dontDestroyOnLoad; }
			set { _dontDestroyOnLoad = value; }
		}

		/// <summary>Gets and Sets active property.</summary>
		public bool active
		{
			get { return _active; }
			set { _active = value; }
		}

		/// <summary>Destroying the attached Behaviour will result in the game or Scene receiving OnDestroy.</summary>
	    protected override void OnDestroy()
		{
			InvokeEvent(PoolObjectEvent.Destroyed);
		}
		
#region IPoolObjectMethods:
		/// <summary>Independent Actions made when this Pool Object is being created.</summary>
		public virtual void OnObjectCreation()
		{
			this.DefaultOnCreation();
		}
		
		/// <summary>Actions made when this Pool Object is being recycled.</summary>
		public virtual void OnObjectRecycled()
		{
			this.DefaultOnRecycle();
			InvokeEvent(PoolObjectEvent.Recycled);
		}
		
		/// <summary>Callback invoked when the object is deactivated.</summary>
		public virtual void OnObjectDeactivation()
		{
			this.DefaultOnDeactivation();
			InvokeEvent(PoolObjectEvent.Deactivated);	
		}
		
		/// <summary>Actions made when this Pool Object is being destroyed.</summary>
		public virtual void OnObjectDestruction()
		{
			InvokeEvent(PoolObjectEvent.Destroyed);
			active = false;
			this.DefaultOnDestruction();
		}
		
		/// <summary>Invokes onPoolObjectEvent's callback. Use this only to invoke that event [this is used as a public method that would allow another class to invoke Pool-Object's events].</summary>
		/// <param name="_event">Event to invoke.</param>
		public void InvokeEvent(PoolObjectEvent _event)
		{
			if(onPoolObjectEvent != null) onPoolObjectEvent(this, _event);
		}
#endregion
	}
}