using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

namespace Voidless
{
public class BaseUIInputController : BaseInputController
{
	/*[Space(5f)]
	[Header("UI Input's Attributes")]
	[TabGroup("UI")][SerializeField] private string _UIPointID; 				/// <summary>UI Point's Input's ID.</summary>
	[TabGroup("UI")][SerializeField] private string _UILeftClickID; 			/// <summary>UI Left Click's Input's ID.</summary>
	[TabGroup("UI")][SerializeField] private string _UIMiddleClickID; 			/// <summary>UI Middle Click's Input's ID.</summary>
	[TabGroup("UI")][SerializeField] private string _UIRightClickID; 			/// <summary>UI Right Click's Input's ID.</summary>
	[TabGroup("UI")][SerializeField] private string _UIScrollWheelID; 			/// <summary>UI Scroll Wheel's Input's ID.</summary>
	[TabGroup("UI")][SerializeField] private string _UIMoveID; 					/// <summary>UI Move's Input's ID.</summary>
	[TabGroup("UI")][SerializeField] private string _UISubmitID; 				/// <summary>UI Submit's Input's ID.</summary>
	[TabGroup("UI")][SerializeField] private string _UICancelID; 				/// <summary>UI Cancel's Input's ID.</summary>
	[TabGroup("UI")][SerializeField] private string _UITrackedPositionID; 		/// <summary>UI Tracked Position's Input's ID.</summary>
	[TabGroup("UI")][SerializeField] private string _UITrackedOrientationID; 	/// <summary>UI Tracked Orientation's Input's ID.</summary>
	private InputAction _UIPointAction; 										/// <summary>UI's Point Input's Action.</summary>
	private InputAction _UILeftClickAction; 									/// <summary>UI's Left Click Input's Action.</summary>
	private InputAction _UIMiddleClickAction; 									/// <summary>UI's Middle Click Input's Action.</summary>
	private InputAction _UIRightClickAction; 									/// <summary>UI's Right Click Input's Action.</summary>
	private InputAction _UIScrollWheelAction; 									/// <summary>UI's Scroll Wheel Input's Action.</summary>
	private InputAction _UIMoveAction; 											/// <summary>UI's Move Input's Action.</summary>
	private InputAction _UISubmitAction; 										/// <summary>UI's Submit Input's Action.</summary>
	private InputAction _UICancelAction; 										/// <summary>UI's Cancel Input's Action.</summary>
	private InputAction _UITrackedPositionAction; 								/// <summary>UI's Tracked Position Input's Action.</summary>
	private InputAction _UITrackedOrientationAction; 							/// <summary>UI's Tracked Orientation Input's Action.</summary>

#region Getters/Setters:
	/// <summary>Gets UIPointID property.</summary>
	public string UIPointID { get { return _UIPointID; } }

	/// <summary>Gets UILeftClickID property.</summary>
	public string UILeftClickID { get { return _UILeftClickID; } }

	/// <summary>Gets UIMiddleClickID property.</summary>
	public string UIMiddleClickID { get { return _UIMiddleClickID; } }

	/// <summary>Gets UIRightClickID property.</summary>
	public string UIRightClickID { get { return _UIRightClickID; } }

	/// <summary>Gets UIScrollWheelID property.</summary>
	public string UIScrollWheelID { get { return _UIScrollWheelID; } }

	/// <summary>Gets UIMoveID property.</summary>
	public string UIMoveID { get { return _UIMoveID; } }

	/// <summary>Gets UISubmitID property.</summary>
	public string UISubmitID { get { return _UISubmitID; } }

	/// <summary>Gets UICancelID property.</summary>
	public string UICancelID { get { return _UICancelID; } }

	/// <summary>Gets UITrackedPositionID property.</summary>
	public string UITrackedPositionID { get { return _UITrackedPositionID; } }

	/// <summary>Gets UITrackedOrientationID property.</summary>
	public string UITrackedOrientationID { get { return _UITrackedOrientationID; } }

	/// <summary>Gets and Sets UIPointAction property.</summary>
	public InputAction UIPointAction
	{
		get { return _UIPointAction; }
		protected set { _UIPointAction = value; }
	}

	/// <summary>Gets and Sets UILeftClickAction property.</summary>
	public InputAction UILeftClickAction
	{
		get { return _UILeftClickAction; }
		protected set { _UILeftClickAction = value; }
	}

	/// <summary>Gets and Sets UIMiddleClickAction property.</summary>
	public InputAction UIMiddleClickAction
	{
		get { return _UIMiddleClickAction; }
		protected set { _UIMiddleClickAction = value; }
	}

	/// <summary>Gets and Sets UIRightClickAction property.</summary>
	public InputAction UIRightClickAction
	{
		get { return _UIRightClickAction; }
		protected set { _UIRightClickAction = value; }
	}

	/// <summary>Gets and Sets UIScrollWheelAction property.</summary>
	public InputAction UIScrollWheelAction
	{
		get { return _UIScrollWheelAction; }
		protected set { _UIScrollWheelAction = value; }
	}

	/// <summary>Gets and Sets UIMoveAction property.</summary>
	public InputAction UIMoveAction
	{
		get { return _UIMoveAction; }
		protected set { _UIMoveAction = value; }
	}

	/// <summary>Gets and Sets UISubmitAction property.</summary>
	public InputAction UISubmitAction
	{
		get { return _UISubmitAction; }
		protected set { _UISubmitAction = value; }
	}

	/// <summary>Gets and Sets UICancelAction property.</summary>
	public InputAction UICancelAction
	{
		get { return _UICancelAction; }
		protected set { _UICancelAction = value; }
	}

	/// <summary>Gets and Sets UITrackedPositionAction property.</summary>
	public InputAction UITrackedPositionAction
	{
		get { return _UITrackedPositionAction; }
		protected set { _UITrackedPositionAction = value; }
	}

	/// <summary>Gets and Sets UITrackedOrientationAction property.</summary>
	public InputAction UITrackedOrientationAction
	{
		get { return _UITrackedOrientationAction; }
		protected set { _UITrackedOrientationAction = value; }
	}
#endregion

	/// <summary>Sets Input's Actions.</summary>
	protected override void SetInputActions()
	{
		base.SetInputActions();

		if(!string.IsNullOrEmpty(UIPointID)) UIPointAction = actionMap.FindAction(UIPointID, true);
		if(!string.IsNullOrEmpty(UILeftClickID)) UILeftClickAction = actionMap.FindAction(UILeftClickID, true);
		if(!string.IsNullOrEmpty(UIMiddleClickID)) UIMiddleClickAction = actionMap.FindAction(UIMiddleClickID, true);
		if(!string.IsNullOrEmpty(UIRightClickID)) UIRightClickAction = actionMap.FindAction(UIRightClickID, true);
		if(!string.IsNullOrEmpty(UIScrollWheelID)) UIScrollWheelAction = actionMap.FindAction(UIScrollWheelID, true);
		if(!string.IsNullOrEmpty(UIMoveID)) UIMoveAction = actionMap.FindAction(UIMoveID, true);
		if(!string.IsNullOrEmpty(UISubmitID)) UISubmitAction = actionMap.FindAction(UISubmitID, true);
		if(!string.IsNullOrEmpty(UICancelID)) UICancelAction = actionMap.FindAction(UICancelID, true);
		if(!string.IsNullOrEmpty(UITrackedPositionID)) UITrackedPositionAction = actionMap.FindAction(UITrackedPositionID, true);
		if(!string.IsNullOrEmpty(UITrackedOrientationID)) UITrackedOrientationAction = actionMap.FindAction(UITrackedOrientationID, true);
	}*/
}
}