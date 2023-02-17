using UnityEngine;

namespace Voidless
{
public interface IFOVListener
{
	/// <summary>Callback invoked when the subscribed FOV triggers with a GameObject.</summary>
	/// <param name="_object">GameObject the FOV triggered with.</param>
	void OnGameObjectCollision(GameObject _object);
}
}