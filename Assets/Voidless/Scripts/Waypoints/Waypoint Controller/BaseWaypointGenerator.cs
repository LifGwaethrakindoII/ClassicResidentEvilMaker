using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public enum DebugOption 											/// <summary>Debug Option Modes.</summary>
{
	None, 															/// <summary>Null Debug Option.</summary>
	DebugAndModifyOnEditorSceneOnly, 								/// <summary>Debug and Modify on Editor Scene option.</summary>
	DebugAndModifyOnInspectorOnly 									/// <summary>Debug and Modify on Inspector option.</summary>
}

public abstract class BaseWaypointGenerator<W> : MonoBehaviour where W : Waypoint
{
	[Header("Waypoints' Collection Attributes:")]
	[SerializeField] private int _quantity; 						/// <summary>Waypoints' quantity.</summary>
	[SerializeField] private int _priorQuantity; 					/// <summary>Prior quantity [before the quantty change on inspector].</summary>
	[Space(5f)]
	[SerializeField] private W _waypointPrefab; 					/// <summary>WaypointPrefab.</summary>
	[Space(5f)]
	[SerializeField] protected List<W> _waypoints; 					/// <summary>Waypoints.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[SerializeField] private bool _drawWhenSelected; 				/// <summary>Just draw this Gizmo when selected? Otherwise, it will draw in any case.</summary>
	[Space(5f)]
	[SerializeField] private bool _showWaypointsList; 				/// <summary>Show Waypoints' list on Inspector?.</summary>
	[Space(5f)]
	[Header("Override Properties:")]
	[SerializeField] private bool _overrideProperties; 				/// <summary>Override Waypoints' properties?.</summary>
	[SerializeField] private bool _overrideDrawWhenSelected; 		/// <summary>Override Waypoints' drawWhenSelected property.</summary>
	[SerializeField] private DrawTypes _overrideDrawType; 			/// <summary>Overriden draw type property for Waypoints.</summary>
	[SerializeField] private Color _overrideColor; 					/// <summary>Overriden color property for Waypoints.</summary>
	[SerializeField] private Vector3 _overrideDimensions; 			/// <summary>Overriden dimensions property for Waypoints.</summary>
	[SerializeField] private float _overrideNormalProjection; 		/// <summary>Overriden normal projection property for Waypoints.</summary>	
	[Space(5f)]
	[Header("Debug Options:")]
	[SerializeField] private DebugOption _positionsDebugOption; 	/// <summary>Waypoints' positions debug option on the inspector.</summary>
	[SerializeField] private DebugOption _rotationsDebugOption; 	/// <summary>Waypoints' rotations debug option on the inspector.</summary>
	[Space(5f)]
	[Header("GUI Text Attribues:")]
	[SerializeField] private bool _debugGUIText; 					/// <summary>Debug Waypoints on GUI Text?.</summary>
	[SerializeField] private string _textLabel; 					/// <summary>Waypoint's Text label [additional to its index].</summary>
	[SerializeField] private Color _textColor; 						/// <summary>Waypoint's Text Color.</summary>
	[SerializeField] private Vector2 _textOffset; 					/// <summary>Waypoint's Text Offset.</summary>
#endif

#region Getters/Setters:
	/// <summary>Gets and Sets quantity property.</summary>
	public int quantity
	{
		get { return _quantity; }
		set { _quantity = value; }
	}

	/// <summary>Gets and Sets priorQuantity property.</summary>
	public int priorQuantity
	{
		get { return _priorQuantity; }
		set { _priorQuantity = value; }
	}

	/// <summary>Gets and Sets waypointPrefab property.</summary>
	public W waypointPrefab
	{
		get { return _waypointPrefab; }
		set { _waypointPrefab = value; }
	}

	/// <summary>Gets and Sets waypoints property.</summary>
	public virtual List<W> waypoints
	{
		get { return _waypoints; }
		set { _waypoints = value; }
	}

	/// <summary>Gets first property.</summary>
	public W first { get { return waypoints[0]; } }
#endregion

#if UNITY_EDITOR
	/// <summary>Gets and Sets drawWhenSelected property.</summary>
	public bool drawWhenSelected
	{
		get { return _drawWhenSelected; }
		set { _drawWhenSelected = value; }
	}

	/// <summary>Gets and Sets showWaypointsList property.</summary>
	public bool showWaypointsList
	{
		get { return _showWaypointsList; }
		set { _showWaypointsList = value; }
	}

	/// <summary>Gets and Sets overrideProperties property.</summary>
	public bool overrideProperties
	{
		get { return _overrideProperties; }
		set { _overrideProperties = value; }
	}

	/// <summary>Gets and Sets overrideDrawWhenSelected property.</summary>
	public bool overrideDrawWhenSelected
	{
		get { return _overrideDrawWhenSelected; }
		set { _overrideDrawWhenSelected = value; }
	}

	/// <summary>Gets and Sets overrideDrawType property.</summary>
	public DrawTypes overrideDrawType
	{
		get { return _overrideDrawType; }
		set { _overrideDrawType = value; }
	}

	/// <summary>Gets and Sets overrideColor property.</summary>
	public Color overrideColor
	{
		get { return _overrideColor; }
		set { _overrideColor = value; }
	}

	/// <summary>Gets and Sets overrideDimensions property.</summary>
	public Vector3 overrideDimensions
	{
		get { return _overrideDimensions; }
		set { _overrideDimensions = value; }
	}

	/// <summary>Gets and Sets overrideNormalProjection property.</summary>
	public float overrideNormalProjection
	{
		get { return _overrideNormalProjection; }
		set { _overrideNormalProjection = value; }
	}

	/// <summary>Gets and Sets positionsDebugOption property.</summary>
	public DebugOption positionsDebugOption
	{
		get { return _positionsDebugOption; }
		set { _positionsDebugOption = value; }
	}

	/// <summary>Gets and Sets rotationsDebugOption property.</summary>
	public DebugOption rotationsDebugOption
	{
		get { return _rotationsDebugOption; }
		set { _rotationsDebugOption = value; }
	}

	/// <summary>Gets and Sets debugGUIText property.</summary>
	public bool debugGUIText
	{
		get { return _debugGUIText; }
		set { _debugGUIText = value; }
	}

	/// <summary>Gets and Sets textLabel property.</summary>
	public string textLabel
	{
		get { return _textLabel; }
		set { _textLabel = value; }
	}

	/// <summary>Gets and Sets textColor property.</summary>
	public Color textColor
	{
		get { return _textColor; }
		set { _textColor = value; }
	}

	/// <summary>Gets and Sets textOffset property.</summary>
	public Vector2 textOffset
	{
		get { return _textOffset; }
		set { _textOffset = value; }
	}
#endif

	/// <summary>Callback called when the script is being atteched to a GameObject.</summary>
	private void Reset()
	{
		waypoints = new List<W>();
#if UNITY_EDITOR
		overrideColor = Color.white;
		textColor = Color.white;
		overrideDimensions = Vector3.one;
		textOffset = Vector2.zero;
		overrideDrawType = DrawTypes.Wired;
		overrideNormalProjection = 1f;
		textLabel = string.Empty;
#endif
	}

#if UNITY_EDITOR
	/// <summary>Draws Gizmos, even if not selected.</summary>
	void OnDrawGizmos()
	{
		if(!drawWhenSelected) DrawGizmos();
	}

	/// <summary>Draws Gizmos only when selected.</summary>
	void OnDrawGizmosSelected()
	{
		if(drawWhenSelected) DrawGizmos();
	}

	/// <summary>Draws Gizmos.</summary>
	protected virtual void DrawGizmos()
	{
		if(waypoints.Count > 0)
		{
			for(int i = 0; i < waypoints.Count; i++)
			{
				if(waypoints[i] != null)
				{
					if(debugGUIText) DrawDebugGUIText(i);
				}
			}
		}
	}

	protected virtual void DrawDebugGUIText(int _index)
	{
		VGizmos.DrawText(textLabel + _index.ToString(), waypoints[_index].position, textOffset, textColor);
	}

	/// <summary>Updates Waypoint's properties.</summary>
	public virtual void UpdateWaypointsProperties()
	{
		for(int i = 0; i < waypoints.Count; i++)
		{
			waypoints[i].drawWhenSelected = overrideDrawWhenSelected;
			waypoints[i].drawType = overrideDrawType;
			waypoints[i].dimensions = overrideDimensions;
			waypoints[i].color = overrideColor;
			waypoints[i].normalProjection = overrideNormalProjection;
		}
	}
#endif

	/// <summary>Updates Waypoint Generator. The quantity comparision against the last state quantity determines whether it will create or remove waypoints.</summary>
	public virtual void UpdateWaypointGenerator()
	{
		int quantityDelta = Mathf.Abs(quantity - priorQuantity);

		if(quantity < priorQuantity)
		{ /// Delete the rest [must be at end of frame].
			StartCoroutine(DestroyAtEndOfFrame(quantityDelta));
		} else if(quantity > priorQuantity)
		{ /// Create the missing ones.
			StartCoroutine(CreateAtEndOfFrame(quantityDelta));
		}

		priorQuantity = quantity;
	}

	/// <summary>Destroys the quantity difference of waypoints at the end of the frame.</summary>
	/// <param name="_quantityDelta">Delta between the quantity and the last state quantity.</param>
	protected virtual IEnumerator DestroyAtEndOfFrame(int _quantityDelta)
	{
		yield return new WaitForEndOfFrame();
		for(int i = (waypoints.Count - 1); i > (waypoints.Count - _quantityDelta - 1); i--)
		{
			DestroyImmediate(waypoints[i].gameObject);
		}
		waypoints.RemoveRange((waypoints.Count - _quantityDelta), _quantityDelta);
		waypoints.TrimExcess();
	}

	/// <summary>Creates the quantity difference of waypoints at the end of the frame.</summary>
	/// <param name="_quantityDelta">Delta between the quantity and the last state quantity.</param>
	protected virtual IEnumerator CreateAtEndOfFrame(int _quantityDelta)
	{
		yield return new WaitForEndOfFrame();
		for(int i = 0; i < _quantityDelta; i++)
		{
			W newWaypoint = Instantiate(waypointPrefab, transform.position, transform.rotation) as W;
			newWaypoint.transform.parent = transform;
			waypoints.Add(newWaypoint);
		}
	}
}
}