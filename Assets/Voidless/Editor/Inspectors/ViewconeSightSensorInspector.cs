using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomEditor(typeof(ViewconeSightSensor))]
public class ViewconeSightSensorInspector : Editor
{
	private ViewconeSightSensor viewconeSightSensor; 	/// <summary>Inspector's Target.</summary>
	private Transform transform; 						/// <summary>ViewconeSightSensor's Transform.</summary>

	/// <summary>Sets target property.</summary>
	private void OnEnable()
	{
		viewconeSightSensor = target as ViewconeSightSensor;
		transform = viewconeSightSensor.transform;
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		DrawDefaultInspector();
		
	}

	/// <summary>Enables the Editor to handle an event in the Scene view.</summary>
	public void OnSceneGUI()
	{
		float halfAngleZ = viewconeSightSensor.angleZ * 0.5f;
		float halfAngleX = viewconeSightSensor.angleX * 0.5f;

		Handles.color = viewconeSightSensor.color;
		Handles.DrawSolidArc(transform.position, transform.up, transform.forward, halfAngleZ, viewconeSightSensor.sphereCollider.radius);
		Handles.DrawSolidArc(transform.position, transform.up, transform.forward, -halfAngleZ, viewconeSightSensor.sphereCollider.radius);
		Handles.DrawSolidArc(transform.position, transform.right, transform.forward, halfAngleX, viewconeSightSensor.sphereCollider.radius);
		Handles.DrawSolidArc(transform.position, transform.right, transform.forward, -halfAngleX, viewconeSightSensor.sphereCollider.radius);
		Handles.color = viewconeSightSensor.color.WithAlpha(1.0f);
		Handles.DrawSolidArc(transform.position, transform.up, transform.forward, halfAngleZ, viewconeSightSensor.nearPlane);
		Handles.DrawSolidArc(transform.position, transform.up, transform.forward, -halfAngleZ, viewconeSightSensor.nearPlane);
		Handles.DrawSolidArc(transform.position, transform.right, transform.forward, halfAngleX, viewconeSightSensor.nearPlane);
		Handles.DrawSolidArc(transform.position, transform.right, transform.forward, -halfAngleX, viewconeSightSensor.nearPlane);
	}
}
}