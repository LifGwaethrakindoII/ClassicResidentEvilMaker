using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.VR
{
/// SELF-REFLECTING NOTE: MAKE THE UPDATE CYCLE RELATIVE TO THE USER MOTION'S MAGNITUDE [THE FASTER, THE SHORTER THE UPDATE CYCLE IS]

/// <summary>Event invoked when the Pattern Evaluator recognizes a Pattern's Command.</summary>
/// <param name="_command">Command obtained from the Patterns' Data.</param>
public delegate void OnPatternRecognized(Command _command);

#pragma warning disable 67
public class PatternRecognizer : MonoBehaviour
{
	public static event OnPatternRecognized onPatternRecognized; 			/// <summary>OnPatternRecognized's event delegate.</summary>

	/*[SerializeField] private ApplicationData _data; 				/// <summary>Application's Data.</summary>
	//[SerializeField] private User _user; 							/// <summary>Ground Support's Member to recognize patterns.</summary>
	[Space(5f)]
	[Header("Patter Data's Attributes:")]
	[SerializeField] private Vector3 _distanceThreshold; 			/// <summary>Orientation's Distance Threshold.</summary>
	[SerializeField] private float _motionMagnitudeThreshold; 		/// <summary>User's Motion Magnitude's threshold to be considered a motion.</summary>
	[SerializeField] private float _motionUpdateCycle; 				/// <summary>Cycle in seconds to update the User's current motion' status.</summary>
	private PatternCommand[] patternCommands; 						/// <summary>LMAO.</summary>
	private List<PatternData> patternsData; 						/// <summary>Set of pattern's points that will be evaluated.</summary>
	private Coroutine patternEvaluator; 							/// <summary>Pattern Evaluator's Coroutine.</summary>
	/// New Mode:
	private List<PatternClassificationResult> evaluations;
	[SerializeField] private float _waitTolerance; 					/// <summary>Wait Tolerance if the paddles are not on the desired's waypoint.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets data property.</summary>
	public ApplicationData data
	{
		get { return _data; }
		set { _data = value; }
	}

	/// <summary>Gets and Sets user property.</summary>
	public User user
	{
		get { return _user; }
		set { _user = value; }
	}

	/// <summary>Gets and Sets distanceThreshold property.</summary>
	public Vector3 distanceThreshold
	{
		get { return _distanceThreshold; }
		set { _distanceThreshold = value; }
	}

	/// <summary>Gets and Sets motionMagnitudeThreshold property.</summary>
	public float motionMagnitudeThreshold
	{
		get { return _motionMagnitudeThreshold; }
		set { _motionMagnitudeThreshold = value; }
	}

	/// <summary>Gets and Sets motionUpdateCycle property.</summary>
	public float motionUpdateCycle
	{
		get { return _motionUpdateCycle; }
		set { _motionUpdateCycle = value; }
	}

	/// <summary>Gets and Sets waitTolerance property.</summary>
	public float waitTolerance
	{
		get { return _waitTolerance; }
		set { _waitTolerance = value; }
	}
#endregion

	private void OnEnable()
	{
		user.onPaddlesPicked += OnUserPickedPaddles;
		ApplicationData.onDataUpdated += SetEvaluations;
	}

	private void OnDisable()
	{
		user.onPaddlesPicked -= OnUserPickedPaddles;
		ApplicationData.onDataUpdated -= SetEvaluations;
	}

	private void Awake()
	{
		patternCommands = data.commands;
		_motionMagnitudeThreshold *= _motionMagnitudeThreshold;
		
		SetEvaluations(data);
		/// Ye olde Mode:
		//this.StartCoroutine(RecognizePattern(), ref patternEvaluator);
	}

	private void Update()
	{
		ClassifyPatterns();
	}
	
	private void ClassifyPatterns()
	{
		if(user.hasPaddles)
		foreach(PatternClassificationResult evaluation in evaluations)
		{
			if(!evaluation.started) evaluation.MoveNext();
			else
			{
				if(PaddlesOnWaypoints(evaluation.Current))
				{
					if(!evaluation.MoveNext() /*&& evaluation.finished)
					{
						OnPatternRecognized(evaluation.classification.command);
						ResetEvaluations();
					}
					else evaluation.ResetToleration();
				}
				else if((PaddlesOnWaypoints(evaluation.Previous) || UserHandsAreMoving()) && evaluation.TolerationEnded()) evaluation.Reset();
				else if(!UserHandsAreMoving()) ResetEvaluations();
			}
		}
	}

	/// <summary>Resets all classification's evaluations.</summary>
	private void ResetEvaluations()
	{
		foreach(PatternClassificationResult evaluation in evaluations)
		{
			evaluation.Reset();
		}
	}

	private void SetEvaluations(ApplicationData _data)
	{
		evaluations = new List<PatternClassificationResult>(_data.classifications.Length);

		for(int i = 0; i < _data.classifications.Length; i++)
		{
			evaluations.Add(new PatternClassificationResult(_data.classifications[i], waitTolerance));	
		}
	}

	private bool PaddlesOnWaypoints(UserPatternWaypoints _waypoints)
	{
		return PaddleOnWaypoint(user.rightHand.paddle, _waypoints.rightWaypoint) && PaddleOnWaypoint(user.leftHand.paddle, _waypoints.leftWaypoint);
	}

	private bool UserHandsAreMoving()
	{
		return (user.rightHand.body.velocity.sqrMagnitude > (motionMagnitudeThreshold * Time.deltaTime) || user.leftHand.body.velocity.sqrMagnitude > (motionMagnitudeThreshold * Time.deltaTime));
	}

	/// <summary>Evaluates if Paddle is inside Pattern's Waypoint.</summary>
	/// <param name="_paddle">Paddle to evaluate.</param>
	/// <param name="_waypoint">Current Pattern's Waypoint.</param>
	/// <returns>True if paddle is inside Waypoint's radius, false otherwise.</returns>
	private bool PaddleOnWaypoint(LandingPaddle _paddle, PatternWaypoint _waypoint)
	{
		return ((_paddle.GetOffsetTipPosition() - user.eye.TransformPoint(_waypoint.offsetPoint)).sqrMagnitude <= (_waypoint.toleranceRadius * _waypoint.toleranceRadius));
	}

	private IEnumerator<PatternClassification> PatternWaypointsIterator(PatternClassification _classification)
	{
		foreach(UserPatternWaypoints userWaypoints in _classification.waypoints)
		{
			yield return null;
		}
	}

	private IEnumerator<bool> FuchiPopo(PatternClassification _classification)
	{
		foreach(UserPatternWaypoints userWaypoints in _classification.waypoints)
		{
			yield return false;
		}
	}

	private void OnUserPickedPaddles(bool _picked)
	{
		ResetEvaluations();
		/*switch(_picked)
		{
			case true:
			this.StartCoroutine(RecognizePattern(), ref patternEvaluator);
			break;

			case false:
			if(onPatternRecognized != null) onPatternRecognized(default(Command));
			this.DispatchCoroutine(ref patternEvaluator);
			break;
		}
	}

#region YeOldeWay:
	private OrientationSemantics GetOrientation(Transform _centralJoint, Vector3 _childJointPosition, Vector3? _distanceThreshold = null)
	{
		OrientationSemantics orientation = OrientationSemantics.None;
		Vector3 relativePoint = _centralJoint.InverseTransformPoint(_childJointPosition);
		if(!_distanceThreshold.HasValue) _distanceThreshold = new Vector3(0.0f, 0.0f);

		orientation |= relativePoint.x > _distanceThreshold.Value.x ? OrientationSemantics.Right : relativePoint.x < -_distanceThreshold.Value.x ? OrientationSemantics.Left : OrientationSemantics.None;
		orientation |= relativePoint.y > _distanceThreshold.Value.y ? OrientationSemantics.Up : relativePoint.y < -_distanceThreshold.Value.y ? OrientationSemantics.Down : OrientationSemantics.None;
		orientation |= relativePoint.z > _distanceThreshold.Value.z ? OrientationSemantics. Forward : relativePoint.z < -_distanceThreshold.Value.z ? OrientationSemantics.Back : OrientationSemantics.None;

		return orientation;
	}

	/// \TODO Mark Method as deprecated.
	/// <summary>Evaluates set of PatternData's points.</summary>
	/// <param name="_patternPoints">Set of PatternData points to evaluate.</param>
	private void EvaluatePattern(List<PatternData> _patternPoints)
	{
		bool registeredFirstKey = false;
		bool registeredMiddleKey = false;
		bool registeredFinalKey = false;

		foreach(PatternCommand command in patternCommands)
		{
			PatternData[] middlePoses = null;
			PatternData firstPose = command.keyPoses[0];
			PatternData? finalPose = null;
			registeredFirstKey = false;
			registeredMiddleKey = false;
			registeredFinalKey = false;
			int currentUnregisteredMiddleKey = 0;
			
			if(command.keyPoses.Length > 1) finalPose = command.keyPoses[command.keyPoses.Length - 1];
			if(command.keyPoses.Length > 2)
			{
				middlePoses = new PatternData[command.keyPoses.Length - 2];
				for(int i = 1; i < (command.keyPoses.Length - 1); i++)
				{
					middlePoses[i - 1] = command.keyPoses[i];
				}
			}

			for(int i = 0; i < _patternPoints.Count; i++)
			{
				PatternData data = _patternPoints[i];

				if(_patternPoints.Count < command.keyPoses.Length) continue;

				if(!registeredFirstKey)
				{
					if(data == firstPose)
					{
						registeredFirstKey = true;
						if(!finalPose.HasValue)
						{
							OnPatternRecognized(command.command);
							return;
						}
						continue;
					}
				}
				if((middlePoses != null) && registeredFirstKey && (currentUnregisteredMiddleKey < middlePoses.Length))
				{
					if(data == middlePoses[currentUnregisteredMiddleKey])
					{
						currentUnregisteredMiddleKey++;
						registeredMiddleKey = !(currentUnregisteredMiddleKey < middlePoses.Length);
						continue;
					}
				}	
				if(finalPose.HasValue && !registeredFinalKey && registeredFirstKey && (middlePoses != null && registeredMiddleKey || middlePoses == null))
				{
					if(data == finalPose)
					{
						registeredFinalKey = true;
						OnPatternRecognized(command.command);
						return;
					}
				}
			}
		}

		if(!user.hasPaddles) this.DispatchCoroutine(ref patternEvaluator);
		else this.StartCoroutine(RecognizePattern(), ref patternEvaluator);
	}

	private void OnPatternRecognized(Command _command)
	{
		if(onPatternRecognized != null) onPatternRecognized(_command);

		/*if(!user.hasPaddles) this.DispatchCoroutine(ref patternEvaluator);
		else this.StartCoroutine(RecognizePattern(), ref patternEvaluator);
	}


	/// <summary>Gets Pattern Data from given User.</summary>
	/// <param name="User">User to retrive PatternData from.</param>
	/// <returns>User's PatternData.</returns>
	private PatternData RetrieveUserPattern(/*User _user)
	{
		/*return new PatternData
		(
			GetOrientation(_user.torax, _user.eye.position, distanceThreshold),
			GetOrientation(_user.torax, _user.leftHand.transform.position, distanceThreshold),
			GetOrientation(_user.leftHand.transform, _user.leftHand.GetRelativePaddlePoint()),
			GetOrientation(_user.torax, _user.rightHand.transform.position, distanceThreshold),
			GetOrientation(_user.rightHand.transform, _user.rightHand.GetRelativePaddlePoint())
		);
		return default(PatternData);
	}

	/// <summary>Records the User's Pattern each update cycle.</summary>
	private IEnumerator RecognizePattern()
	{
		patternsData = new List<PatternData>();
		float? motionMagnitude = null;
		Vector3 averageMotionPosition = Vector3.zero;

		while((!motionMagnitude.HasValue || motionMagnitude.Value > motionMagnitudeThreshold * Time.deltaTime) && user.hasPaddles)
		{
			WaitForSeconds wait = new WaitForSeconds(motionUpdateCycle);
			
			patternsData.Add(RetrieveUserPattern(user));

			if(patternsData.Count > 1) motionMagnitude = (averageMotionPosition - user.GetAverageJointPoint()).sqrMagnitude;
			averageMotionPosition = user.GetAverageJointPoint();
			
			yield return wait;
		}

		//yield return new WaitForSeconds(stationaryMotionTolerance);
		EvaluatePattern(patternsData);
	}
#endregion*/
}
}