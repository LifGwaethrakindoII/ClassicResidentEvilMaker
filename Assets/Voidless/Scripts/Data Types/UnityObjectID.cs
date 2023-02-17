using UnityEngine;

namespace Voidless
{
public struct UnityObjectID
{
	public int _ID; 	/// <summary>Unity Object's ID.</summary>

	/// <summary>Gets and Sets ID property.</summary>
	public int ID
	{
		get { return _ID; }
		private set { _ID = value; }
	}

	/// <summary>Implicit UnityObjectID to int operator.</summary>
	public static implicit operator int(UnityObjectID _object) { return _object.ID; }

	/// <summary>Implicit UnityObjectID to int operator.</summary>
	public static implicit operator UnityObjectID(Object _object) { return new UnityObjectID(_object); }	

	/// <summary>UnityObjectID's Constructor.</summary>
	/// <param name="_object">Unity's Object to get instance ID from.</param>
	public UnityObjectID(Object _object) : this()
	{
		ID = _object.GetInstanceID();
	}
}
}