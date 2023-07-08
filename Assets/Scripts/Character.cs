using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Animations.Rigging;

namespace Voidless.REMaker
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
	public const string KEY_ANIMATIONEVENT_USEWEAPON = "KEY_ANIMATIONEVENT_USEWEAPON";

	public const int FLAG_ABILITY_MOVE = 1 << 0;
	public const int FLAG_ABILITY_ROTATE = 1 << 1;
	public const int FLAG_ABILITY_ALL = ~0;
	public const int STATE_FLAG_RUNNING = 1 << 0;
	public const int STATE_FLAG_TURNINGAROUND = 1 << 1;
	public const int STATE_FLAG_AIMING = 1 << 2;
	public const int STATE_FLAG_ATTACKING = 1 << 3;

	[InfoBox("@ToString()")]
	[Header("Component Dependencies:")]
	[TabGroup("Components")][SerializeField] private VAnimatorController _animatorController;
	[TabGroup("Components")][SerializeField] private AnimationEventInvoker _animationEventInvoker;
	[Space(5f)]
	[TabGroup("Movement")][SerializeField] private float movementSpeed;
	[TabGroup("Movement")][SerializeField] private float runScalar;
	[TabGroup("Movement")][SerializeField] private float backwardsMovementScalar;
	[TabGroup("Movement")][SerializeField] private float turnAroundDuration;
	[Space(5f)]
	[TabGroup("Rotation")][SerializeField] private float rotationSpeed;
	[TabGroup("Rotation")][SerializeField][Range(0.0f, 180.0f)] private float _minMovementAngle;
	[Space(5f)]
	[Header("Rig Constraints:")]
	[TabGroup("Animations" ,"Rig")][SerializeField] private StringMultiAimConstraintDictionary _multiAimConstraints;
	[TabGroup("Animations" ,"Rig")][SerializeField] private StringTwoBoneIKConstraintDictionary _twoBoneIKConstraints;
	[Space(5f)]
	[Header("Animator Controller's Attributes:")]
	[TabGroup("Animations" ,"Animations")][SerializeField] private int _locomotionLayer;
	[TabGroup("Animations" ,"Animations")][SerializeField] private int _actionLayer;
	[TabGroup("Animations" ,"Animations")][SerializeField][Range(0.0f, 1.0f)] private float _defaultCrossFade;
	[TabGroup("Animations" ,"Animations")][SerializeField] private UnityHash _leftAxisXHash;
	[TabGroup("Animations" ,"Animations")][SerializeField] private UnityHash _leftAxisYHash;
	[TabGroup("Animations" ,"Animations")][SerializeField] private UnityHash _rightAxisXHash;
	[TabGroup("Animations" ,"Animations")][SerializeField] private UnityHash _rightAxisYHash;
	private Renderer[] _renderers;
	private CharacterController _characterController;
	private Item _equippedItem;
	private Weapon _equippedWeapon;
	private Vector2 _leftAxes;
	private Vector2 _rightAxes;
	private int _states;
	private int _abilities;
	protected Coroutine actionRoutine;
	protected Coroutine turnAroundRoutine;

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

	/// <summary>Gets animatorController property.</summary>
	public VAnimatorController animatorController { get { return _animatorController; } }

	/// <summary>Gets animationEventInvoker property.</summary>
	public AnimationEventInvoker animationEventInvoker { get { return _animationEventInvoker; } }

	/// <summary>Gets locomotionLayer property.</summary>
	public int locomotionLayer { get { return _locomotionLayer; } }

	/// <summary>Gets actionLayer property.</summary>
	public int actionLayer { get { return _actionLayer; } }

	/// <summary>Gets and Sets states property.</summary>
	public int states
	{
		get { return _states; }
		protected set { _states = value; }
	}

	/// <summary>Gets and Sets abilities property.</summary>
	public int abilities
	{
		get { return _abilities; }
		protected set { _abilities = value; }
	}

	/// <summary>Gets and Sets minMovementAngle property.</summary>
	public float minMovementAngle
	{
		get { return _minMovementAngle; }
		set { _minMovementAngle = value; }
	}

	/// <summary>Gets defaultCrossFade property.</summary>
	public float defaultCrossFade { get { return _defaultCrossFade; } }

	/// <summary>Gets and Sets equippedWeapon property.</summary>
	public Weapon equippedWeapon
	{
		get { return _equippedWeapon; }
		set { _equippedWeapon = value; }
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

	/// <summary>Gets leftAxisXHash property.</summary>
	public UnityHash leftAxisXHash { get { return _leftAxisXHash; } }

	/// <summary>Gets leftAxisYHash property.</summary>
	public UnityHash leftAxisYHash { get { return _leftAxisYHash; } }

	/// <summary>Gets rightAxisXHash property.</summary>
	public UnityHash rightAxisXHash { get { return _rightAxisXHash; } }

	/// <summary>Gets rightAxisYHash property.</summary>
	public UnityHash rightAxisYHash { get { return _rightAxisYHash; } }

	/// <summary>Character's static constructor.</summary>
	static Character()
	{
		NAMES_ABILITIES = new string[]
		{
			"FLAG_ABILITY_MOVE",
			"FLAG_ABILITY_ROTATE"
		};
		NAMES_STATES = new string[]
		{
			"STATE_FLAG_RUNNING",
			"STATE_FLAG_TURNINGAROUND",
			"STATE_FLAG_AIMING",
			"STATE_FLAG_ATTACKING"
		};
	}

#region UnityMethods:
	/// <summary>Draws Gizmos on Editor mode when Character's instance is selected.</summary>
	protected virtual void OnDrawGizmosSelected() { /*...*/ }

	/// <summary>Character's instance initialization.</summary>
	protected virtual void Awake()
	{
		abilities = FLAG_ABILITY_ALL;
		if(animationEventInvoker != null) animationEventInvoker.AddAnimationEventActionListener(OnAnimationEvent);
	}

	/// <summary>Character's starting actions before 1st Update frame.</summary>
	protected virtual void Start() { /*...*/ }
	
	/// <summary>Character's tick at each frame.</summary>
	protected virtual void Update()
	{
		OnAxesUpdated();
	}
#endregion

	[OnInspectorGUI("Get Renderers")]
	/// <summary>Gets self-contained Renderers.</summary>
	private void GetRenderers()
	{
		renderers = GetComponentsInChildren<Renderer>();
	}

	/// <summary>Enables Renderers.</summary>
	/// <param name="_enable">Enable? true by default.</param>
	public void EnableRenderers(bool _enable = true)
	{
		if(renderers == null || renderers.Length == 0) GetRenderers();

		foreach(Renderer renderer in renderers)
		{
			renderer.gameObject.SetActive(_enable);
		}
	}

	/// <summary>Sets Left Axes.</summary>
	/// <param name="_axes">Left Axes.</param>
	public virtual void SetLeftAxes(Vector2 _axes)
	{
		leftAxes = _axes;
	}

	/// <summary>Sets Right Axes.</summary>
	/// <param name="_axes">Right Axes.</param>
	public virtual void SetRightAxes(Vector2 _axes)
	{
		rightAxes = _axes;
	}

	/// <summary>Callback intwernally invoked when both left and right axes are updated.</summary>
	protected virtual void OnAxesUpdated() { /*...*/ }

	/// <summary>Goes Idle.</summary>
	public void GoIdle()
	{
		animatorController.animator.SetFloat(leftAxisYHash, 0.0f);
	}

	/// <summary>Activates/Deactivates Running Flag.</summary>
	/// <param name="_run">Activate?.</param>
	public void Run(bool _run)
	{
		switch(_run)
		{
			case true:
			states |= STATE_FLAG_RUNNING;
			break;

			case false:
			states &= ~STATE_FLAG_RUNNING;
			break;
		}
	}

	/// <summary>Displaces Character.</summary>
	/// <param name="axes">Displacement's Axes.</param>
	public void Move(Vector2 axes)
	{
		/// Don't move if turning around...
		if(turnAroundRoutine != null || (states | STATE_FLAG_TURNINGAROUND) == states) return;

		Vector3 axes3D = new Vector3(axes.x, 0.0f, axes.y);
		
		if(axes3D.sqrMagnitude > 1.0f) axes3D.Normalize();

		float sign = Mathf.Sign(axes3D.z);
		float blend = 0.0f;

		RotateSelf(axes3D.x);

		if(Mathf.Abs(axes3D.z) > Mathf.Abs(axes3D.x))
		{
			if(axes3D.z < 0.0f)
			{
				sign *= backwardsMovementScalar;
				blend = -2.0f;

			} else if((states | STATE_FLAG_RUNNING) == states)
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

		animatorController.animator.SetFloat(leftAxisYHash, blend);
	}

	/// <summary>Rotates and moves character towards provided direction [used for AI context].</summary>
	/// <param name="direction">Target direction.</param>
	public void MoveTowards(Vector3 direction)
	{
		float angle = Vector3.Angle(direction, transform.forward);
		float sign = Mathf.Sign((Quaternion.Inverse(transform.rotation) * direction).x);
		float n = Mathf.Min(angle, rotationSpeed) / rotationSpeed;

		RotateTowardsDirection(direction);

		if(angle <= minMovementAngle) Move(Vector2.up);
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

	/// <summary>Makes the character turn around.</summary>
	public virtual void TurnAround()
	{
		states |= STATE_FLAG_TURNINGAROUND;
		this.StartCoroutine(TurnAroundCoroutine(), ref turnAroundRoutine);
	}

	/// <summary>Callback invoked when an Animation Event occurs.</summary>
	/// <param name="_event">Animation's Event.</param>
	protected virtual void OnAnimationEvent(AnimationEvent _event)
	{
		Debug.Log("[Character] Invoked Animation String Event " + _event.stringParameter);

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

			case KEY_ANIMATIONEVENT_USEWEAPON:
				if(equippedWeapon != null)
				{
					Debug.Log("[Character] Invoke OnUse() on Weapon " + equippedWeapon.gameObject.name);
					equippedWeapon.OnUse();
				}
			break;

			default:
				Debug.Log("[Character] Non-registered event with key " + _event.stringParameter + " was invoked...");
			break;
		}
	}

	/// <summary>Turn-Around's Coroutine.</summary>
	protected virtual IEnumerator TurnAroundCoroutine()
	{
		float i = 1.0f / turnAroundDuration;
		float t = 0.0f;
		Quaternion a = transform.rotation;
		Quaternion b = a * Quaternion.Euler(0.0f, 180.0f, 0.0f);

		while(t < 1.0f)
		{
			transform.rotation = Quaternion.Lerp(a, b, t);
			t += (Time.deltaTime * i);
			animatorController.animator.SetFloat(leftAxisYHash, 1.0f);
			yield return null;
		}

		animatorController.animator.SetFloat(leftAxisYHash, 0.0f);
		transform.rotation = b;
		states &= ~STATE_FLAG_TURNINGAROUND;
		this.DispatchCoroutine(ref turnAroundRoutine);
	}

	/// <returns>String representing this Character.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("Abilities: ");
		builder.AppendLine(VString.GetNamedBitChain(abilities, NAMES_ABILITIES));
		builder.Append("States: ");
		builder.AppendLine(VString.GetNamedBitChain(states, NAMES_STATES));

		return builder.ToString();
	}
}
}