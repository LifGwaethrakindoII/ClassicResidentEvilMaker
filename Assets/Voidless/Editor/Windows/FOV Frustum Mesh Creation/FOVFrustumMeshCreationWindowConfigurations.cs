using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct FOVFrustumMeshCreationWindowConfigurations
{
	[SerializeField] private FOVData _FOVData; 	/// <summary>FOV's Data.</summary>
	[SerializeField] private string _name; 	/// <summary>Data's Path.</summary>
	[SerializeField] private Color _color; 		/// <summary>Handles' Color.</summary>

	/// <summary>Gets and Sets FOVData property.</summary>
	public FOVData FOVData
	{
		get { return _FOVData; }
		set { _FOVData = value; }
	}

	/// <summary>Gets and Sets name property.</summary>
	public string name
	{
		get { return _name; }
		set { _name = value; }
	}

	/// <summary>Gets and Sets color property.</summary>
	public Color color
	{
		get { return _color; }
		set { _color = value; }
	}

	/// <returns>True if it is default.</returns>
	public bool IsDefault()
	{
		return (!string.IsNullOrEmpty(name) && color != default(Color));
	}
}
}