using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

namespace Voidless
{
public class BaseInputController : MonoBehaviour
{
	public const int ID_STATE_UI = 1 << 0;

	[InfoBox("@ToString()")]
	[Space(5f)]
	[Header("Axes' Input Actions:")]
	[TabGroup("Main", "Axes")][SerializeField] private InputActionReference _leftAxisXInputReference;
	[TabGroup("Main", "Axes")][SerializeField] private InputActionReference _leftAxisYInputReference;
	[TabGroup("Main", "Axes")][SerializeField] private InputActionReference _rightAxisXInputReference;
	[TabGroup("Main", "Axes")][SerializeField] private InputActionReference _rightAxisYInputReference;
	[Space(5f)]
	[Header("Axes' Settings:")]
	[TabGroup("GamePad", "Axes")][Range(0.0f, 0.9f)][SerializeField] private float _leftDeadZoneRadius;
	[TabGroup("GamePad", "Axes")][Range(0.0f, 0.9f)][SerializeField] private float _rightDeadZoneRadius;
	[Space(5f)]
	[Header("UI Input Actions:")]
	[TabGroup("UI", "UI")][SerializeField] private InputActionReference _UIPointInputReference;
	[TabGroup("UI", "UI")][SerializeField] private InputActionReference _UILeftClickInputReference;
	[TabGroup("UI", "UI")][SerializeField] private InputActionReference _UIMiddleClickInputReference;
	[TabGroup("UI", "UI")][SerializeField] private InputActionReference _UIRightClickInputReference;
	[TabGroup("UI", "UI")][SerializeField] private InputActionReference _UIScrollWheelInputReference;
	[TabGroup("UI", "UI")][SerializeField] private InputActionReference _UIMoveInputReference;
	[TabGroup("UI", "UI")][SerializeField] private InputActionReference _UISubmitInputReference;
	[TabGroup("UI", "UI")][SerializeField] private InputActionReference _UICancelInputReference;
	[TabGroup("UI", "UI")][SerializeField] private InputActionReference _UITrackedPositionInputReference;
	[TabGroup("UI", "UI")][SerializeField] private InputActionReference _UITrackedOrientationInputReference;
	private Vector2 _leftAxes;
	private Vector2 _rightAxes;
	private Vector2 _previousLeftAxes;
	private Vector2 _previousRightAxes;
	private int _stateFlags;
	private int _inputFlags;
	private float _leftAxesMagnitude;
	private float _rightAxesMagnitude;

#region Getters/Setters:
	/// <summary>Gets leftAxisXInputReference property.</summary>
	public InputActionReference leftAxisXInputReference { get { return _leftAxisXInputReference; } }

	/// <summary>Gets leftAxisYInputReference property.</summary>
	public InputActionReference leftAxisYInputReference { get { return _leftAxisYInputReference; } }

	/// <summary>Gets rightAxisXInputReference property.</summary>
	public InputActionReference rightAxisXInputReference { get { return _rightAxisXInputReference; } }

	/// <summary>Gets rightAxisYInputReference property.</summary>
	public InputActionReference rightAxisYInputReference { get { return _rightAxisYInputReference; } }

	public InputActionReference UIPointInputReference { get { return _UIPointInputReference; } }

	/// <summary>Gets UILeftClickInputReference property.</summary>
	public InputActionReference UILeftClickInputReference { get { return _UILeftClickInputReference; } }

	/// <summary>Gets UIMiddleClickInputReference property.</summary>
	public InputActionReference UIMiddleClickInputReference { get { return _UIMiddleClickInputReference; } }

	/// <summary>Gets UIRightClickInputReference property.</summary>
	public InputActionReference UIRightClickInputReference { get { return _UIRightClickInputReference; } }

	/// <summary>Gets UIScrollWheelInputReference property.</summary>
	public InputActionReference UIScrollWheelInputReference { get { return _UIScrollWheelInputReference; } }

	/// <summary>Gets UIMoveInputReference property.</summary>
	public InputActionReference UIMoveInputReference { get { return _UIMoveInputReference; } }

	/// <summary>Gets UISubmitInputReference property.</summary>
	public InputActionReference UISubmitInputReference { get { return _UISubmitInputReference; } }

	/// <summary>Gets UICancelInputReference property.</summary>
	public InputActionReference UICancelInputReference { get { return _UICancelInputReference; } }

	/// <summary>Gets UITrackedPositionInputReference property.</summary>
	public InputActionReference UITrackedPositionInputReference { get { return _UITrackedPositionInputReference; } }

	/// <summary>Gets UITrackedOrientationInputReference property.</summary>
	public InputActionReference UITrackedOrientationInputReference { get { return _UITrackedOrientationInputReference; } }

	/// <summary>Gets and Sets leftDeadZoneRadius property.</summary>
	public float leftDeadZoneRadius
	{
		get { return _leftDeadZoneRadius; }
		set { _leftDeadZoneRadius = Mathf.Clamp(value, 0.0f, 1.0f); }
	}

	/// <summary>Gets and Sets rightDeadZoneRadius property.</summary>
	public float rightDeadZoneRadius
	{
		get { return _rightDeadZoneRadius; }
		set { _rightDeadZoneRadius = Mathf.Clamp(value, 0.0f, 1.0f); }
	}

	/// <summary>Gets and Sets leftAxesMagnitude property.</summary>
	public float leftAxesMagnitude
	{
		get { return _leftAxesMagnitude; }
		protected set { _leftAxesMagnitude = value; }
	}

	/// <summary>Gets and Sets rightAxesMagnitude property.</summary>
	public float rightAxesMagnitude
	{
		get { return _rightAxesMagnitude; }
		protected set { _rightAxesMagnitude = value; }
	}

	/// <summary>Gets and Sets leftAxes property.</summary>
	public Vector2 leftAxes
	{
		get { return _leftAxes; }
		protected set { _leftAxes = value; }
	}

	/// <summary>Gets and Sets rightAxes property.</summary>
	public Vector2 rightAxes
	{
		get { return _rightAxes; }
		protected set { _rightAxes = value; }
	}

	/// <summary>Gets and Sets previousLeftAxes property.</summary>
	public Vector2 previousLeftAxes
	{
		get { return _previousLeftAxes; }
		protected set { _previousLeftAxes = value; }
	}

	/// <summary>Gets and Sets previousRightAxes property.</summary>
	public Vector2 previousRightAxes
	{
		get { return _previousRightAxes; }
		protected set { _previousRightAxes = value; }
	}

	/// <summary>Gets and Sets stateFlags property.</summary>
	public int stateFlags
	{
		get { return _stateFlags; }
		protected set { _stateFlags = value; }
	}


	/// <summary>Gets and Sets inputFlags property.</summary>
	public int inputFlags
	{
		get { return _inputFlags; }
		protected set { _inputFlags = value; }
	}
#endregion

	/// <summary>Callback invoked when BaseCharacterInputController's instance is enabled.</summary>
	protected virtual void OnEnable() { /*...*/ }

	/// <summary>Callback invoked when BaseCharacterInputController's instance is disabled.</summary>
	protected virtual void OnDisable() { /*...*/ }

	/// <summary>Resets BaseCharacterInputController's instance to its default values.</summary>
	public virtual void Reset()
	{
#if UNITY_EDITOR
		leftDeadZoneRadius = 0.01f;
		rightDeadZoneRadius = 0.01f;
#endif
		leftAxes = Vector2.zero;
		rightAxes = Vector2.zero;
		previousLeftAxes = Vector2.zero;
		previousRightAxes = Vector2.zero;
		rightAxesMagnitude = 0.0f;
		stateFlags = 0;
		inputFlags = 0;
	}

	/// <summary>BaseCharacterInputController's instance initialization.</summary>
	protected virtual void Awake()
	{
		Initialize();
	}
	
	/// <summary>BaseCharacterInputController's tick at each frame.</summary>
	protected virtual void Update ()
	{
		UpdateAxes();
	}

	/// <summary>Updates BaseCharacterInputController's instance at each Physics Thread's frame.</summary>
	protected virtual void FixedUpdate() { /*...*/ }

	/// <summary>Callback internally invoked when the Axes are updated, but before the previous axes' values get updated.</summary>
	protected virtual void OnAxesUpdated() { /*...*/ }

	/// <summary>Initializes BaseCharacterInputController.</summary>
	protected virtual void Initialize()
	{
		VInputSystem.Enable(
			leftAxisXInputReference,
			leftAxisYInputReference,
			rightAxisXInputReference,
			rightAxisYInputReference
		);
	}

	/// <summary>Updates Left and Right Axes.</summary>
	protected virtual void UpdateAxes()
	{
		if(leftAxisXInputReference != null && leftAxisYInputReference != null)
		{
			leftAxes = new Vector2(
				leftAxisXInputReference.ReadValue<float>(),
				leftAxisYInputReference.ReadValue<float>()
			);
			leftAxesMagnitude = leftAxes.magnitude;
		}
		if(rightAxisXInputReference != null && rightAxisYInputReference != null)
		{
			rightAxes = new Vector2(
				rightAxisXInputReference.ReadValue<float>(),
				rightAxisYInputReference.ReadValue<float>()
			);
			rightAxesMagnitude = rightAxes.magnitude;
		}

		if(leftAxes.sqrMagnitude < (leftDeadZoneRadius * leftDeadZoneRadius)) leftAxes = Vector2.zero;
		if(rightAxes.sqrMagnitude < (rightDeadZoneRadius * rightDeadZoneRadius)) rightAxes = Vector2.zero;

		OnAxesUpdated();

		previousLeftAxes = leftAxes;
		previousRightAxes = rightAxes;
	}

	/// <summary>Adds State(s).</summary>
	/// <param name="_states">State(s) to add.</param>
	public void AddStates(int _states)
	{
		stateFlags |= _states;
	}

	/// <summary>Removes State(s).</summary>
	/// <param name="_states">State(s) to remove.</param>
	public void RemoveStates(int _states)
	{
		stateFlags &= ~_states;
	}

	/// <returns>String representing character's Controller.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("Input Flags: ");
		builder.AppendLine(inputFlags.GetBitChain());
		builder.Append("Left-Axes: ");
		builder.AppendLine(leftAxes.ToString());
		builder.Append("Right-Axes: ");
		builder.AppendLine(rightAxes.ToString());

		return builder.ToString();
	}
}
}