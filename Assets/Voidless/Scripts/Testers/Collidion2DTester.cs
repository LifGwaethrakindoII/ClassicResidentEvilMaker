using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class Collidion2DTester : MonoBehaviour
{
	private const string EVENT_COLLISION_ENTER = "Enter"; 	/// <summary>Enter's Collision Type.</summary>
	private const string EVENT_COLLISION_STAY = "Stay"; 	/// <summary>Stay's Collision Type.</summary>
	private const string EVENT_COLLISION_EXIT = "Exit"; 	/// <summary>Exit's Collision Type.</summary>

	[SerializeField] private GameObjectTag[] _tags; 		/// <summary>Tags that are evaluated on collision.</summary>
	private StringBuilder builder; 							/// <summary>StringBuilder used for debug purposes.</summary>

	/// <summary>Gets tags property.</summary>
	public GameObjectTag[] tags { get { return _tags; } }

	/// <summary>Collidion2DTester's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		builder = new StringBuilder();
	}

	/// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision2D data associated with this collision Event.</param>
	private void OnCollisionEnter2D(Collision2D col)
	{
		GameObject obj = col.gameObject;

		if(tags == null) DebugCollision(col, EVENT_COLLISION_ENTER);
		else foreach(GameObjectTag tag in tags)
		{
			if(obj.CompareTag(tag)) DebugCollision(col, EVENT_COLLISION_ENTER);
		}
	}

	/// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision2D data associated with this collision Event.</param>
	private void OnCollisionStay2D(Collision2D col)
	{
		GameObject obj = col.gameObject;

		if(tags == null) DebugCollision(col, EVENT_COLLISION_STAY);
		else foreach(GameObjectTag tag in tags)
		{
			if(obj.CompareTag(tag)) DebugCollision(col, EVENT_COLLISION_STAY);
		}
	}

	/// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision2D data associated with this collision Event.</param>
	private void OnCollisionExit2D(Collision2D col)
	{
		GameObject obj = col.gameObject;

		if(tags == null) DebugCollision(col, EVENT_COLLISION_EXIT);
		else foreach(GameObjectTag tag in tags)
		{
			if(obj.CompareTag(tag)) DebugCollision(col, EVENT_COLLISION_EXIT);
		}
	}

	/// <summary>Debugs Collision.</summary>
	/// <param name="collision">Collision2D.</param>
	/// <param name="_event">Event type.</param>
	private void DebugCollision(Collision2D collision, string _event)
	{
		builder.Clear();

		builder.Append("Collision: { Event: ");
		builder.Append(_event);
		builder.Append(", GameObject: ");
		builder.Append(collision.gameObject.name);
		builder.Append(" }");
#if UNITY_EDITOR
		Debug.Log(builder.ToString());
#endif
	}
}
}