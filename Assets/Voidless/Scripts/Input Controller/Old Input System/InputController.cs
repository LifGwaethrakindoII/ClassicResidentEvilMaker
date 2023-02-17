using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_SWITCH
using nn.hid;
#endif
#if UNITY_N3DS
using UnityEngine.N3DS;
using N3DS = UnityEngine.N3DS;
#endif

using Unity = UnityEngine;

namespace Voidless
{
public enum Axis 															/// <summary>Input's Axes.</summary>
{
	Horizontal, 															/// <summary>Horizontal Axis.</summary>
	LeftHorizontal, 														/// <summary>Left Horizontal Axis.</summary>
	RightHorizontal, 														/// <summary>Right Horizontal Axis.</summary>
	LeftVertical, 															/// <summary>Left Vertical Axis.</summary>
	RightVertical, 															/// <summary>Right Vertical Axis.</summary>
	Vertical, 																/// <summary>Vertical Axis.</summary>
	LeftTrigger, 															/// <summary>Left Trigger Axis.</summary>
	RightTrigger, 															/// <summary>Right Trigger Axis.</summary>
	DPadHorizontal, 														/// <summary>D-Pad's Horizontal Axis.</summary>
	DPadVertical 															/// <summary>D-Pad's Vertical Axis.</summary>
}

public enum InputAxis
{
	Horizontal,
	Vertical,
	LeftHorizontal,
	LeftVertical,
	RightHorizontal,
	RightVertical,
	DPadHorizontal,
	DPadVertical,
	LeftTrigger1,
	LeftTrigger2,
	RightTrigger1,
	RightTrigger2,
	Button1,
	Button2,
	Button3,
	Button4,
	Start,
	Select,
	Home
}

public enum InputState 														/// <summary>Input States.</summary>
{
	None, 																	/// <summary>No input state [default].</summary>
	Begins, 																/// <summary>Input begins state.</summary>
	Stays, 																	/// <summary>Input stays state.</summary>
	Ended 																	/// <summary>Input ends state.</summary>
}

#region Events:
/// <summary>Event called when an input is received.</summary>
/// <param name="_inputID">ID of the input that was received.</param>
/// <param name="_state">State of the input received.</param>
public delegate void OnInputReceived(int _inputID, InputState _state);

/// <summary>Event called when the Right Axes changes.</summary>
/// <param name="_xAxis">X Axis.</param>
/// <param name="_yAxis">Y Axis.</param>
public delegate void OnRightAxesChange(float _xAxis, float _yAxis);

/// <summary>Event called when the Left Axes changes.</summary>
/// <param name="_xAxis">X Axis.</param>
/// <param name="_yAxis">Y Axis.</param>
public delegate void OnLeftAxesChange(float _xAxis, float _yAxis);

/// <summary>Event called when the D-Pad Axes changes.</summary>
/// <param name="_xAxis">X Axis.</param>
/// <param name="_yAxis">Y Axis.</param>
public delegate void OnDPadAxesChanges(float _xAxis, float _yAxis);

/// <summary>Event called when the Right Trigger Axis changes.</summary>
/// <param name="_axis">Trigger's axis.</param>
public delegate void OnRightTriggerAxisChange(float _axis);

/// <summary>Event called when the Left Trigger Axis changes.</summary>
/// <param name="_axis">Trigger's axis.</param>
public delegate void OnLeftTriggerAxisChange(float _axis);

/// <summary>Event called when the mouse's axes change.</summary>
/// <param name="_xAxis">X Axis.</param>
/// <param name="_yAxis">Y Axis.</param>
public delegate void OnMouseAxesChange(float _xAxis, float _yAxis);

/// <summary>Event called when the Touch's axis changes.</summary>
/// <param name="_touch">Touch that happened.</param>
/// <param name="_index">Index of the touch.</param>
public delegate void OnTouch(Touch _touch, int _index);

/// <summary>event invoked when a Mouse Button is Press.</summary>
/// <param name="_mouseButtonID">Mouse Button's ID.</param>
/// <param name="_state">State of the Input received.</param>
public delegate void OnMouseInput(int _mouseButtonID, InputState _state);
#endregion

public class InputController : Singleton<InputController>
{
	public static event OnInputReceived onInputReceived; 					/// <summary>OnInputReceived event's delegate.</summary>
	public static event OnRightAxesChange onRightAxesChange; 				/// <summary>OnRightAxesChange event's delegate.</summary>
	public static event OnLeftAxesChange onLeftAxesChange; 					/// <summary>OnLeftAxesChange event's delegate.</summary>
	public static event OnDPadAxesChanges onDPadAxesChanges; 				/// <summary>OnDPadAxesChange event's delegate.</summary>
	public static event OnRightTriggerAxisChange onRightTriggerAxisChange; 	/// <summary>OnRightTriggerAxisChange event's delegate.</summary>
	public static event OnLeftTriggerAxisChange onLeftTriggerAxisChange; 	/// <summary>OnLeftTriggerAxisChange event's delegate.</summary>
	public static event OnMouseAxesChange onMouseAxesChange; 				/// <summary>OnMouseAxesChange event's delegate.</summary>
	public static event OnMouseInput onMouseInput; 							/// <summary>OnMouseInput event's delegate.</summary>
	public static event OnTouch onTouch;                                    /// <summary>OnTouch event's delegate.</summary>

#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || UNITY_XBOXONE || UNITY_WSA_10_0 || UNITY_WSA)
    [Space(5f)]
	[SerializeField] private DetectableControllers _detectableControllers; 	/// <summary>Detectable Controllers on Play Mode.</summary>
#endif
	[Space(5f)]
	[SerializeField] private TextAsset _inputMappingFile; 					/// <summary>Input Mapping's File.</summary>
	[Space(5f)]
	[Header("Click/Touch Input's Data:")]
	[SerializeField] private Camera _camera; 								/// <summary>Input Controller's Camera reference for touch input.</summary>
	[SerializeField] private LayerMask _layerMask; 							/// <summary>Layer Mask.</summary>
	private InputMapping _inputMapping; 									/// <summary>Input's Mapping.</summary>
	private Vector2 _leftAxes; 												/// <summary>Input's Left Axes.</summary>
	private Vector2 _previousLeftAxes; 										/// <summary>Previous Left's Axes.</summary>
	private Vector2 _rightAxes; 											/// <summary>Input's Right Axes.</summary>
	private Vector2 _previousRightAxes; 									/// <summary>Previous Right's Axes.</summary>
#if UNITY_SWITCH
	private NpadState[] _NpadStates; 										/// <summary>NpadStates for theNintendo Switch.</summary>
#endif

#region Getters/Setters:
	/// <summary>Gets and Sets inputMappingFile property.</summary>
	public TextAsset inputMappingFile
	{
		get { return _inputMappingFile; }
		set
		{
			_inputMappingFile = value;
			UpdateInputMapping();
		}
	}

	/// <summary>Gets and Sets camera property.</summary>
	public new Camera camera
	{
		get
		{
			if(_camera == null) _camera = Camera.main;
			return _camera;
		}
		set { _camera = value; }
	}

#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || UNITY_XBOXONE || UNITY_WSA_10_0 || UNITY_WSA)
	/// <summary>Gets and Sets detectableControllers property.</summary>
	public DetectableControllers detectableControllers
	{
		get { return _detectableControllers; }
		set { _detectableControllers = value; }
	}
#endif

	/// <summary>Gets and Sets inputMapping property.</summary>
	public InputMapping inputMapping
	{
		get { return _inputMapping; }
		set { _inputMapping = value; }
	}

	/// <summary>Gets and Sets leftAxes property.</summary>
	public Vector2 leftAxes
	{
		get { return _leftAxes; }
		set { _leftAxes = value; }
	}

	/// <summary>Gets and Sets previousLeftAxes property.</summary>
	public Vector2 previousLeftAxes
	{
		get { return _previousLeftAxes; }
		set { _previousLeftAxes = value; }
	}

	/// <summary>Gets and Sets rightAxes property.</summary>
	public Vector2 rightAxes
	{
		get { return _rightAxes; }
		set { _rightAxes = value; }
	}

	/// <summary>Gets and Sets previousRightAxes property.</summary>
	public Vector2 previousRightAxes
	{
		get { return _previousRightAxes; }
		set { _previousRightAxes = value; }
	}
#endregion

#if UNITY_SWITCH
	/// <summary>Gets and Sets NpadStates property.</summary>
	public NpadState[] NpadStates
	{
		get { return _NpadStates; }
		set { _NpadStates = value; }
	}
#endif

#region UnityMethods:
	/// <summary>InputController's' instance initialization.</summary>
	protected override void OnAwake()
	{
		UpdateInputMapping();
	}
	
	/// <summary>InputController's tick at each frame.</summary>
	private void Update()
	{
		TrackInput();
		UpdateAxes();
	}
#endregion

	/// <summary>Updates Input's Mapping by given file.</summary>
	private void UpdateInputMapping()
	{
		inputMapping = VJSONSerializer.DeserializeFromJSONFromTextAsset<InputMapping>(inputMappingFile);
#if UNITY_SWITCH
		NpadStates = new NpadState[] { new NpadState(), new NpadState() };
		Npad.Initialize();
#endif
	}

	/// <summary>Tracks the Input setups depending on the current platform.</summary>
	private void TrackInput()
	{
		if(inputMapping != null)
		{
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || UNITY_XBOXONE || UNITY_WSA_10_0 || UNITY_WSA)
            if (detectableControllers.HasFlag(DetectableControllers.XBox))
			{
				CheckXBoxControllerInputs();
				UpdateXBoxAxesKeyStates();	
			}
			if(detectableControllers.HasFlag(DetectableControllers.Pc)) CheckPCControllerInputs();
#elif (UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE)
			CheckTouchInputs();
#elif UNITY_SWITCH
			UpdateNintendoSwitchControllersInputs();
#elif UNITY_N3DS
			CheckNintendo3DSControllerInputs();
#endif
		}
	}

	/// <summary>Gets Clamped Magnitude of given axes.</summary>
	/// <param name="_axes">Axes to measure the magnitide from.</param>
	/// <returns>Clamped axes' magnitude.</returns>
	public static float GetAxesMagnitude(Vector2 _axes)
	{
		return Mathf.Min(_axes.magnitude, 1.0f);
	}

	/// <summary>Evaluates if input at given index has begun.</summary>
	/// <param name="_inputID">Input's ID.</param>
	/// <returns>True if input at given ID has begun.</returns>
	public static bool InputBegin(int _inputID, int _playerID = 0)
	{
		if(Instance.inputMapping != null)
		{
                //// TEMPORAL REPAIR:
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || UNITY_XBOXONE || UNITY_WSA_10_0 || UNITY_WSA)
            bool result = false;
			
			if(Instance.detectableControllers.HasFlag(DetectableControllers.XBox)) result = Instance.GetXBoxKeyDown(Instance.inputMapping.XBoxControllerSetup.keyMapping[_inputID]);
			if(Instance.detectableControllers.HasFlag(DetectableControllers.Pc)) result = Input.GetKeyDown(Instance.inputMapping.PCControllerSetup.keyMapping[_inputID]);

			return result;
#elif UNITY_SWITCH
			bool result = NintendoSwitchButtonBegin(_inputID, _playerID);
			if(!result) result = NintendoSwitchButtonBegin(_inputID, _playerID + 1);

			return result;
#elif UNITY_N3DS
			return GamePad.GetButtonTrigger(Instance.inputMapping.N3DSControllerSetup.keyMapping[_inputID]);
#endif
		}
		return false;
	}

	/// <summary>Evaluates if input at given index is still active.</summary>
	/// <param name="_inputID">Input's ID.</param>
	/// <returns>True if input at given ID is still active.</returns>
	public static bool InputStay(int _inputID, int _playerID = 0)
	{
		if(Instance.inputMapping != null)
		{
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || UNITY_XBOXONE || UNITY_WSA_10_0 || UNITY_WSA)
            if (Instance.detectableControllers.HasFlag(DetectableControllers.XBox)) return Instance.GetXBoxKey(Instance.inputMapping.XBoxControllerSetup.keyMapping[_inputID]);
			if(Instance.detectableControllers.HasFlag(DetectableControllers.Pc)) return Input.GetKey(Instance.inputMapping.PCControllerSetup.keyMapping[_inputID]);
#elif UNITY_SWITCH
			bool result = NintendoSwitchButtonStay(_inputID, _playerID);
			if(!result) result = NintendoSwitchButtonStay(_inputID, _playerID + 1);

			return result;
#elif UNITY_N3DS
			return GamePad.GetButtonHold(Instance.inputMapping.N3DSControllerSetup.keyMapping[_inputID]);
#endif
		}
		return false;
	}

	/// <summary>Evaluates if input at given index has ended.</summary>
	/// <param name="_inputID">Input's ID.</param>
	/// <returns>True if input at given ID has ended.</returns>
	public static bool InputEnds(int _inputID, int _playerID = 0)
	{
		if(Instance.inputMapping != null)
		{
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || UNITY_XBOXONE || UNITY_WSA_10_0 || UNITY_WSA)
            if (Instance.detectableControllers.HasFlag(DetectableControllers.XBox)) return Instance.GetXBoxKeyUp(Instance.inputMapping.XBoxControllerSetup.keyMapping[_inputID]);
			if(Instance.detectableControllers.HasFlag(DetectableControllers.Pc)) return Input.GetKeyUp(Instance.inputMapping.PCControllerSetup.keyMapping[_inputID]);
#elif UNITY_SWITCH
			bool result = NintendoSwitchButtonEnd(_inputID, _playerID);
			if(!result) result = NintendoSwitchButtonEnd(_inputID, _playerID + 1);

			return result;
#elif UNITY_N3DS
			return GamePad.GetButtonRelease(Instance.inputMapping.N3DSControllerSetup.keyMapping[_inputID]);
#endif
		}
		return false;
	}

	/// <summary>Gets Axis' value.</summary>
	/// <param name="_axis">Axis to evaluate.</param>
	/// <returns>Evaluated axis.</returns>
	public static float GetAxis(Axis _axis)
	{
		switch(_axis)
		{
			case Axis.LeftHorizontal:
			case Axis.Horizontal: 		return Instance.leftAxes.x;
			case Axis.RightHorizontal: 	return Instance.rightAxes.x;

			case Axis.LeftVertical:
			case Axis.Vertical: 		return Instance.leftAxes.y;
			case Axis.RightVertical: 	return Instance.rightAxes.y;

			case Axis.LeftTrigger:
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || UNITY_XBOXONE || UNITY_WSA_10_0 || UNITY_WSA)
		if(Instance.detectableControllers.HasFlag(DetectableControllers.XBox))
		return Instance.inputMapping.XBoxControllerSetup.leftTrigger;
		if(Instance.detectableControllers.HasFlag(DetectableControllers.Pc))
		return Instance.inputMapping.PCControllerSetup.leftTrigger;
#elif (UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE)
			return 0.0f;
#elif UNITY_N3DS
			return Instance.inputMapping.N3DSControllerSetup.leftTrigger;
#endif
 			return 0.0f;

			case Axis.RightTrigger:
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || UNITY_XBOXONE || UNITY_WSA_10_0 || UNITY_WSA)
		if(Instance.detectableControllers.HasFlag(DetectableControllers.XBox))
		return Instance.inputMapping.XBoxControllerSetup.rightTrigger;
		if(Instance.detectableControllers.HasFlag(DetectableControllers.Pc))
		return Instance.inputMapping.PCControllerSetup.rightTrigger;
#elif (UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE)
			return 0.0f;
#elif UNITY_N3DS
			return Instance.inputMapping.N3DSControllerSetup.rightTrigger;
#endif
 			return 0.0f;

			case Axis.DPadHorizontal:
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || UNITY_XBOXONE || UNITY_WSA_10_0 || UNITY_WSA)
		if(Instance.detectableControllers.HasFlag(DetectableControllers.XBox))
		return Instance.inputMapping.XBoxControllerSetup.dPadAxisX;
		if(Instance.detectableControllers.HasFlag(DetectableControllers.Pc))
		return Instance.inputMapping.PCControllerSetup.dPadAxisX;
#elif (UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE)
			return 0.0f;
#elif UNITY_N3DS
			return Instance.inputMapping.N3DSControllerSetup.dPadAxisX;
#endif
 			return 0.0f;

			case Axis.DPadVertical:
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || UNITY_XBOXONE || UNITY_WSA_10_0 || UNITY_WSA)
		if(Instance.detectableControllers.HasFlag(DetectableControllers.XBox))
		return Instance.inputMapping.XBoxControllerSetup.dPadAxisY;
		if(Instance.detectableControllers.HasFlag(DetectableControllers.Pc))
		return Instance.inputMapping.PCControllerSetup.dPadAxisY;
#elif (UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE)
			return 0.0f;
#elif UNITY_N3DS
			return Instance.inputMapping.N3DSControllerSetup.dPadAxisY;
#endif
 			return 0.0f;

		}

		return 0.0f;
	}

	/// <returns>Updates Input's Axes.</returns>
	private void UpdateAxes(int _playerID = 0)
	{
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || UNITY_XBOXONE || UNITY_WSA_10_0 || UNITY_WSA)
		if(detectableControllers.HasFlag(DetectableControllers.XBox))
		{
			_leftAxes.x = inputMapping.XBoxControllerSetup.leftAxisX;
			_leftAxes.y = inputMapping.XBoxControllerSetup.leftAxisY;
			_rightAxes.x = inputMapping.XBoxControllerSetup.rightAxisX;
			_rightAxes.y = inputMapping.XBoxControllerSetup.rightAxisY;
		} else if(detectableControllers.HasFlag(DetectableControllers.Pc))
		{
			_leftAxes.x = inputMapping.PCControllerSetup.leftAxisX;
			_leftAxes.y = inputMapping.PCControllerSetup.leftAxisY;
			_rightAxes = leftAxes;
		}
#elif (UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE)
		_leftAxes.x = 0.0f;
		_leftAxes.y = 0.0f;	
#elif UNITY_SWITCH
		NpadState state = Instance.NpadStates[_playerID];
		NpadId ID = _playerID.ToNpadID();
		NpadStyle style = Npad.GetStyleSet(ID);
		//Npad.GetState(ref state, ID, style);
		AnalogStickState rightAnalogStick = state.analogStickR;
		AnalogStickState leftAnalogStick = state.analogStickL;

		_rightAxes = ID.IDStyleToLeftAxis(new Vector2(rightAnalogStick.fx, rightAnalogStick.fy));
		_leftAxes = ID.IDStyleToLeftAxis(new Vector2(leftAnalogStick.fx, leftAnalogStick.fy));

		if((_rightAxes.sqrMagnitude == 0.0f || _leftAxes.sqrMagnitude == 0.0f) && _playerID == 0) UpdateAxes(_playerID + 1);
#elif UNITY_N3DS
		_leftAxes.x = inputMapping.N3DSControllerSetup.leftAxisX;
		_leftAxes.y = inputMapping.N3DSControllerSetup.leftAxisY;
		_rightAxes.x = inputMapping.N3DSControllerSetup.rightAxisX;
		_rightAxes.y = inputMapping.N3DSControllerSetup.rightAxisY;
#endif

		previousLeftAxes = leftAxes;
		previousRightAxes = rightAxes;
	}

//---------------------------------------
//	 		PC's Methods 				|
//---------------------------------------

	/// <summary>Checks PC controller's mapped inputs.</summary>
	private void CheckPCControllerInputs()
	{
		if(onMouseAxesChange != null) onMouseAxesChange(inputMapping.PCControllerSetup.mouseAxisX, inputMapping.PCControllerSetup.mouseAxisY);
		if(onRightAxesChange != null) onRightAxesChange(inputMapping.PCControllerSetup.rightAxisX, inputMapping.PCControllerSetup.rightAxisY);
		if(onLeftAxesChange != null) onLeftAxesChange(inputMapping.PCControllerSetup.leftAxisX, inputMapping.PCControllerSetup.leftAxisY);
		if(onRightTriggerAxisChange != null) onRightTriggerAxisChange(inputMapping.PCControllerSetup.rightTrigger);
		if(onLeftTriggerAxisChange != null) onLeftTriggerAxisChange(inputMapping.PCControllerSetup.leftTrigger);

		if(onMouseInput != null)
		{
			for(int i = 0; i < PCControllerSetup.MOUSE_INPUTS_LENGTH; i++)
			{
				if(Input.GetMouseButtonDown(i))
					onMouseInput(i, InputState.Begins);
				else if(Input.GetMouseButton(i))
					onMouseInput(i, InputState.Stays);
				else if(Input.GetMouseButtonUp(i))
					onMouseInput(i, InputState.Ended);
			}
		}	

		if(inputMapping.PCControllerSetup.keyMapping.Length > 0 && onInputReceived != null)
		{
			for(int i = 0; i < inputMapping.PCControllerSetup.keyMapping.Length; i++)
			{
				if(Input.GetKeyDown(inputMapping.PCControllerSetup.keyMapping[i]))
					onInputReceived(i, InputState.Begins);
				else if(Input.GetKey(inputMapping.PCControllerSetup.keyMapping[i]))
					onInputReceived(i, InputState.Stays);
				else if(Input.GetKeyUp(inputMapping.PCControllerSetup.keyMapping[i]))
					onInputReceived(i, InputState.Ended);
			}
		}
	}

	/// <summary>Checks if a point hits the screen's PointHitOnViewport [for 3D Mode].</summary>
	/// <param name="_hit">Hit reference to modify.</param>
	/// <param name="_distance">Ray's distance [infinity by default].</param>
	/// <param name="_debug">Debug the ray? false as default.</param>
	public static bool ClickHitOnViewport(out RaycastHit _hit, float _distance = Mathf.Infinity, int _layerMask = Physics.AllLayers, bool _debug = false)
	{
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX)
		Ray ray = Instance.camera.ScreenPointToRay(Input.mousePosition);
		if(_debug) Debug.DrawRay(ray.origin, ray.direction * _distance, Color.blue);
		return Physics.Raycast(ray, out _hit, _distance, _layerMask);
#else
		_hit = default(RaycastHit);
		return false;
#endif  
	}


	/// <summary>Checks if a point hits the screen's PointHitOnViewport [for 2D Mode].</summary>
	/// <param name="_hit">Hit reference to modify.</param>
	/// <param name="_distance">Ray's distance [infinity by default].</param>
	/// <param name="_debug">Debug the ray? false as default.</param>
	public static bool ClickHitOnViewport(out RaycastHit2D _hit, float _distance = Mathf.Infinity, int _layerMask = Physics.AllLayers, bool _debug = false)
	{
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX)
		if(Instance.camera.orthographic)
		{
			Vector3 vector = Instance.camera.ScreenToWorldPoint(Input.mousePosition.WithZ(Instance.camera.nearClipPlane));
			if(_debug) { Debug.DrawLine(vector, Vector3.zero * _distance, Color.blue); }
			_hit = Physics2D.Raycast(vector, Vector2.zero, _distance, _layerMask);
		}
		else
		{
			Ray ray = Instance.camera.ScreenPointToRay(Input.mousePosition);
			_hit = Physics2D.GetRayIntersection(ray, _distance, _layerMask);
		}

		return (_hit.collider != null);
#else
		_hit = default(RaycastHit2D);
		Debug.LogWarning("[InputController] Cannot detect click on this platform.");
		return false;
#endif  
	} 

//---------------------------------------
//	 		X-Box's Methods 			|
//---------------------------------------

	/// <summary>Checks XBox controller's mapped inputs.</summary>
	private void CheckXBoxControllerInputs()
	{
		if(onRightAxesChange != null) onRightAxesChange(inputMapping.XBoxControllerSetup.rightAxisX, inputMapping.XBoxControllerSetup.rightAxisY);
		if(onLeftAxesChange != null) onLeftAxesChange(inputMapping.XBoxControllerSetup.leftAxisX, inputMapping.XBoxControllerSetup.leftAxisY);
		if(onDPadAxesChanges != null) onDPadAxesChanges(inputMapping.XBoxControllerSetup.dPadAxisX, inputMapping.XBoxControllerSetup.dPadAxisY);
		if(onRightTriggerAxisChange != null) onRightTriggerAxisChange(inputMapping.XBoxControllerSetup.rightTrigger);
		if(onLeftTriggerAxisChange != null) onLeftTriggerAxisChange(inputMapping.XBoxControllerSetup.leftTrigger);

		if(inputMapping.XBoxControllerSetup.keyMapping.Length > 0 && onInputReceived != null)
		{
			for(int i = 0; i < inputMapping.XBoxControllerSetup.keyMapping.Length; i++)
			{
				if(GetXBoxKeyDown(inputMapping.XBoxControllerSetup.keyMapping[i]))
					onInputReceived(i, InputState.Begins);
				else if(GetXBoxKey(inputMapping.XBoxControllerSetup.keyMapping[i]))
					onInputReceived(i, InputState.Stays);
				else if(GetXBoxKeyUp(inputMapping.XBoxControllerSetup.keyMapping[i]))
					onInputReceived(i, InputState.Ended);
			}
		}
	}

	/// <summary>Updates XBox's Axes Keys' States.</summary>
	private void UpdateXBoxAxesKeyStates()
	{
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX)
		EvaluateXBoxAxis(inputMapping.XBoxControllerSetup.leftTrigger, ref inputMapping.XBoxControllerSetup.leftTriggerState);
		EvaluateXBoxAxis(inputMapping.XBoxControllerSetup.rightTrigger, ref inputMapping.XBoxControllerSetup.rightTriggerState);
		EvaluateXBoxAxis(inputMapping.XBoxControllerSetup.dPadAxisX, ref inputMapping.XBoxControllerSetup.dPadRightState, ref inputMapping.XBoxControllerSetup.dPadLeftState);
		EvaluateXBoxAxis(inputMapping.XBoxControllerSetup.dPadAxisY, ref inputMapping.XBoxControllerSetup.dPadUpState, ref inputMapping.XBoxControllerSetup.dPadDownState);
#endif	
	}

	/// <summary>Evaluates if XBox's Key state has begun.</summary>
	/// <param name="_key">XBox's Key to evaluate.</param>
	/// <returns>True if provided XBox's key press has begun.</returns>
	public bool GetXBoxKeyDown(XBoxInputKey _key)
	{
		switch(_key)
		{
#if !(UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX || UNITY_XBOXONE || UNITY_WSA_10_0 || UNITY_WSA)	
			case XBoxInputKey.DPadUp: 			return inputMapping.XBoxControllerSetup.dPadUpState == InputState.Begins;
			case XBoxInputKey.DPadDown: 		return inputMapping.XBoxControllerSetup.dPadDownState == InputState.Begins;
			case XBoxInputKey.DPadLeft: 		return inputMapping.XBoxControllerSetup.dPadLeftState == InputState.Begins;
			case XBoxInputKey.DPadRight: 		return inputMapping.XBoxControllerSetup.dPadRightState == InputState.Begins;
#endif
			case XBoxInputKey.LeftTrigger: 		return inputMapping.XBoxControllerSetup.leftTriggerState == InputState.Begins;
			case XBoxInputKey.RightTrigger: 	return inputMapping.XBoxControllerSetup.rightTriggerState == InputState.Begins;
			default: 							return Input.GetKeyDown(_key.ToKeyCode());
		}
	}

	/// <summary>Evaluates if XBox's Key state stays.</summary>
	/// <param name="_key">XBox's Key to evaluate.</param>
	/// <returns>True if provided XBox's key press stays.</returns>
	public bool GetXBoxKey(XBoxInputKey _key)
	{
		switch(_key)
		{
#if !(UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX || UNITY_XBOXONE || UNITY_WSA_10_0 || UNITY_WSA)	
			case XBoxInputKey.DPadUp: 			return inputMapping.XBoxControllerSetup.dPadUpState == InputState.Stays;
			case XBoxInputKey.DPadDown: 		return inputMapping.XBoxControllerSetup.dPadDownState == InputState.Stays;
			case XBoxInputKey.DPadLeft: 		return inputMapping.XBoxControllerSetup.dPadLeftState == InputState.Stays;
			case XBoxInputKey.DPadRight: 		return inputMapping.XBoxControllerSetup.dPadRightState == InputState.Stays;
#endif
			case XBoxInputKey.LeftTrigger: 		return inputMapping.XBoxControllerSetup.leftTriggerState == InputState.Stays;
			case XBoxInputKey.RightTrigger: 	return inputMapping.XBoxControllerSetup.rightTriggerState == InputState.Stays;
			default: 							return Input.GetKey(_key.ToKeyCode());
		}
	}

	/// <summary>Evaluates if XBox's Key state has finished.</summary>
	/// <param name="_key">XBox's Key to evaluate.</param>
	/// <returns>True if provided XBox's key press has finished.</returns>
	public bool GetXBoxKeyUp(XBoxInputKey _key)
	{
		switch(_key)
		{
#if !(UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX || UNITY_XBOXONE || UNITY_WSA_10_0 || UNITY_WSA)	
			case XBoxInputKey.DPadUp: 			return inputMapping.XBoxControllerSetup.dPadUpState == InputState.Ended;
			case XBoxInputKey.DPadDown: 		return inputMapping.XBoxControllerSetup.dPadDownState == InputState.Ended;
			case XBoxInputKey.DPadLeft: 		return inputMapping.XBoxControllerSetup.dPadLeftState == InputState.Ended;
			case XBoxInputKey.DPadRight: 		return inputMapping.XBoxControllerSetup.dPadRightState == InputState.Ended;
#endif
			case XBoxInputKey.LeftTrigger: 		return inputMapping.XBoxControllerSetup.leftTriggerState == InputState.Ended;
			case XBoxInputKey.RightTrigger: 	return inputMapping.XBoxControllerSetup.rightTriggerState == InputState.Ended;
			default: 							return Input.GetKeyUp(_key.ToKeyCode());
		}
	}

	/// <summary>Evaluates XBox's axis.</summary>
	/// <param name="_axis">Axis' Value.</param>
	/// <param name="_inputState">State's reference to modify.</param>
	public void EvaluateXBoxAxis(float _axis, ref InputState _inputState)
	{
		if(_axis != 0.0f)
		{
			switch(_inputState)
			{
				case InputState.None:
				case InputState.Ended:
				_inputState = InputState.Begins;
				break;

				case InputState.Begins:
				default:
				_inputState = InputState.Stays;
				break;
			}

		} else switch(_inputState)
		{
			case InputState.Begins:
			case InputState.Stays:
			_inputState = InputState.Ended;
			break;

			default:
			_inputState = InputState.None;
			break;
		}
	}

	/// <summary>Evaluates XBox's axis.</summary>
	/// <param name="_axis">Axis' Value.</param>
	/// <param name="_positiveAxisInputState">Positive Axis's State reference to modify.</param>
	/// <param name="_negativeAxisInputState">Negative Axis's State reference to modify.</param>
	public void EvaluateXBoxAxis(float _axis, ref InputState _positiveAxisInputState, ref InputState _negativeAxisInputState)
	{
		if(_axis > 0.0f)
		{
			EvaluateXBoxAxis(1.0f, ref _positiveAxisInputState);
			EvaluateXBoxAxis(0.0f, ref _negativeAxisInputState);

		} else if(_axis < 0.0f)
		{
			EvaluateXBoxAxis(0.0f, ref _positiveAxisInputState);
			EvaluateXBoxAxis(1.0f, ref _negativeAxisInputState);

		} else
		{
			EvaluateXBoxAxis(0.0f, ref _positiveAxisInputState);
			EvaluateXBoxAxis(0.0f, ref _negativeAxisInputState);
		}
	}

//---------------------------------------
//	 		Mobile's Methods 			|
//---------------------------------------

	/// <summary>Checks Tactil Platform's touches.</summary>
	private void CheckTouchInputs()
	{
		if(Input.touchCount > 0)
		{
			for(int i = 0; i < Input.touchCount; i++)
			{
				if(onTouch != null) onTouch(Input.touches[i], i);	
			}
		}
	}

	/// <summary>Checks if a touch hits the screen's viewport [for 3D Mode].</summary>
	/// <param name="_touchIndex">Touch's Index.</param>
	/// <param name="_hit">Hit reference to modify.</param>
	/// <param name="_distance">Ray's distance [infinity by default].</param>
	public static bool TouchOnViewport(int _touchIndex, out RaycastHit _hit, float _distance = Mathf.Infinity, int _layerMask = Physics.AllLayers)
	{
#if (UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE)
		Ray ray = Instance.camera.ScreenPointToRay(Input.GetTouch(_touchIndex).position);
		return Physics.Raycast(ray, out _hit, _distance, _layerMask);
#else
		_hit = default(RaycastHit);
		return false;
#endif
	}

	/// <summary>Checks if a touch hits the screen's viewport [for 2D Mode].</summary>
	/// <param name="_touchIndex">Touch's Index.</param>
	/// <param name="_hit">Hit reference to modify.</param>
	/// <param name="_distance">Ray's distance [infinity by default].</param>
	public static bool TouchOnViewport(int _touchIndex, out RaycastHit2D _hit, float _distance = Mathf.Infinity, int _layerMask = Physics.AllLayers)
	{
#if (UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE)
		if(Instance.camera.orthographic)
		{
			Vector3 vector = Instance.camera.ScreenToWorldPoint(Input.GetTouch(_touchIndex).position);
			vector.z = Instance.camera.nearClipPlane;
			_hit = Physics2D.Raycast(vector, Vector2.zero, _distance, _layerMask);
		}
		else
		{
			Ray ray = Instance.camera.ScreenPointToRay(Input.GetTouch(_touchIndex).position);
			_hit = Physics2D.GetRayIntersection(ray, _distance, _layerMask);
		}

		return (_hit.collider != null);
#else
		_hit = default(RaycastHit2D);
		return false;
#endif
	}

//---------------------------------------
//	 		Nintendo Switch's Methods 	|
//---------------------------------------

#if UNITY_SWITCH
	/// <summary>Updates all Nintendo Switch Controllers.</summary>
	private void UpdateNintendoSwitchControllersInputs()
	{
		/*
		- NpadState is a structure that holds the state requested at that moment.
		It has the following properties:
			- Buttons that is pressing on that time.
			- State of both Left and Right analog sticks.
			- Previous buttons that were pressed the last time it was updated.

		- NpadId tells the Player's ID (from 1 to 8, 9th being Handheld).
		- NpadStyle tells which controller mode the current Player is on.
		- Npad.GetState() updates the state of the NpadState's reference passed.
		*/
		NpadState state = default(NpadState);
		NpadId ID = NpadId.Invalid;
		NpadStyle style = NpadStyle.None;

		for(int i = 0; i < NpadStates.Length; i++)
		{
			state = NpadStates[i];
			ID = i.ToNpadID();
			style = Npad.GetStyleSet(ID);
			Npad.GetState(ref state, ID, style);
			NpadStates[i] = state;
		}
	}

	private static bool NintendoSwitchButtonBegin(int _inputID, int _playerID)
	{
		NpadState state = Instance.NpadStates[_playerID];
		NintendoSwitchButton button = Instance.inputMapping.NintendoSwitchControllerSetup.keyMapping[_inputID];
		NpadId ID = _playerID.ToNpadID();
		NpadButton remappedButton = VNintendoSwitch.IDStyleToNpadButton(ID, button);

		return state.GetButtonDown(remappedButton);
		return (state.preButtons | remappedButton) != state.preButtons
		&& (state.buttons | remappedButton) == state.buttons;
	}

	private static bool NintendoSwitchButtonStay(int _inputID, int _playerID)
	{
		NpadState state = Instance.NpadStates[_playerID];
		NintendoSwitchButton button = Instance.inputMapping.NintendoSwitchControllerSetup.keyMapping[_inputID];
		NpadId ID = _playerID.ToNpadID();
		NpadButton remappedButton = VNintendoSwitch.IDStyleToNpadButton(ID, button);

		return state.GetButton(remappedButton);
		return (state.preButtons | remappedButton) == state.preButtons
		&& (state.buttons | remappedButton) == state.buttons;
	}

	private static bool NintendoSwitchButtonEnd(int _inputID, int _playerID)
	{
		NpadState state = Instance.NpadStates[_playerID];
		NintendoSwitchButton button = Instance.inputMapping.NintendoSwitchControllerSetup.keyMapping[_inputID];
		NpadId ID = _playerID.ToNpadID();
		NpadButton remappedButton = VNintendoSwitch.IDStyleToNpadButton(ID, button);

		return state.GetButtonUp(remappedButton);
		return (state.preButtons | remappedButton) == state.preButtons
		&& (state.buttons | remappedButton) != state.buttons;
	}

	/*private static GetNintendoSwitchButtonDown(NpadState _state, NintendoSwitchButton _button,  int _playerID = 0)
	{
		NpadState state = NpadStates[_playerID];
		NpadId ID = _playerID.ToNpadID();
		NpadButton button = 
	}*/
#endif

//---------------------------------------
//	 		Nintendo 3DS's Methods 		|
//---------------------------------------

#if UNITY_N3DS
	/// <summary>Checks Nintendo 3DS controller's mapped inputs.</summary>
	private void CheckNintendo3DSControllerInputs()
	{

		if(onRightAxesChange != null) onRightAxesChange(inputMapping.N3DSControllerSetup.rightAxisX, inputMapping.N3DSControllerSetup.rightAxisY);
		if(onLeftAxesChange != null) onLeftAxesChange(inputMapping.N3DSControllerSetup.leftAxisX, inputMapping.N3DSControllerSetup.leftAxisY);
		if(onDPadAxesChanges != null) onDPadAxesChanges(inputMapping.N3DSControllerSetup.dPadAxisX, inputMapping.N3DSControllerSetup.dPadAxisY);
		if(onRightTriggerAxisChange != null) onRightTriggerAxisChange(inputMapping.N3DSControllerSetup.rightTrigger);
		if(onLeftTriggerAxisChange != null) onLeftTriggerAxisChange(inputMapping.N3DSControllerSetup.leftTrigger);

		if(inputMapping.N3DSControllerSetup.keyMapping.Length > 0 && onInputReceived != null)
		{
			for(int i = 0; i < inputMapping.N3DSControllerSetup.keyMapping.Length; i++)
			{
				if(GamePad.GetButtonTrigger(inputMapping.N3DSControllerSetup.keyMapping[i]))
					onInputReceived(i, InputState.Begins);
				else if(GamePad.GetButtonHold(inputMapping.N3DSControllerSetup.keyMapping[i]))
					onInputReceived(i, InputState.Stays);
				else if(GamePad.GetButtonRelease(inputMapping.N3DSControllerSetup.keyMapping[i]))
					onInputReceived(i, InputState.Ended);
			}
		}
	}
#endif

	/// <returns>String representing this Input's Controller.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("Input's Mapping: ");
		builder.AppendLine(inputMapping.ToString());

		return builder.ToString();
	}
}
}