using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless.LevelFlowControl;

namespace Voidless
{
public class LevelController : SceneController
{
	[SerializeField] private LevelFlowData _levelFlowData; 	/// <summary>Level Flow's Data.</summary>
	[SerializeField] private bool[] testEvents; 				/// <summary>Events testing [delete as soon as tests end].</summary>
	[SerializeField] private RootLevelFlowNode _levelFlowRoot; 					/// <summary>Level Flow's Root parent.</summary>

	/// <summary>Gets and Sets levelFlowData property.</summary>
	public LevelFlowData levelFlowData
	{
		get { return _levelFlowData; }
		set { _levelFlowData = value; }
	}

	/// <summary>Gets and Sets levelFlowRoot property.</summary>
	public RootLevelFlowNode levelFlowRoot
	{
		get { return _levelFlowRoot; }
		set { _levelFlowRoot = value; }
	}

#region UnityMethods:
	/// <summary>LevelController's' instance initialization.</summary>
	/*void Awake()
	{
		
	}*/

	/// <summary>LevelController's starting actions before 1st Update frame.</summary>
	void Start ()
	{
		
	}
	
	/// <summary>LevelController's tick at each frame.</summary>
	void Update ()
	{
		
	}
#endregion

#region PublicMethods:
	public void ChangeEvent(int _eventID, bool _success)
	{
		testEvents[_eventID] = _success;
		Debug.Log("[LevelController] Changing Event to: " + _success);
	}

	public void ActivateEvent(int _eventID)
	{
		testEvents[_eventID] = true;
		Debug.Log("[LevelController] Changing Event to: " + true);
	}

	public void DeactivateEvent(int _eventID)
	{
		testEvents[_eventID] = false;
		Debug.Log("[LevelController] Changing Event to: " + false);
	}

	public bool TestCheckEvent(int _eventID)
	{
		Debug.Log("[LevelController] Event " + _eventID + " is: " + testEvents[_eventID]);
		return testEvents[_eventID];
	}
#endregion

#region PrivateMethods:
	/// <summary>Method Invoked when all of the ILoadable objects are loaded.</summary>
	protected override void OnObjectsLoaded()
	{
		//...
	}
#endregion
}
}