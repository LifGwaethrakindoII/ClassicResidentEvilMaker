using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Voidless
{
public abstract class InteractableTriggerZone : TriggerZone<InteractableTriggerZone>
{
	/// <summary>Callback invoked when InteractableTriggerZone's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	private void OnDestroy()
	{
		CharacterInputController.onInputAction -= OnInputAction;
	}

	/// <summary>Callback internally invoked when a GameObject's Collider enters the TriggerZone.</summary>
	/// <param name="_collider">Collider that Enters.</param>
	protected override void OnEnter(Collider _collider)
	{
		base.OnEnter(_collider);
		CharacterInputController.onInputAction += OnInputAction;
	}

	/// <summary>Callback internally invoked when a GameObject's Collider exits the TriggerZone.</summary>
	/// <param name="_collider">Collider that Exits.</param>
	/// <param name="_nextTriggerZone">Next Trigger that ought to be attended.</param>
	protected override void OnExit(Collider _collider, TriggerZone<InteractableTriggerZone> _nextTriggerZone)
	{
		base.OnExit(_collider, _nextTriggerZone);
		CharacterInputController.onInputAction -= OnInputAction;
	}

	/// <summary>Callback invoked when an action is performed.</summary>
	/// <param name="ID">Action's ID.</param>
	/// <param name="state">Input's State.</param>
	protected abstract void OnInputAction(int ID, InputState state);
}
}