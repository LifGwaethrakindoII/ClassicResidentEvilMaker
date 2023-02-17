using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomEditor(typeof(VirtualTransform))]
public class VirtualTransformInspector : Editor
{
	private VirtualTransform virtualTransform; 	/// <summary>Inspector's Target.</summary>

	/// <summary>Sets target property.</summary>
	void OnEnable()
	{
		virtualTransform = target as VirtualTransform;
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		DrawDefaultInspector();
		if(Application.isPlaying)
		{
			if(GUILayout.Button("Register Parents")) virtualTransform.RegisterParents();
			EditorGUILayout.Space();
			if(virtualTransform.virtualParents != null)
			{
				for(int i = 0; i < virtualTransform.virtualParents.Count; i++)
				{
					VirtualTransform parent = virtualTransform.virtualParents[i];
					if(GUILayout.Button("Remove Parent " + virtualTransform.gameObject.name)) virtualTransform.RemoveParent(parent);
				}
			}
			
		}
			
	}
}
}