using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface ISightSensorListener
{
	/// <summary>Callback invoked when Sight Sensor sees a Collider.</summary>
	/// <param name="_collider">Collider sighted.</param>
	void OnColliderSighted(Collider _collider);

	/// <summary>Callback invoked when a previously seen Collider is occluded.</summary>
	/// <param name="_collider">Collider occluded.</param>
	void OnColliderOccluded(Collider _collider);

	/// <summary>Callback invoked when Sight Sensor stops seeing a previously seen Collider.</summary>
	/// <param name="_collider">Collider not seen anymore.</param>
	void OnColliderOutOfSight(Collider _collider);
}
}