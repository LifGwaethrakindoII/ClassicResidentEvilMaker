using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[CreateAssetMenu(menuName = VString.PATH_SCRIPTABLE_OBJECTS + " / Sky Dome's Data")]
public class SkyDomeData : ScriptableObject
{

#region Properties:
	[SerializeField] private Vector3 _rotationAxis; 	/// <summary>Sky Dome's rotation axis relative to transfor's direction.</summary>
	[SerializeField] private float _speed; 	/// <summary>Sky Dome's Speed.</summary>
#endregion

#region Getters:
	/// <summary>Gets rotationAxis property.</summary>
	public Vector3 rotationAxis { get { return _rotationAxis; } }

	/// <summary>Gets speed property.</summary>
	public float speed { get { return _speed; } }
#endregion

}
}