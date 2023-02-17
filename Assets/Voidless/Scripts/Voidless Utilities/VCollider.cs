using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public enum SpatialRelationship
{
	Undefined,
	NonIntersection,
	Intersection,
	Contact,
	AContainsB,
	BContainsA
}

public static class VCollider
{
	public static IEnumerator<Vector3> GetBoundsCornerVertices(this Collider _collider)
	{
		Vector3 position = _collider.transform.position;
		float x = _collider.bounds.extents.x;
		float y = _collider.bounds.extents.y;
		float z = _collider.bounds.extents.z;

		yield return position + new Vector3(x, y, z);
		yield return position + new Vector3(x, -y, z);
		yield return position + new Vector3(x, y, -z);
		yield return position + new Vector3(x, -y, -z);
		yield return position + new Vector3(-x, y, z);
		yield return position + new Vector3(-x, -y, z);
		yield return position + new Vector3(-x, y, -z);
		yield return position + new Vector3(-x, -y, -z);
	}
}
}