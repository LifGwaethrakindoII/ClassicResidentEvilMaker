using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CanEditMultipleObjects]
[CustomEditor(typeof(TransformDeltaCalculator))]
public class TransformDeltaCalculatorInspector : Editor
{
	private TransformDeltaCalculator transformDeltaCalculator; 	/// <summary>Inspector's Target.</summary>
	private Transform transform; 								/// <summary>TransformDeltaCalculator's Transform.</summary>

	/// <summary>Sets target property.</summary>
	private void OnEnable()
	{
		transformDeltaCalculator = target as TransformDeltaCalculator;
		transform = transformDeltaCalculator.transform;
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		DrawDefaultInspector();
	}

	/// <summary>Enables the Editor to handle an event in the Scene view.</summary>
	public void OnSceneGUI()
	{
		float alpha = transformDeltaCalculator.rotationColorAlpha;
		Vector3 eulerAngles = transformDeltaCalculator.angularVelocity;

		Handles.color = Color.red.WithAlpha(alpha);
		Handles.DrawSolidArc(transform.position, transform.up, transform.right, eulerAngles.x, transformDeltaCalculator.gizmosRadius);
		Handles.color = Color.green.WithAlpha(alpha);
		Handles.DrawSolidArc(transform.position, transform.up, transform.right, eulerAngles.y, transformDeltaCalculator.gizmosRadius);
		Handles.color = Color.blue.WithAlpha(alpha);
		Handles.DrawSolidArc(transform.position, transform.right, transform.forward, eulerAngles.z, transformDeltaCalculator.gizmosRadius);
	}
}
}