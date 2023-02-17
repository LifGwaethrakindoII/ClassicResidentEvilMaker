using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Voidless;

namespace Timenauts
{
[CustomEditor(typeof(ConstrainedJoint))]
public class ConstrainedJointInspector : Editor
{
	private const float ALPHA_COLOR = 0.5f; 	/// <summary>Alpha's Color.</summary>

	private ConstrainedJoint constrainedJoint; 	/// <summary>Inspector's Target.</summary>
	private Transform transform; 				/// <summary>Target's Transform.</summary>

	/// <summary>Sets target property.</summary>
	private void OnEnable()
	{
		constrainedJoint = target as ConstrainedJoint;
		//transform = constrainedJoint.transform;
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		DrawDefaultInspector();
	}

	/// <summary>Enables the Editor to handle an event in the Scene view.</summary>
	public void OnSceneGUI()
	{
		//transform = constrainedJoint.joint;
		

		if(transform == null) return;

		/*Handles.color = Color.red.WithAlpha(ALPHA_COLOR);
		Handles.DrawSolidArc(transform.position, transform.right, transform.forward, constrainedJoint.xLimits.Min(), constrainedJoint.gizmosRadius);
		Handles.DrawSolidArc(transform.position, transform.right, transform.forward, constrainedJoint.xLimits.Max(), constrainedJoint.gizmosRadius);
		Handles.color = Color.green.WithAlpha(ALPHA_COLOR);
		Handles.DrawSolidArc(transform.position, transform.up, transform.forward, constrainedJoint.yLimits.Min(), constrainedJoint.gizmosRadius);
		Handles.DrawSolidArc(transform.position, transform.up, transform.forward, constrainedJoint.yLimits.Max(), constrainedJoint.gizmosRadius);
		Handles.color = Color.blue.WithAlpha(ALPHA_COLOR);
		Handles.DrawSolidArc(transform.position, transform.forward, transform.up, constrainedJoint.zLimits.Min(), constrainedJoint.gizmosRadius);
		Handles.DrawSolidArc(transform.position, transform.forward, transform.up, constrainedJoint.zLimits.Max(), constrainedJoint.gizmosRadius);*/
	}
}
}