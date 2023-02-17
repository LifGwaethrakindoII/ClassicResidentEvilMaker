using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Voidless
{
[Serializable]
public class VAssetReference : AssetReference, IEqualityComparer<VAssetReference>
{
	/// <summary>VAssetReference default constructor.</summary>
	public VAssetReference(string guid) : base(guid) { /*...*/ }

	/// <summary>Determines whether the specified objects are equal.</summary>
	/// <param name="a">AssetReference A.</param>
	/// <param name="b">AssetReference B.</param>
	public bool Equals(VAssetReference a, VAssetReference b)
	{
		return a.GetKey() == b.GetKey();
	}

	/// <returns>Returns a hash code for the specified object.</returns>
	public int GetHashCode(VAssetReference reference)
	{
		return reference.RuntimeKey.GetHashCode();
	}

	/// <summary>Checks if another object is equal to this.</summary>
	/// <param name="obj">Object to compare against.</param>
	public override bool Equals(object obj)
	{
		VAssetReference r = obj as VAssetReference;

		return (r == null) ? false : this.GetKey() == r.GetKey();
	}

	/// <returns>True if VAssetReference is valid [not empty].</returns>
	public bool IsValid()
	{
		return RuntimeKeyIsValid();
	}

	/// <returns>Generates a number corresponding to the value of the object to support the use of a hash table.</returns>
	public override int GetHashCode()
	{
		return this.GetKey().GetHashCode();
	}
}
}