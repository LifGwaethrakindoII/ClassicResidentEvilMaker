using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Voidless
{
[Flags]
public enum SelectionState
{
	Normal = 0,
	Highlighted = 1,
	Pressed = 2,
	Selected = 4,
	Disabled = 8
}

[RequireComponent(typeof(Graphic))]
public class GraphicSelectable : MonoBehaviour, ISelectHandler, IDeselectHandler
{
	[Header("Oscillation Attributes:")]
	[SerializeField] private float _oscillationSpeed; 				/// <summary>Oscillation's Speed.</summary>
	[SerializeField] private SelectionState _oscillateOnStates; 	/// <summary>Selection States where this GameObject may oscillate colors.</summary>
	private float _time; 											/// <summary>Time's argument for oscillation.</summary>
	private SelectionState _selectionStates; 						/// <summary>Selection's State.</summary>
	private Graphic _graphic; 										/// <summary>Graphic's Component.</summary>

	/// <summary>Gets oscillationSpeed property.</summary>
	public float oscillationSpeed { get { return _oscillationSpeed; } }

	/// <summary>Gets and Sets time property.</summary>
	public float time
	{
		get { return _time; }
		private set { _time = value; }
	}

	/// <summary>Gets oscillateOnStates property.</summary>
	public SelectionState oscillateOnStates { get { return _oscillateOnStates; } }

	/// <summary>Gets and Sets selectionStates property.</summary>
	public SelectionState selectionStates
	{
		get { return _selectionStates; }
		set { _selectionStates = value; }
	}

	/// <summary>Gets graphic Component.</summary>
	public Graphic graphic
	{ 
		get
		{
			if(_graphic == null) _graphic = GetComponent<Graphic>();
			return _graphic;
		}
	}

	/// <summary>Callback invoked when GraphicSelectable's instance is enabled.</summary>
	private void OnEnable()
	{
		time = 0.0f;
	}

	/// <summary>Updates GraphicSelectable's instance at each frame.</summary>
	private void Update()
	{
		if((oscillateOnStates | selectionStates) != oscillateOnStates) return;

		Color color = graphic.color;

		time += oscillationSpeed * Time.deltaTime;
		color.a = VMath.RemapValueToNormalizedRange(Mathf.Sin(time), -1.0f, 1.0f);

		graphic.color = color;
	}

	/// <summary>Callback when this object is selected.</summary>
	/// <param name="eventData">Event's Data.</param>
	public void OnSelect(BaseEventData eventData)
	{
		selectionStates |= SelectionState.Selected;
	}

	/// <summary>Called by the EventSystem when a new object is being selected.</summary>
	/// <param name="eventData">Current Event Data.</param>
	public void OnDeselect(BaseEventData eventData)
	{
		selectionStates &= ~SelectionState.Selected;
	}
}
}