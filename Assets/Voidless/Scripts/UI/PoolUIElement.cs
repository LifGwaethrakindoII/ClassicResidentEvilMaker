using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class PoolUIElement : UIElement, IPoolObject
{
	public event OnPoolObjectDeactivation onPoolObjectDeactivation; 	/// <summary>Event invoked when this Pool Object is being deactivated.</summary>

	[SerializeField] private bool _dontDestroyOnLoad; 					/// <summary>Is this Pool Object going to be destroyed when changing scene? [By default it destroys it].</summary>
	private bool _active; 												/// <summary>Is this Pool Object active [preferibaly unavailable to recycle]?.</summary>

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
	
#region IPoolObjectMethods:
	/// <summary>Independent Actions made when this Pool Object is being created.</summary>
	public virtual void OnObjectCreation()
	{
		this.DefaultOnCreation();
	}
	
	/// <summary>Actions made when this Pool Object is being reseted.</summary>
	public virtual void OnObjectReset()
	{
		this.DefaultOnRecycle();
	}
	
	/// <summary>Callback invoked when the object is deactivated.</summary>
	public virtual void OnObjectDeactivation()
	{
		this.DefaultOnDeactivation();
		if(onPoolObjectDeactivation != null) onPoolObjectDeactivation(this);
	}
	
	/// <summary>Actions made when this Pool Object is being destroyed.</summary>
	public virtual void OnObjectDestruction()
	{
		if(active && onPoolObjectDeactivation != null) onPoolObjectDeactivation(this);
		active = false;
		this.DefaultOnDestruction();
	}
#endregion
}
}