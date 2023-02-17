using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Voidless
{
[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
	public static readonly string[] NAMES_ABILITIES;
	public static readonly string[] NAMES_STATES;

	public const string KEY_ANIMATIONEVENT_TURNABILITY_MOVE_OFF = "KEY_ANIMATIONEVENT_TURNABILITY_MOVE_OFF";
	public const string KEY_ANIMATIONEVENT_TURNABILITY_MOVE_ON = "KEY_ANIMATIONEVENT_TURNABILITY_MOVE_ON";
	public const string KEY_ANIMATIONEVENT_TURNABILITY_ROTATE_OFF = "KEY_ANIMATIONEVENT_TURNABILITY_ROTATE_OFF";
	public const string KEY_ANIMATIONEVENT_TURNABILITY_ROTATE_ON = "KEY_ANIMATIONEVENT_TURNABILITY_ROTATE_ON";

	public const int FLAG_ABILITY_MOVE = 1 << 0;
	public const int FLAG_ABILITY_ROTATE = 1 << 1;
	public const int FLAG_ABILITY_ALL = ~0;
	public const int STATE_FLAG_RUNNING = 1 << 0;

	[InfoBox("@ToString()")]
	[SerializeField] private VAnimatorController animatorController; 		/// <summary>AnimatorController's Component attached to the Animator.</summary>
	[SerializeField] private AnimationEventInvoker animationEventInvoker; 	/// <summary>AnimationEventInvoker's Component attached to the Animator.</summary>
	[SerializeField] private AngleDotProduct forwardDotLimit; 				/// <summary>Forward Vector's Dot Limit.</summary>
	[SerializeField] private AngleDotProduct backwardDotLimit; 				/// <summary>Backward Vector's Dot Limit.</summary>
	[SerializeField] private float movementSpeed; 							/// <summary>Movement's Speed.</summary>
	[SerializeField] private float runScalar; 								/// <summary>Running's Scalar.</summary>
	[SerializeField] private float backwardsMovementScalar; 				/// <summary>Backwards' Movement Scalar.</summary>
	[SerializeField] private float rotationSpeed; 							/// <summary>Rotation's Speed.</summary>
	private Renderer[] _renderers; 											/// <summary>Character's Renderers.</summary>
	private int state; 														/// <summary>State Flags.</summary>
	private int abilities;
	private CharacterController _characterController; 						/// <summary>CharacterController's Component.</summary>

	/// <summary>Gets and Sets renderers property.</summary>
	public Renderer[] renderers
	{
		get { return _renderers; }
		set { _renderers = value; }
	}

	/// <summary>Gets characterController Component.</summary>
	public CharacterController characterController
	{ 
		get
		{
			if(_characterController == null) _characterController = GetComponent<CharacterController>();
			return _characterController;
		}
	}

#region UnityMethods:
	static Character()
	{
		NAMES_ABILITIES = new string[]
		{
			"FLAG_ABILITY_MOVE",
			"FLAG_ABILITY_ROTATE"
		};
		NAMES_STATES = new string[]
		{
			"STATE_FLAG_RUNNING"
		};
	}

	/// <summary>Character's instance initialization.</summary>
	protected virtual void Awake()
	{
		abilities = FLAG_ABILITY_ALL;
		if(animationEventInvoker != null) animationEventInvoker.AddAnimationEventActionListener(OnAnimationEvent);
	}

	/// <summary>Character's starting actions before 1st Update frame.</summary>
	protected virtual void Start ()
	{
		
	}
	
	/// <summary>Character's tick at each frame.</summary>
	protected virtual void Update ()
	{
		
	}
#endregion

	[OnInspectorGUI("Get Renderers")]
	private void GetRenderers()
	{
		renderers = GetComponentsInChildren<Renderer>();
	}

	public void EnableRenderers(bool _enable = true)
	{
		if(renderers == null || renderers.Length == 0) GetRenderers();

		foreach(Renderer renderer in renderers)
		{
			renderer.gameObject.SetActive(_enable);
		}
	}

	/// <summary>Goes Idle.</summary>
	public void GoIdle()
	{
		animatorController.animator.SetFloat("LeftAxisY", 0.0f);
	}

	/// <summary>Activates/Deactivates Running Flag.</summary>
	/// <param name="_run">Activate?.</param>
	public void Run(bool _run)
	{
		switch(_run)
		{
			case true:
			state |= STATE_FLAG_RUNNING;
			break;

			case false:
			state &= ~STATE_FLAG_RUNNING;
			break;
		}
	}

	/// <summary>Displaces Character.</summary>
	/// <param name="axes">Displacement's Axes.</param>
	public void Move(Vector2 axes)
	{
		Vector3 axes3D = new Vector3(axes.x, 0.0f, axes.y);
		
		if(axes3D.sqrMagnitude > 1.0f) axes3D.Normalize();

		float sign = Mathf.Sign(axes3D.z);
		float blend = 0.0f;

		RotateSelf(axes3D.x * sign);

		if(Mathf.Abs(axes3D.z) > Mathf.Abs(axes3D.x))
		{
			if(axes3D.z < 0.0f)
			{
				sign *= backwardsMovementScalar;
				blend = -2.0f;

			} else if((state | STATE_FLAG_RUNNING) == state)
			{
				sign *= runScalar;
				blend = 2.0f;
			}
			else blend = 1.0f;

			if((abilities | FLAG_ABILITY_MOVE) == abilities)
			{
				Vector3 displacement = transform.forward * (axes3D.magnitude * sign * movementSpeed * Time.deltaTime);
				characterController.Move(displacement);
			}
		}
		else blend = -1.0f;

		animatorController.animator.SetFloat("LeftAxisY", blend);
	}

	/// <summary>Rotates and moves character towards provided direction [used for AI context].</summary>
	/// <param name="direction">Target direction.</param>
	public void MoveTowards(Vector3 direction)
	{
		float angle = Vector3.Angle(direction, transform.forward);
		float sign = Mathf.Sign((Quaternion.Inverse(transform.rotation) * direction).x);
		float n = Mathf.Min(angle, rotationSpeed) / rotationSpeed;


		RotateTowardsDirection(direction);

		//if(angle > 10.0f) return;

		/*Vector3 displacement = transform.forward * (Mathf.Min(direction.magnitude, 1.0f) * movementSpeed * Time.deltaTime);
		characterController.Move(displacement);*/
		Move(Vector2.up);
	}

	/// <summary>Rotates itself on the left or right, depending of the provided sign.</summary>
	/// <param name="s">Normalized sign that determines which side to rotate relative to itself.</param>
	public void RotateSelf(float s)
	{
		if((abilities | FLAG_ABILITY_ROTATE) != abilities) return;
		transform.Rotate(Vector3.up * rotationSpeed * s * Time.deltaTime, Space.Self);
	}

	/// <summary>Rotates towards direction [used for AI context].</summary>
	/// <param name="direction">Desired direction [y component gets zeroed internally].</param>
	public void RotateTowardsDirection(Vector3 direction)
	{
		if((abilities | FLAG_ABILITY_ROTATE) != abilities) return;

		direction.y = 0.0f;

		Quaternion lookRotation = Quaternion.LookRotation(direction);
		Quaternion rotation = transform.rotation;

		transform.rotation = Quaternion.RotateTowards(rotation, lookRotation, rotationSpeed * Time.deltaTime);
	}

	/// <summary>Rotates towards target.</summary>
	/// <param name="_target">Target's point in space.</param>
	public void RotateTowardsTarget(Vector3 _target)
	{
		RotateTowardsDirection(_target - transform.position);
	}

	/// <summary>Rotates Character.</summary>
	/// <param name="direction">Look Direction.</param>
	public void Rotate(Vector2 direction)
	{
		direction.y = 0.0f;

		Quaternion lookRotation = Quaternion.LookRotation(direction);
		Quaternion rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

		transform.rotation = rotation;
	}

	/// <summary>Callback invoked when an Animation Event occurs.</summary>
	/// <param name="_event">Animation's Event.</param>
	protected virtual void OnAnimationEvent(AnimationEvent _event)
	{
		switch(_event.stringParameter)
		{
			case KEY_ANIMATIONEVENT_TURNABILITY_MOVE_OFF:
				abilities &= ~FLAG_ABILITY_MOVE;
			break;

			case KEY_ANIMATIONEVENT_TURNABILITY_MOVE_ON:
				abilities |= FLAG_ABILITY_MOVE;
			break;

			case KEY_ANIMATIONEVENT_TURNABILITY_ROTATE_OFF:
				abilities &= ~FLAG_ABILITY_ROTATE;
			break;

			case KEY_ANIMATIONEVENT_TURNABILITY_ROTATE_ON:
				abilities |= FLAG_ABILITY_ROTATE;
			break;

			default:
				Debug.Log("[Character] Non-registered event with key " + _event.stringParameter + " was invoked...");
			break;
		}
	}

	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("Abilities: ");
		builder.AppendLine(VString.GetNamedBitChain(abilities, NAMES_ABILITIES));
		builder.Append("States: ");
		builder.AppendLine(VString.GetNamedBitChain(state, NAMES_STATES));

		return builder.ToString();
	}
}
}