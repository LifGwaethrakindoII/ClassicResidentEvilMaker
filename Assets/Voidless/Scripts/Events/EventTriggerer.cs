using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
/// <summary>Event invoked when an event is triggere.</summary>
/// <param name="_ID">ID of the Event triggered.</param>
public delegate void OnEventTriggered(int _ID);

public class EventTriggerer : MonoBehaviour
{
	public static event OnEventTriggered onEventTriggered; 	/// <summary>OnEventTriggered subscription delegate.</summary>

	[SerializeField] private int _ID; 						/// <summary>Event's ID.</summary>

	/// <summary>Gets and Sets ID property.</summary>
	public int ID
	{
		get { return _ID; }
		set { _ID = value; }
	}

	/// <summary>Triggers event, invoking onEventTriggered internally.</summary>
	public void TriggerEvent()
	{
		if(onEventTriggered != null) onEventTriggered(ID);
	}
}
}