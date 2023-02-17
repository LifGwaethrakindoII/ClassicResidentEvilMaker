using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// \TODO Quaternion.FromToRotation? https://docs.unity3d.com/ScriptReference/Quaternion.FromToRotation.html

namespace Voidless
{
public enum JointState
{
	None,
	Default,
	FollowTarget
}

public class ConstrainedJoint : MonoBehaviour, IFiniteStateMachine<JointState>//, ITimeAffectable
{
	[SerializeField] private ReorientedJoint _joint; 			/// <summary>Reoriented Joints.</summary>
	[SerializeField] private Axes3D _constraints; 				/// <summary>Rotation's Constraints.</summary>
	[SerializeField] private Axes3D _referenceConstraints; 		/// <summary>Reference Rotation's Constraints.</summary>
	[SerializeField] private EulerRotation _offset; 			/// <summary>Additional Offset.</summary>
	[SerializeField] private bool _rotateReference; 			/// <summary>Rotate Reference's Transform.</summary>
	[SerializeField] private float _rotationSpeed; 				/// <summary>Rotation's Speed.</summary>
#if UNITY_EDITOR
	[SerializeField] private float rayLength; 					/// <summary>Ray's Length.</summary>
#endif
	private float _localTimeScale; 								/// <summary>Local Time's Scale.</summary>	
	private Vector3 _target; 									/// <summary>Joint's Target.</summary>
	private Quaternion _initialRotation; 						/// <summary>Initial's Local Rotation.</summary>
	private JointState _state; 									/// <summary>Current State.</summary>
	private JointState _previousState; 							/// <summary>Previous State.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets constraints property.</summary>
	public Axes3D constraints
	{
		get { return _constraints; }
		set { _constraints = value; }
	}

	/// <summary>Gets and Sets referenceConstraints property.</summary>
	public Axes3D referenceConstraints
	{
		get { return _referenceConstraints; }
		set { _referenceConstraints = value; }
	}

	/// <summary>Gets and Sets rotateReference property.</summary>
	public bool rotateReference
	{
		get { return _rotateReference; }
		set { _rotateReference = value; }
	}

	/// <summary>Gets and Sets joint property.</summary>
	public ReorientedJoint joint
	{
		get { return _joint; }
		set { _joint = value; }
	}

	/// <summary>Gets and Sets offset property.</summary>
	public EulerRotation offset
	{
		get { return _offset; }
		set { _offset = value; }
	}

	/// <summary>Gets and Sets rotationSpeed property.</summary>
	public float rotationSpeed
	{
		get { return _rotationSpeed; }
		set { _rotationSpeed = value; }
	}

	/// <summary>Gets and Sets localTimeScale property.</summary>
	public float localTimeScale
	{
		get { return _localTimeScale; }
		set { _localTimeScale = value; }
	}

	/// <summary>Gets and Sets target property.</summary>
	public Vector3 target
	{
		get { return _target; }
		set { _target = value; }
	}

	/// <summary>Gets and Sets initialRotation property.</summary>
	public Quaternion initialRotation
	{
		get { return _initialRotation; }
		set { _initialRotation = value; }
	}

	/// <summary>Gets and Sets state property.</summary>
	public JointState state
	{
		get { return _state; }
		set { _state = value; }
	}

	/// <summary>Gets and Sets previousState property.</summary>
	public JointState previousState
	{
		get { return _previousState; }
		set { _previousState = value; }
	}
#endregion

#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode when ConstrainedJoint's instance is selected.</summary>
	private void OnDrawGizmosSelected()
	{
		if(joint == null) return;

		Gizmos.color = Color.cyan;
		Gizmos.DrawRay(joint.transform.position, joint.rotation * offset * Vector3.forward * rayLength);
		
		if(Application.isPlaying)
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawRay(joint.transform.position, (target - joint.transform.position));
		}
	}
#endif

#region UnityMethods:
	/// <summary>Resets ConstrainedJoint's instance to its default values.</summary>
	public void Reset()
	{
		initialRotation = joint.transform.localRotation;
	}

	/// <summary>ConstrainedJoint's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		Reset();
		this.ChangeState(JointState.Default);
		localTimeScale = 1.0f;
	}

	/// <summary>Updates ConstrainedJoint's instance at each frame.</summary>
	private void LateUpdate()
	{
		switch(state)
		{
			case JointState.FollowTarget:
			Vector3 direction = Vector3.zero;
			float t = (rotationSpeed * localTimeScale * Time.smoothDeltaTime);

			if(rotateReference && joint.reference != null)
			{
				direction = VVector3.ConstrainedDirection(target, joint.reference.position + joint.transform.forward, referenceConstraints);
				joint.reference.rotation = Quaternion.Lerp(joint.reference.rotation, Quaternion.LookRotation(direction), t);
			}

			direction = VVector3.ConstrainedDirection(target, joint.transform.position, constraints);
			Quaternion lookRotation = VQuaternion.Delta(Quaternion.LookRotation(direction), (joint.rotationOffset * offset.rotation));

			joint.transform.rotation = Quaternion.Lerp(joint.transform.rotation, lookRotation, t);
			break;

			default:
			joint.transform.localRotation = initialRotation;
			return;
		}
	}
#endregion

#region FSM:
	/// <summary>Enters JointState State.</summary>
	/// <param name="_state">JointState State that will be entered.</param>
	public void OnEnterState(JointState _state)
	{
		switch(_state)
		{
			case JointState.Default:
			joint.transform.localRotation = initialRotation;
			break;
	
			default:
			break;
		}
	}
	
	/// <summary>Leaves JointState State.</summary>
	/// <param name="_state">JointState State that will be left.</param>
	public void OnExitState(JointState _state)
	{ }
#endregion

}
}