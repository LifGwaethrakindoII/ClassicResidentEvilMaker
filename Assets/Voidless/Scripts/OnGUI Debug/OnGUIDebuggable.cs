using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class OnGUIDebuggable : MonoBehaviour
{
	protected static readonly Camera camera;  		/// <summary>Camera's Reference.</summary>

	[TextArea(5, 30)] 
	[SerializeField] private string _debugText; 	/// <summary>Debug's Text.</summary>
	[SerializeField] private Rect _rect; 			/// <summary>GUI's Rect.</summary>
	private List<MonoBehaviour> _monoBehaviours; 	/// <summary>MonoBehaviours contained on this GameObject.</summary>

	/// <summary>Static Constructor.</summary>
	static OnGUIDebuggable()
	{
		camera = Camera.main;	
	}

	/// <summary>Gets and Sets monoBehaviours property.</summary>
	public List<MonoBehaviour> monoBehaviours
	{
		get { return _monoBehaviours; }
		set { _monoBehaviours = value; }
	}

	/// <summary>Gets and Sets debugText property.</summary>
	public string debugText
	{
		get { return _debugText; }
		set { _debugText = value; }
	}

	/// <summary>Callback invoked when OnGUIDebuggable's instance is enabled.</summary>
	private void OnEnable()
	{
		OnGUIDebugger.AddObject(this);
	}

	/// <summary>Callback invoked when OnGUIDebuggable's instance is disabled.</summary>
	private void OnDisable()
	{
		OnGUIDebugger.RemoveObject(this);
	}

	private void OnGUI()
	{
		/*Vector3 direction = (transform.position - camera.transform.position).normalized;

		if(Vector3.Dot(camera.transform.forward, direction) < 0.0f) return;

		if(monoBehaviours == null || monoBehaviours.Count == 0) return;

		Vector3 screenPoint = Vector3.zero;
		Vector2 scrollRectSize = Vector2.zero;

		foreach(MonoBehaviour monoBehaviour in monoBehaviours)
		{

		}*/
	}

	/// <summary>OnGUIDebugData's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		monoBehaviours = new List<MonoBehaviour>();

		foreach(Component component in GetComponents<Component>())
		{
			MonoBehaviour monoBehaviour = component as MonoBehaviour;
			
			if(monoBehaviour != null && component != this)
			monoBehaviours.Add(monoBehaviour);
		}
	}

	/// <returns>String representing this debug text.</returns>
	public override string ToString()
	{
		return debugText;
	}
}
}