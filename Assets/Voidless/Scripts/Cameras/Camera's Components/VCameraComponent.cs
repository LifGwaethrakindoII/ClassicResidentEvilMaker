using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*============================================================
**
** Class:  VCameraComponent
**
** Purpose: Component that contains a reference to a VCamera's Component.
** Used for all VCamera's functionalities.
**
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/

namespace Voidless
{
[RequireComponent(typeof(VCamera))]
[ExecuteInEditMode]
public abstract class VCameraComponent : MonoBehaviour
{
	private VCamera _vCamera; 	/// <summary>vCamera's Component.</summary>
	
	/// <summary>Gets and Sets vCamera Component.</summary>
	public VCamera vCamera
	{ 
		get
		{
			if(_vCamera == null) _vCamera = GetComponent<VCamera>();
			return _vCamera;
		}
	}

	/// <summary>Method called when this instance is created.</summary>
	protected virtual void Awake() { /*...*/ }
}
}