using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CanEditMultipleObjects]
[CustomEditor(typeof(NormalizedPropertyFunction))]
public class NormalizedPropertyFunctionInspector : Editor
{
	private const float OFFSET_X = 20.0f;
	private const float OFFSET_Y = 20.0f;
	private const float HEIGHT_BOX = 200.0f;
	private const float WIDTH_MIN = 100.0f;
	private const float WIDTH_MAX = 1000.0f;
	private const float HEIGHT_MIN = 250.0f;
	private const float HEIGHT_MAX = 250.0f;
	private const int SPLITS_TIME = 40;

	private NormalizedPropertyFunction normalizedPropertyFunction; 	/// <summary>Inspector's Target.</summary>
	private Material material;

	/// <summary>Sets target property.</summary>
	void OnEnable()
	{
		normalizedPropertyFunction = target as NormalizedPropertyFunction;
		SetMaterial();
	}

	void OnDisable()
	{
		if(material != null) DestroyImmediate(material);
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		DrawDefaultInspector();
		PaintGraph();
	}

	protected void PaintGraph()
	{
		EditorGUILayout.Space();
		GUILayout.BeginHorizontal(EditorStyles.helpBox);
		Rect layout = GUILayoutUtility.GetRect(10, 1000, 200, 200);

		if(Event.current.type == EventType.Repaint)
		{
	        GUI.BeginClip(layout);
	        GL.PushMatrix();

	        GL.Clear(true, false, Color.black);
	        material.SetPass(0);

	        /*GL.Begin(GL.QUADS);
	        GL.Color(Color.black);
	        GL.Vertex3(0, 0, 0);
	        GL.Vertex3(layout.width, 0, 0);
	        GL.Vertex3(layout.width, layout.height, 0);
	        GL.Vertex3(0, layout.height, 0);
	        GL.End();*/

        	GL.Begin(GL.LINES);
        	GL.Color(Color.white);
        	GL.Vertex3(OFFSET_X, layout.height - OFFSET_Y, 0);
        	GL.Vertex3(layout.width - OFFSET_X, layout.height - OFFSET_Y, 0);
        	GL.End();

        	GL.Begin(GL.LINES);
        	GL.Color(Color.white);
        	GL.Vertex3(OFFSET_X, layout.height - OFFSET_Y, 0);
        	GL.Vertex3(OFFSET_X, OFFSET_Y, 0);
        	GL.End();

        	float split = 1.0f / (float)(SPLITS_TIME);
        	float progress = 0.0f;
        	GL.Begin(GL.LINES);
        	GL.Color(Color.white);
        	for(int i = 0; i < SPLITS_TIME; i++)
        	{
        		progress = (i + 1.0f) * split;
        		float t = normalizedPropertyFunction.Evaluate(progress);
        		float x = Mathf.Lerp(OFFSET_X, layout.width - OFFSET_X, t);
        		float y = Mathf.Lerp(layout.height - OFFSET_Y, OFFSET_Y, progress);
        		GL.Vertex3(x, y, 0);
        		//Debug.Log("[NormalizedPropertyFunctionInspector] PROGRESS: " + progress);
        	}
        	GL.End();

	        GL.PopMatrix();
	        GUI.EndClip();
		}

    	GUILayout.EndHorizontal();
	}

	protected void SetMaterial()
	{
		Shader shader = Shader.Find("Unlit/Color");
		material = new Material(shader);
		material.SetColor("_Color", Color.black);
	}
}
}