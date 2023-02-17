using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Voidless
{
[Serializable]
public class VAssetReferenceT<T> : AssetReferenceT<T>, IEqualityComparer<VAssetReferenceT<T>> where T : UnityEngine.Object
{
	/// <summary>VAssetReferenceT default constructor.</summary>
	public VAssetReferenceT(string guid) : base(guid) { /*...*/ }

	/// <summary>Determines whether the specified objects are equal.</summary>
	/// <param name="a">AssetReference A.</param>
	/// <param name="b">AssetReference B.</param>
	public bool Equals(VAssetReferenceT<T> a, VAssetReferenceT<T> b)
	{
		return a.GetKey() == b.GetKey();
	}

	/// <returns>Returns a hash code for the specified object.</returns>
	public int GetHashCode(VAssetReferenceT<T> reference)
	{
		return reference.RuntimeKey.GetHashCode();
	}

	/// <summary>Checks if another object is equal to this.</summary>
	/// <param name="obj">Object to compare against.</param>
	public override bool Equals(object obj)
	{
		VAssetReferenceT<T> r = obj as VAssetReferenceT<T>;

		return (r == null) ? false : this.GetKey() == r.GetKey();
	}

	/// <returns>Generates a number corresponding to the value of the object to support the use of a hash table.</returns>
	public override int GetHashCode()
	{
		return this.GetKey().GetHashCode();
	}
}
}