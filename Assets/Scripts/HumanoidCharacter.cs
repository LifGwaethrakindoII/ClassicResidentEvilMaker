using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Animations.Rigging;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Voidless.REMaker
{
public enum Hand { Right, Left }

public class HumanoidCharacter : Character
{
	[Space(5f)]
	[Header("Humanoid Character's Attributes:")]
	[SerializeField] private Hand _dominantHand;
	[Space(5f)]
	[Header("Body Anchors:")]
	[TabGroup("Animations" ,"Rig")][SerializeField] private Transform _leftHandAnchor;
	[TabGroup("Animations" ,"Rig")][SerializeField] private Transform _rightHandAnchor;
	[Space(5f)]
	[Header("Multi-Aim Constraints:")]
	[TabGroup("Animations" ,"Rig")][SerializeField] private MultiAimConstraint _torsoAimConstraint;
	[TabGroup("Animations" ,"Rig")][SerializeField] private MultiAimConstraint _leftHandAimConstraint;
	[TabGroup("Animations" ,"Rig")][SerializeField] private MultiAimConstraint _rightHandAimConstraint;
	[TabGroup("Animations" ,"Rig")][SerializeField] private MultiAimConstraint _leftFootAimConstraint;
	[TabGroup("Animations" ,"Rig")][SerializeField] private MultiAimConstraint _rightFootAimConstraint;
	[Space(5f)]
	[Header("Two-Bone IK Constraints:")]
	[TabGroup("Animations" ,"Rig")][SerializeField] private TwoBoneIKConstraint _leftHandIKConstraint;
	[TabGroup("Animations" ,"Rig")][SerializeField] private TwoBoneIKConstraint _rightHandIKConstraint;
	[TabGroup("Animations" ,"Rig")][SerializeField] private TwoBoneIKConstraint _leftFootIKConstraint;
	[TabGroup("Animations" ,"Rig")][SerializeField] private TwoBoneIKConstraint _rightFootIKConstraint;
	[Space(5f)]
	[Header("Torso Aiming's Attributes:")]
	[TabGroup("Animations" ,"Rig")][SerializeField][Range(0.0f, 90.0f)] private float _torsoAimMaxAngle;
	[TabGroup("Animations" ,"Rig")][SerializeField][Range(0.0f, 90.0f)] private float _torsoAimMinAngle;
	[TabGroup("Animations" ,"Rig")][SerializeField] private EulerRotation _torsoAimOffset;
	[TabGroup("Animations" ,"Rig")][SerializeField] private float _torsoAimRadius;
	[Space(5f)]
	[TabGroup("Animations" ,"Animations")][SerializeField] private UnityHash _aimHash;
	[Space(5f)]
	[TabGroup("Actions", "Aiming")][SerializeField] private float _aimingAcceleration;
	private float _aimingAxis;
	protected float aimingVelocity;

#region Getters/Setters:
	/// <summary>Gets and Sets dominantHand property.</summary>
	public Hand dominantHand
	{
		get { return _dominantHand; }
		set { _dominantHand = value; }
	}

	/// <summary>Gets and Sets leftHandAnchor property.</summary>
	public Transform leftHandAnchor
	{
		get { return _leftHandAnchor; }
		set { _leftHandAnchor = value; }
	}

	/// <summary>Gets and Sets rightHandAnchor property.</summary>
	public Transform rightHandAnchor
	{
		get { return _rightHandAnchor; }
		set { _rightHandAnchor = value; }
	}

	/// <summary>Gets torsoAimConstraint property.</summary>
	public MultiAimConstraint torsoAimConstraint { get { return _torsoAimConstraint; } }

	/// <summary>Gets leftHandAimConstraint property.</summary>
	public MultiAimConstraint leftHandAimConstraint { get { return _leftHandAimConstraint; } }

	/// <summary>Gets rightHandAimConstraint property.</summary>
	public MultiAimConstraint rightHandAimConstraint { get { return _rightHandAimConstraint; } }

	/// <summary>Gets leftFootAimConstraint property.</summary>
	public MultiAimConstraint leftFootAimConstraint { get { return _leftFootAimConstraint; } }

	/// <summary>Gets rightFootAimConstraint property.</summary>
	public MultiAimConstraint rightFootAimConstraint { get { return _rightFootAimConstraint; } }

	/// <summary>Gets leftHandIKConstraint property.</summary>
	public TwoBoneIKConstraint leftHandIKConstraint { get { return _leftHandIKConstraint; } }

	/// <summary>Gets rightHandIKConstraint property.</summary>
	public TwoBoneIKConstraint rightHandIKConstraint { get { return _rightHandIKConstraint; } }

	/// <summary>Gets leftFootIKConstraint property.</summary>
	public TwoBoneIKConstraint leftFootIKConstraint { get { return _leftFootIKConstraint; } }

	/// <summary>Gets rightFootIKConstraint property.</summary>
	public TwoBoneIKConstraint rightFootIKConstraint { get { return _rightFootIKConstraint; } }

	/// <summary>Gets aimingAcceleration property.</summary>
	public float aimingAcceleration { get { return _aimingAcceleration; } }

	/// <summary>Gets and Sets aimingAxis property.</summary>
	public float aimingAxis
	{
		get { return _aimingAxis; }
		protected set { _aimingAxis = value; }
	}
#endregion

	/// <summary>Gets aimHash property.</summary>
	public UnityHash aimHash { get { return _aimHash; } }

	/// <summary>Draws Gizmos on Editor mode when Character's instance is selected.</summary>
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();

		if(torsoAimConstraint == null) return;

		Color color = Color.cyan.WithAlpha(0.5f);
		Transform torso = torsoAimConstraint.data.constrainedObject;
		float radius = 0.05f;
		Quaternion rotation = transform.rotation;
		Quaternion minRotation = rotation * Quaternion.Euler(Vector3.right * _torsoAimMinAngle);
		Quaternion maxRotation = rotation * Quaternion.Euler(-Vector3.right * _torsoAimMaxAngle);

		Gizmos.color = color;
		Gizmos.DrawSphere(torso.position + (minRotation * (Vector3.forward * _torsoAimRadius)), radius);
		Gizmos.DrawSphere(torso.position + (maxRotation * (Vector3.forward * _torsoAimRadius)), radius);

#if UNITY_EDITOR
		Handles.color = color;
		Handles.DrawSolidArc(torso.position, transform.right, transform.forward, _torsoAimMinAngle, _torsoAimRadius);
		Handles.DrawSolidArc(torso.position, -transform.right, transform.forward, _torsoAimMaxAngle, _torsoAimRadius);
#endif
	}

	/// <summary>HumanoidCharacter's instance initialization when loaded [Before scene loads].</summary>
	protected override void Awake()
	{
		base.Awake();
		VAnimationRigging.SetRigConstraintsWeights(0.0f,
			leftFootIKConstraint,
			rightFootIKConstraint,
			leftFootAimConstraint,
			rightFootAimConstraint,
			rightHandIKConstraint,
			rightHandAimConstraint,
			leftHandIKConstraint,
			leftHandAimConstraint
		);
	}

	/// <summary>Updates HumanoidCharacter's instance at each frame.</summary>
	protected override void Update()
	{
		base.Update();
	}

	/// <summary>Callback intwernally invoked when both left and right axes are updated.</summary>
	protected override void OnAxesUpdated()
	{
		base.OnAxesUpdated();

		if((states | STATE_FLAG_AIMING) == states)
		{
			RotateSelf(leftAxes.x);
			SetTorsoAimingAxis(leftAxes.y);
		}
		else
		{
			if(leftAxes.sqrMagnitude > 0.0f) Move(leftAxes);
			else GoIdle();
		}
	}

	/// <summary>Sets the Character to aim with its equipped weapon [if it has one].</summary>
	/// <param name="_aim">Aim? default to true. If false, it will cancel the aiming.</param>
	public void Aim(bool _aim = true)
	{
		switch(_aim)
		{
			case true:
				/*Cancel if either:
				- Already aiming.
				- There is no Torso aim constraint.
				- There is no weapon equipped*/
				if((states | STATE_FLAG_AIMING) == states
				|| torsoAimConstraint == null
				|| equippedWeapon == null) return;

				states |= STATE_FLAG_AIMING;
				animatorController.animator.SetLayerWeight(locomotionLayer, 0.0f);
				VAnimationRigging.SetRigConstraintsWeights(0.0f,
					leftFootIKConstraint,
					rightFootIKConstraint,
					leftFootAimConstraint,
					rightFootAimConstraint
				);
				VAnimationRigging.SetRigConstraintsWeights(1.0f, torsoAimConstraint);
				animatorController.animator.CrossFade(aimHash, defaultCrossFade, actionLayer);
				SetTorsoAimingAxis(0.0f);
				aimingAxis = 0.5f;
				SetTorsoAimingAxis(aimingAxis);
			break;

			case false:
				states &= ~STATE_FLAG_AIMING;

				if((states | STATE_FLAG_ATTACKING) == states) return;

				VAnimationRigging.SetRigConstraintsWeights(0.0f, torsoAimConstraint);
				animatorController.animator.SetLayerWeight(locomotionLayer, 1.0f);
				animatorController.animator.CrossFade(VAnimator.CREDENTIAL_EMPTY, defaultCrossFade, actionLayer);
			break;
		}
	}

	/// <summary>Sets Torso's Aiming Axis.</summary>
	public void SetTorsoAimingAxis(float a)
	{
		a = Mathf.Clamp(a, -1.0f, 1.0f);

		float t = VMath.RemapValueToNormalizedRange(a, -1.0f, 1.0f);
		Transform torso = torsoAimConstraint.data.constrainedObject;
		float radius = _torsoAimRadius;
		Quaternion rotation = transform.rotation * _torsoAimOffset;
		Quaternion minRotation = rotation * Quaternion.Euler(Vector3.right * _torsoAimMinAngle);
		Quaternion maxRotation = rotation * Quaternion.Euler(-Vector3.right * _torsoAimMaxAngle);
		Vector3 min = torso.position + (minRotation * (Vector3.forward * radius));
		Vector3 max = torso.position + (maxRotation * (Vector3.forward * radius));
		Transform target = torsoAimConstraint.GetSourceObject();

		aimingAxis = VMath.AccelerateTowards(aimingAxis, t, ref aimingVelocity, aimingAcceleration, Time.deltaTime);
		target.position = Vector3.Slerp(min, max, aimingAxis);
	}

#region HandGetters:
/*===================================================================================================================================================
| 	Hands' Getters:																																	|
===================================================================================================================================================*/
	/// <returns>Domitant Hand.</returns>
	public Transform GetDominantHand()
	{
		switch(dominantHand)
		{
			case Hand.Left: 	return leftHandAnchor;
			case Hand.Right:
			default: 			return rightHandAnchor;
		}
	}

	/// <returns>Recessive Hand.</returns>
	public Transform GetRecessiveHand()
	{
		switch(dominantHand)
		{
			case Hand.Left: 	return rightHandAnchor;
			case Hand.Right:
			default: 			return leftHandAnchor;
		}
	}

	/// <returns>Dominant Hand's IK Constraint.</returns>
	public TwoBoneIKConstraint GetDominantHandIK()
	{
		switch(dominantHand)
		{
			case Hand.Left: 	return leftHandIKConstraint;
			case Hand.Right:
			default: 			return rightHandIKConstraint;
		}	
	}

	/// <returns>Recessive Hand's IK Constraint.</returns>
	public TwoBoneIKConstraint GetRecessiveHandIK()
	{
		switch(dominantHand)
		{
			case Hand.Left: 	return rightHandIKConstraint;
			case Hand.Right:
			default: 			return leftHandIKConstraint;
		}	
	}

	/// <returns>Dominant Hand's Aim Constraint.</returns>
	public MultiAimConstraint GetDominantHandAim()
	{
		switch(dominantHand)
		{
			case Hand.Left: 	return leftHandAimConstraint;
			case Hand.Right:
			default: 			return rightHandAimConstraint;
		}	
	}

	/// <returns>Recessive Hand's Aim Constraint.</returns>
	public MultiAimConstraint GetRecessiveHandAim()
	{
		switch(dominantHand)
		{
			case Hand.Left: 	return rightHandAimConstraint;
			case Hand.Right:
			default: 			return leftHandAimConstraint;
		}	
	}
#endregion

	[Button("Equip Item")]
	/// <summary>Equips Item into dominant hand [regardless of the sub-type].</summary>
	/// <param name="_item">Item to equip.</param>
	/// <returns>True if the Item was successfully equipped.</returns>
	public bool EquipItem(Item _item)
	{
		Transform mainHand = GetDominantHand();
		
		if(_item == null || mainHand == null) return false;

		_item.gameObject.SetActive(true);
		_item.transform.SetOffsetedPosition(mainHand.position, _item.GetGrabPointOffset());
		_item.transform.rotation = mainHand.rotation;
		_item.transform.parent = mainHand;

		return true;
	}

	[Button("Equip Weapon")]
	/// <summary>Equips Weapon into dominant Hand.</summary>
	/// <param name="_weapon">Weapon to equip.</param>
	/// <returns>True if the Weapon was successfully equipped.</returns>
	public bool EquipWeapon(Weapon _weapon)
	{
		if(EquipItem(_weapon))
		{
			equippedWeapon = _weapon;
			return true;
		}
		else return false;
	}

	/// <summary>Uses weapon, if equipped.</summary>
	public bool UseWeapon()
	{
		if(equippedWeapon == null || (states | STATE_FLAG_AIMING) != states) return false;

		Debug.Log("[HumanoidCharacter] Pew, pew. Using weapon's animation: " + equippedWeapon.performHash.ToString());
		states |= STATE_FLAG_ATTACKING;
		//animatorController.animator.CrossFade(equippedWeapon.performHash, defaultCrossFade, actionLayer);
		this.StartCoroutine(animatorController.animator.CrossFadeAndWait(equippedWeapon.performHash, defaultCrossFade, actionLayer, OnWeaponUsed), ref actionRoutine);
		return true;
	}

	/// <summary>Callback internally invoked after the weapon has been used.</summary>
	private void OnWeaponUsed()
	{
		this.DispatchCoroutine(ref actionRoutine);

		states &= ~ STATE_FLAG_ATTACKING;
		if((states | STATE_FLAG_AIMING) == states)
		{
			Debug.Log("[HumanoidCharacter] Re-aiming...");
			Aim(false);
			Aim(true);
		} else
		{
			Aim(false);
			Debug.Log("[HumanoidCharacter] NO?");
		}
	}
}
}