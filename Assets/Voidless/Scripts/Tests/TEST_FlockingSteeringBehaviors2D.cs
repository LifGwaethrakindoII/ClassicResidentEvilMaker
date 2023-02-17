using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

using Random = UnityEngine.Random;

public enum SteeringBehaviorTestingType
{
	Wander,
	Flocking
}

[RequireComponent(typeof(Boundaries2DContainer))]
public class TEST_FlockingSteeringBehaviors2D : MonoBehaviour
{
	[SerializeField] private SteeringBehaviorTestingType type; 		/// <summary>Testing's Type.</summary>
	[Space(5f)]
	[SerializeField] private SteeringVehicle2D reference; 			/// <summary>Vehicle's Reference for Prefab.</summary>
	[SerializeField] private int copies; 							/// <summary>Vehicle copies.</summary>
	[Space(5f)]
	[SerializeField] private float minDistanceToReachTarget; 		/// <summary>Minimum disrtance to reach target.</summary>
	[SerializeField] private float rotationSpeed; 					/// <summary>Vehicle's Rotation Speed.</summary>
	[Space(5f)]
	[Header("Flocking Weights:")]
	[SerializeField]
	[Range(0.0f, 5.0F)] private float waypointSeekWeight; 			/// <summary>Waypoints-Seek's Weight.</summary>
	[SerializeField]
	[Range(0.0f, 5.0f)] private float leaderSeekWeight; 			/// <summary>Leader-Seek's Weight.</summary>
	[SerializeField]
	[Range(0.0f, 5.0f)] private float separationWeight; 			/// <summary>Separation's Weight.</summary>
	[SerializeField]
	[Range(0.0f, 5.0f)] private float cohesionWeight; 				/// <summary>Cohesion's Weight.</summary>
	[SerializeField]
	[Range(0.0f, 5.0f)] private float allignmentWeight; 			/// <summary>Allignment's Weight.</summary>
	[Space(5f)]
	[Header("Rubberbanding Attributes:")]
	[SerializeField] private bool applyRubberbanding; 				/// <summary>Apply Rubberbanding?.</summary>
	[SerializeField] private FloatRange rubberbandingSpeedScale; 	/// <summary>Rubberbanding's Speed Range Scale.</summary>
	[SerializeField] private FloatRange rubberbandingForceScale; 	/// <summary>Rubberbanding's Force Range Scale.</summary>
	[SerializeField] private float rubberbandingRadius; 			/// <summary>Rubberbanding's Radius.</summary>
	[Space(5f)]
	[Header("Containment Attributes:")]
	[SerializeField] private float edgeDistanceTolerance; 			/// <summary>Edge distance tolerance [for boundaries].</summary>
	[SerializeField]
	[Range(0.0f, 5.0f)] private float containmentWeight; 			/// <summary>Containment's Weight.</summary>
	[Space(5f)]
	[Header("Wander Attributes:")]
	[SerializeField]
	[Range(0.0f, 5.0f)] private float wanderWeight; 				/// <summary>Wander's Weight.</summary>
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] private Color gizmosColor; 					/// <summary>Gizmos' Color.</summary>
	[SerializeField] private Color leaderColor; 					/// <summary>Leader's Color.</summary>
	[SerializeField] private Color leaderSeekForceColor; 			/// <summary>Leader-Seek Force's Color.</summary>
	[SerializeField] private Color separationForceColor; 			/// <summary>Separarion Force's Color.</summary>
	[SerializeField] private Color cohesionForceColor; 				/// <summary>Cohesion Force's Color.</summary>
	[SerializeField] private Color allignmentForceColor; 			/// <summary>Allignment Force's Color.</summary>
	[SerializeField] private float leaderSphereRadius; 				/// <summary>Leader Sphere's Radius.</summary>
	private SteeringVehicle2D main; 								/// <summary>Main's Steering Vehicle 2D.</summary>
	private List<SteeringVehicle2D> vehicles; 						/// <summary>Subordinate Vehicles.</summary>
	private Boundaries2DContainer _boundariesContainer; 				/// <summary>BoundariesContainer's Component.</summary>
	private Vector3[] waypoints; 									/// <summary>Waypoints.</summary>
	private IEnumerator<Vector3> waypointsIterator; 				/// <summary>Waypoints' Iterator.</summary>
	private Vector2 scrollPosition;

	/// <summary>Gets boundariesContainer Component.</summary>
	public Boundaries2DContainer boundariesContainer
	{ 
		get
		{
			if(_boundariesContainer == null) _boundariesContainer = GetComponent<Boundaries2DContainer>();
			return _boundariesContainer;
		}
	}

	/// <summary>OnGUI is called for rendering and handling GUI events.</summary>
	private void OnGUI()
	{
		GUILayout.Label("Edge Distance Tolerance: " + edgeDistanceTolerance);
		edgeDistanceTolerance = GUILayout.HorizontalSlider(edgeDistanceTolerance, 0.0f, 25.0f);
		GUILayout.Label("Containment Weight: " + containmentWeight);
		containmentWeight = GUILayout.HorizontalSlider(containmentWeight, 0.0f, 5.0f);

		switch(type)
		{
			case SteeringBehaviorTestingType.Wander:
				WanderTestingOnGUI();
			break;

			case SteeringBehaviorTestingType.Flocking:
				FlockingTestingOnGUI();
			break;
		}
	}

	/// <summary>OnGUI for Wander Testing.</summary>
	private void WanderTestingOnGUI()
	{
		GUILayout.Label("Wander Weight: " + wanderWeight);
		wanderWeight = GUILayout.HorizontalSlider(wanderWeight, 0.0f, 5.0f);
	}

	/// <summary>OnGUI for Flocking Testing.</summary>
	private void FlockingTestingOnGUI()
	{
		float newMaxSpeed = main.maxSpeed;
		float newMaxForce = main.maxForce;
		int newReproductions = copies;

		GUILayout.Label("Waypoint-Seek Weight: " + waypointSeekWeight);
		waypointSeekWeight = GUILayout.HorizontalSlider(waypointSeekWeight, 0.0f, 5.0f);
		GUILayout.Label("Leader-Seek Weight: " + leaderSeekWeight);
		leaderSeekWeight = GUILayout.HorizontalSlider(leaderSeekWeight, 0.0f, 5.0f);
		GUILayout.Label("Separation Weight: " + separationWeight);
		separationWeight = GUILayout.HorizontalSlider(separationWeight, 0.0f, 5.0f);
		GUILayout.Label("Cohesion Weight: " + cohesionWeight);
		cohesionWeight = GUILayout.HorizontalSlider(cohesionWeight, 0.0f, 5.0f);
		GUILayout.Label("Allignment Weight: " + allignmentWeight);
		allignmentWeight = GUILayout.HorizontalSlider(allignmentWeight, 0.0f, 5.0f);
		GUILayout.Label("Max-Speed: " + newMaxSpeed);
		newMaxSpeed = GUILayout.HorizontalSlider(newMaxSpeed, 0.0f, 100.0f);
		GUILayout.Label("Max-Force: " + newMaxForce);
		newMaxForce = GUILayout.HorizontalSlider(newMaxForce, 0.0f, 100.0f);
		GUILayout.Label("Reproductions: ");
		newReproductions = VGUILayout.IntField(newReproductions);
		applyRubberbanding = GUILayout.Toggle(applyRubberbanding, "Rubberbanding " + (applyRubberbanding ? "Activated" : "Deactivated"));

		if(vehicles != null && applyRubberbanding)
		{
			int i = 0;
			float distance = 0.0f;
			float t = 0.0f;

			GUILayout.Label("Rubberbanding's Radius: " + rubberbandingRadius);
			rubberbandingRadius = GUILayout.HorizontalSlider(rubberbandingRadius, 0.0f, 50.0f);
			GUILayout.Label("Min Speed's Scalar: " + rubberbandingSpeedScale.Min());
			rubberbandingSpeedScale.min = GUILayout.HorizontalSlider(rubberbandingSpeedScale.min, 0.0f, 10.0f);
			GUILayout.Label("Max Speed's Scalar: " + rubberbandingSpeedScale.Max());
			rubberbandingSpeedScale.max = GUILayout.HorizontalSlider(rubberbandingSpeedScale.max, rubberbandingSpeedScale.min, 10.0f);
			GUILayout.Label("Min Force's Scalar: " + rubberbandingForceScale.Min());
			rubberbandingForceScale.min = GUILayout.HorizontalSlider(rubberbandingForceScale.min, 0.0f, 10.0f);
			GUILayout.Label("Max Force's Scalar: " + rubberbandingForceScale.Max());
			rubberbandingForceScale.max = GUILayout.HorizontalSlider(rubberbandingForceScale.max, rubberbandingForceScale.min, 10.0f);

			scrollPosition = GUILayout.BeginScrollView(scrollPosition);
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("Vehicle #");
			GUILayout.Label("Distance");
			GUILayout.Label("Rubberbanding");
			GUILayout.Label("Speed");
			GUILayout.Label("Force");
			GUILayout.EndHorizontal();

			foreach(SteeringVehicle2D vehicle in vehicles)
			{
				if(vehicle == main) continue;

				distance = (main.transform.position - vehicle.transform.position).magnitude;
				t = Mathf.Min(distance, rubberbandingRadius) / rubberbandingRadius;

				GUILayout.BeginHorizontal();
				GUILayout.Label(i.ToString());
				GUILayout.Label(distance.ToString());
				GUILayout.Label(t.ToString());
				GUILayout.Label(rubberbandingSpeedScale.Lerp(t).ToString());
				GUILayout.Label(rubberbandingForceScale.Lerp(t).ToString());
				GUILayout.EndHorizontal();

				i++;
			}

			GUILayout.EndScrollView();
		}

		if(main.maxSpeed != newMaxSpeed || main.maxForce != newMaxForce)
		{
			if(vehicles != null)
			foreach(SteeringVehicle2D vehicle in vehicles)
			{
				vehicle.maxSpeed = newMaxSpeed;
				vehicle.maxForce = newMaxForce;
			}
		}

		if(newReproductions != copies)
		{
			copies = newReproductions;
			UpdateNeighborhood();
		}
	}

	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		/// Draw Boundary's Limits:
		Vector3 min = boundariesContainer.min;
		Vector3 max = boundariesContainer.max;

		min.x += edgeDistanceTolerance;
		min.y += edgeDistanceTolerance;
		max.x -= edgeDistanceTolerance;
		max.y -= edgeDistanceTolerance;

		Vector3 bottomLeftPoint = min;
		Vector3 bottomRightPoint = new Vector3(max.x, min.y, min.z);
		Vector3 topLeftPoint = new Vector3(min.x, max.y, min.z);
		Vector3 topRightPoint = max;
		
		Gizmos.DrawLine(bottomLeftPoint, bottomRightPoint);
		Gizmos.DrawLine(bottomLeftPoint, topLeftPoint);
		Gizmos.DrawLine(topLeftPoint, topRightPoint);
		Gizmos.DrawLine(bottomRightPoint, topRightPoint);

		switch(type)
		{
			case SteeringBehaviorTestingType.Flocking:
				if(main != null && applyRubberbanding)
				{
					Gizmos.color = leaderColor;
					Gizmos.DrawWireSphere(main.transform.position, rubberbandingRadius);
				}

				Gizmos.color = gizmosColor;

				if(waypoints == null) return;

				float radius = minDistanceToReachTarget * minDistanceToReachTarget;

				foreach(Vector3 waypoint in waypoints)
				{
					Gizmos.DrawWireSphere(waypoint, radius);
				}
			break;
		}
	}

#region UnityMethods:
	/// <summary>TEST_FlockingSteeringBehaviors2D's instance initialization.</summary>
	private void Awake()
	{
		switch(type)
		{
			case SteeringBehaviorTestingType.Wander:
				SteeringVehicle2D vehicle = Instantiate(reference, boundariesContainer.Random(), Quaternion.identity) as SteeringVehicle2D;
				if(vehicle != null)
				{
					main = vehicle;
					StartCoroutine(WanderBehaviour(vehicle));
				}
			break;

			case SteeringBehaviorTestingType.Flocking:
				waypointsIterator = WaypointsIterator();	
				UpdateNeighborhood();		
			break;
		}
	}

	/// <summary>TEST_FlockingSteeringBehaviors2D's starting actions before 1st Update frame.</summary>
	private void Start ()
	{
		//StartCoroutine(MainVehicleBehavior());
	}
	
	/// <summary>TEST_FlockingSteeringBehaviors2D's tick at each frame.</summary>
	private void Update()
	{
		if(main != null)
		{
			boundariesContainer.ContainInsideBoundaries(main.transform);
			
		}

		if(vehicles == null) return;

		foreach(SteeringVehicle2D vehicle in vehicles)
		{
			boundariesContainer.ContainInsideBoundaries(vehicle.transform);
			boundariesContainer.GetContainmentForce(vehicle, edgeDistanceTolerance);
		}
	}

	/// <summary>Updates TEST_FlockingSteeringBehaviors2D's instance at each Physics Thread's frame.</summary>
	private void FixedUpdate()
	{
		switch(type)
		{
			case SteeringBehaviorTestingType.Wander:
				
			break;

			case SteeringBehaviorTestingType.Flocking:
				UpdateVehicles();
				RubberbandingAdjustment();		
			break;
		}
	}
#endregion

	/// <summary>Updates Neighborhood's Vehicles.</summary>
	private void UpdateNeighborhood()
	{
		if(vehicles == null) vehicles = new List<SteeringVehicle2D>();

		copies = Mathf.Max(copies, 1);

		int difference = vehicles.Count - copies;

		if(difference < 0)
		{ /// Gotta Create:
			for(int i = 0; i < Mathf.Abs(difference); i++)
			{
				SteeringVehicle2D vehicle = Instantiate(reference, boundariesContainer.Random(), Quaternion.identity) as SteeringVehicle2D;
				vehicles.Add(vehicle);
			}
		}
		else
		{ /// Gotta Remove:
			for(int i = vehicles.Count - 1; i > copies - 1; i--)
			{
				Destroy(vehicles[i].gameObject);
			}

			vehicles.Resize(copies);
		}

		main = vehicles[0];
	}

	/// <summary>Applies flocking to all neighborhood vehicles.</summary>
	private void UpdateVehicles()
	{
		waypointsIterator.MoveNext();

		foreach(SteeringVehicle2D vehicle in vehicles)
		{
			bool isMain = vehicle == main;
			float weight = (!isMain ? leaderSeekWeight : waypointSeekWeight);
			Vector2 target = (Vector2)(!isMain ? main.transform.position : (waypointsIterator.Current));
			Vector2 mainSeekForce = vehicle.GetSeekForce(target) * weight;
			Vector2 containmentForce = boundariesContainer.GetContainmentForce(vehicle, edgeDistanceTolerance) * containmentWeight;
			Vector2 separationForce = Vector2.zero; 
			Vector2 cohesionForce = Vector2.zero; 
			Vector2 allignments = Vector2.zero; 
			Vector3 sum = Vector3.zero;
			
			if(!isMain)
			{
				separationForce = vehicle.GetSeparationForce(vehicles) * separationWeight;
				cohesionForce = vehicle.GetCohesionForce(vehicles) * cohesionWeight;
				allignments = vehicle.GetAllignment(vehicles) * allignmentWeight;	
			}

			sum = (Vector3)(mainSeekForce + separationForce + cohesionForce + allignments + containmentForce);
			/*vehicle.ApplyForce(mainSeekForce);
			vehicle.ApplyForce(separationForce);
			vehicle.ApplyForce(cohesionForce);
			vehicle.ApplyForce(allignments);*/
			vehicle.ApplyForce(sum);
			vehicle.Displace(Time.fixedDeltaTime);
			vehicle.Rotate(Time.fixedDeltaTime);

#if UNITY_EDITOR
			Debug.DrawRay(vehicle.transform.position, Vector3.ClampMagnitude(mainSeekForce, vehicle.maxSpeed), leaderSeekForceColor);
			Debug.DrawRay(vehicle.transform.position, Vector3.ClampMagnitude(separationForce, vehicle.maxSpeed), separationForceColor);
			Debug.DrawRay(vehicle.transform.position, Vector3.ClampMagnitude(cohesionForce, vehicle.maxSpeed), cohesionForceColor);
			Debug.DrawRay(vehicle.transform.position, Vector3.ClampMagnitude(allignments, vehicle.maxSpeed), allignmentForceColor);
			Debug.DrawRay(vehicle.transform.position, Vector3.ClampMagnitude(vehicle.GetVelocity(), vehicle.maxSpeed), Color.white);
			Debug.DrawRay(vehicle.transform.position, sum, Color.yellow);
#endif
		}
	}

	/// <summary>Adjusts the max-speed of neighborhood vehicles by rubberbanding.</summary>
	private void RubberbandingAdjustment()
	{
		if(!applyRubberbanding) return;

		Vector3 direction = Vector3.zero;
		float m = 0.0f;
		float r = rubberbandingRadius * rubberbandingRadius;
		float t = 0.0f;

		foreach(SteeringVehicle2D vehicle in vehicles)
		{
			if(vehicle == main) continue;

			direction = main.GetPosition() - vehicle.GetPosition();
			m = direction.sqrMagnitude;
			t = Mathf.Min(m, r) / r;
			t = VMath.Sigmoid(t);

			vehicle.maxSpeed = main.maxSpeed * rubberbandingSpeedScale.Lerp(t);
			vehicle.maxForce = main.maxForce * rubberbandingForceScale.Lerp(t);
		}
	}

	/// <summary>Main Vehicle's Behavior [wander inside boundaries' area].</summary>
	private IEnumerator MainVehicleBehavior()
	{
		float distance = minDistanceToReachTarget * minDistanceToReachTarget;

		while(true)
		{
			int size = Random.Range(5, 10);
			waypoints = new Vector3[size];

			for(int i = 0; i < size; i++)
			{
				waypoints[i] = boundariesContainer.Random();
			}

			foreach(Vector3 waypoint in waypoints)
			{
				Vector3 direction = waypoint - main.transform.position;

				while(direction.sqrMagnitude > distance)
				{
					Vector3 seekForce = main.GetSeekForce(waypoint);
					main.transform.position += seekForce * Time.deltaTime;
					main.transform.rotation = Quaternion.RotateTowards(main.transform.rotation, Quaternion.LookRotation(seekForce), rotationSpeed * Time.deltaTime);
					direction = waypoint - main.transform.position;

					yield return null;
				}
			}
		}
	}

	/// <summary>Waypoints' Iterator.</summary>
	private IEnumerator<Vector3> WaypointsIterator()
	{
		float distance = minDistanceToReachTarget * minDistanceToReachTarget;

		while(true)
		{
			int size = Random.Range(5, 10);
			waypoints = new Vector3[size];

			for(int i = 0; i < size; i++)
			{
				waypoints[i] = boundariesContainer.Random();
			}

			foreach(Vector3 waypoint in waypoints)
			{
				Vector3 direction = waypoint - main.transform.position;

				while(direction.sqrMagnitude > distance)
				{
					yield return waypoint;

					direction = waypoint - main.transform.position;
				}
			}
		}
	}

	/// <summary>Wander Behavior.</summary>
	/// <param name="_vehicle">Vehicle thaat will perform the Wander Behavior.</param>
	private IEnumerator WanderBehaviour(SteeringVehicle2D _vehicle)
	{
		FloatRange waitInterval = new FloatRange(0.3f, 0.65f);
		SecondsDelayWait wait = new SecondsDelayWait(waitInterval.Random());
		Vector2 target = Vector2.zero;
		Vector2 direction = Vector2.zero;
		Vector2 force = Vector2.zero;
		float distance = minDistanceToReachTarget * minDistanceToReachTarget;

		while(true)
		{
			target = _vehicle.GetWanderPoint();
			direction = target - _vehicle.GetPosition();

			while(direction.sqrMagnitude > distance && wait.MoveNext())
			{
				force = boundariesContainer.GetContainmentForce(_vehicle, edgeDistanceTolerance) * containmentWeight;
				if(force.sqrMagnitude == 0.0f) force += _vehicle.GetSeekForce(target) * wanderWeight;

#if UNITY_EDITOR
				Debug.DrawRay(_vehicle.transform.position, force, Color.cyan);
#endif

				/*if(force.sqrMagnitude > 0.0f)
				{*/
					_vehicle.ApplyForce(force);
					_vehicle.Displace(Time.fixedDeltaTime);
					_vehicle.Rotate(Time.fixedDeltaTime);
				//}

				direction = target - _vehicle.GetPosition();

				yield return VCoroutines.WAIT_PHYSICS_THREAD;
			}

			wait.ChangeDurationAndReset(waitInterval.Random());

			yield return VCoroutines.WAIT_PHYSICS_THREAD;
		}
	}
}