using System;
using UnityEngine;

namespace Voidless
{
[Serializable]
public class CubemapData
{

#region Properties:
	[SerializeField] private Texture _front; 	/// <summary>Front Cubemap's Face.</summary>
	[SerializeField] private Texture _back; 	/// <summary>Back Cubemap's Face.</summary>
	[SerializeField] private Texture _left; 	/// <summary>Left Cubemap's Face</summary>
	[SerializeField] private Texture _right; 	/// <summary>Right Cubemap's Face.</summary>
	[SerializeField] private Texture _up; 		/// <summary>Up Cubemap's Face.</summary>
	[SerializeField] private Texture _down; 	/// <summary>Down Cubemap's Face.</summary>
#endregion

#region Getters:
	/// <summary>Gets front property.</summary>
	public Texture front { get { return _front; } }

	/// <summary>Gets back property.</summary>
	public Texture back { get { return _back; } }

	/// <summary>Gets left property.</summary>
	public Texture left { get { return _left; } }

	/// <summary>Gets right property.</summary>
	public Texture right { get { return _right; } }

	/// <summary>Gets up property.</summary>
	public Texture up { get { return _up; } }

	/// <summary>Gets down property.</summary>
	public Texture down { get { return _down; } }
#endregion

}
}