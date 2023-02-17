using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class OnGUIDebugger : Singleton<OnGUIDebugger>
{
	[SerializeField] private Camera camera; 				      	/// <summary>Main Camera.</summary>
	[SerializeField] private Vector2 offset; 				      	/// <summary>Box's offset from position.</summary>
	[SerializeField] private Vector2 dimensions; 			   	/// <summary>Scroll Area's Dimensions.</summary>
	[SerializeField] private float scrollOffset; 			   	/// <summary>ScrollBar's Offset.</summary>
	[SerializeField] private float buttonHeight; 			   	/// <summary>GUI Button's Height.</summary>
	[Space(5f)]
	[Header("GUI's Styles: ")]
	[SerializeField] private GUIStyle boxStyle; 			      	/// <summary>GUI's Box Style.</summary>
	[SerializeField] private GUIStyle buttonStyle; 			   	/// <summary>GUI's Button Style.</summary>
	private Dictionary<int, OnGUIDebugData> _dataDictionary; 	/// <summary>GUI Debug Dat's Dictionary.</summary>
	private Vector3 scrollPosition; 						         	/// <summary>GUI Scroll's Position.</summary>
	private float verticalScroll; 							      	/// <summary>Vertical Scroll's Value.</summary>
	private GUIContent content; 							         	/// <summary>GUI's Content.</summary>
	private bool showGUI; 									         	/// <summary>Show GUI.</summary>

	/// <summary>Gets dataDictionary property.</summary>
	public Dictionary<int, OnGUIDebugData> dataDictionary
	{
		get
		{
			if(_dataDictionary == null) _dataDictionary = new Dictionary<int, OnGUIDebugData>();
			return _dataDictionary;
		} 
	}

	/// <summary>Callback called on Awake if this Object is the Singleton's Instance.</summary>
	protected override void OnAwake()
	{
		showGUI = true;
		content = new GUIContent(string.Empty);
	}

	/// <summary>OnGUI is called for rendering and handling GUI events.</summary>
	private void OnGUI()
	{
		if(GUILayout.Button(showGUI ? "Hide" : "Show")) showGUI = !showGUI;

		if(!showGUI) return;

		Component component = null;
		Component[] components = null;
		Vector3 screenPoint = Vector3.zero;
		Vector2 point = Vector2.zero;
		Vector2 size = Vector2.zero;
		Vector2 scrollRectSize = Vector2.zero;
		float dotProduct = 0.0f;
		float halfWidth = 0.0f;
		float halfHeight = 0.0f;
		float accumulatedButtonHeight = 0.0f;
      float additionalY = 0.0f;
		string componentName = null;
      string text = null;

		foreach(OnGUIDebugData data in dataDictionary.Values)
		{
			component = data.mainComponent;

			if(!component.gameObject.activeSelf) continue;

			dotProduct = Vector3.Dot(camera.transform.forward, (component.transform.position - camera.transform.position));

			if(dotProduct < 0.0f) continue;

			components = data.components;

			size = GetBestSize(data/*, out additionalY*/);
			text = component.ToString();
			content.text = text;
			screenPoint = camera.WorldToScreenPoint(component.transform.position);
			halfWidth = size.x * 0.5f;
		   halfHeight = size.y * 0.5f;
			point.x = (screenPoint.x + offset.x - halfWidth);
			point.y = (screenPoint.y - offset.y - halfHeight);
			scrollRectSize.x = Mathf.Min(size.x, dimensions.x);
			scrollRectSize.y = Mathf.Min(size.y, dimensions.y);
			
			data.scrollPosition = GUI.BeginScrollView(new Rect(point.x, point.y, scrollRectSize.x - scrollOffset, scrollRectSize.y - scrollOffset), data.scrollPosition, new Rect(point.x, point.y, size.x, size.y));
			if(components != null)
			{
				if(GUI.Button(new Rect(point.x, point.y, size.x, buttonHeight), component.GetType().Name, buttonStyle))
				data.componentIndex = -1;

				point.y += buttonHeight;
				accumulatedButtonHeight += buttonHeight;

				for(int i = 0; i < components.Length; i++)
   			{
   				if(GUI.Button(new Rect(point.x, point.y, size.x, buttonHeight), components[i].GetType().Name, buttonStyle)) data.componentIndex = i;
   				point.y += buttonHeight;
   				accumulatedButtonHeight += buttonHeight;
   			}

   			if(data.componentIndex >= 0)
   			{
   				text = components[data.componentIndex].ToString();
   				content.text = text;
   				//size = boxStyle.CalcSize(content);
   			}

   			size.y -= accumulatedButtonHeight;			
			}
			GUI.Box(new Rect(point.x, point.y, size.x, size.y), text, boxStyle);
			GUI.EndScrollView();
		}
	}

	/// <summary>Gets best possible given all the components' content size.</summary>
	/// <param name="_data">Data to calculate the best size from.</param>
	/// <returns>Best size.</returns>
	private Vector2 GetBestSize(OnGUIDebugData _data/*, float _additionalY*/)
	{
		Vector2 size = new Vector2(Mathf.NegativeInfinity, Mathf.NegativeInfinity);
		Vector2 contentSize = Vector2.zero;
		string text = _data.ToString();
      float accumulatedButtonHeight = 0.0f;

		content.text = _data.mainComponent.ToString();
		contentSize = boxStyle.CalcSize(content);
		size.x = Mathf.Max(contentSize.x, size.x);
		size.y = Mathf.Max(contentSize.y, size.y);
      accumulatedButtonHeight += buttonHeight;

		foreach(Component component in _data.components)
		{
			content.text = component.ToString();
			contentSize = boxStyle.CalcSize(content);
			size.x = Mathf.Max(contentSize.x, size.x);
			size.y = Mathf.Max(contentSize.y, size.y);
         accumulatedButtonHeight += buttonHeight;
		}

		return size;
	}

   private float GetAdditionalSize(OnGUIDebugData _data)
   {
      float additionalSize = 0.0f;

      /*foreach(Component)
      {}*/

      return additionalSize;
   }

#region StaticMethods:
	/// <summary>Adds GUI Debug's Data.</summary>
	/// <param name="_component">Reference Component.</param>
	/// <param name="_components">Reference's Components.</param>
	public static void AddObject(Component _component, params Component[] _components)
	{
		if(Instance == null) return;

		Dictionary<int, OnGUIDebugData> dictionary = Instance.dataDictionary;
		int instanceID = _component.GetInstanceID();

		if(!dictionary.ContainsKey(instanceID)) dictionary.Add(instanceID, new OnGUIDebugData(_component, _components));
	}

	/// <summary>Adds GUI Debug's Data and Component.</summary>
	/// <param name="_component">Reference Component.</param>
	/// <param name="_components">Reference's Components.</param>
	public static void AddComponents(Component _component, params Component[] _components)
	{
		if(Instance == null) return;

		Dictionary<int, OnGUIDebugData> dictionary = Instance.dataDictionary;
		int instanceID = _component.GetInstanceID();

		if(!dictionary.ContainsKey(instanceID)) dictionary.Add(instanceID, new OnGUIDebugData(_component, _components));
		else dictionary[instanceID].components = _components;
	}

	/// <summary>Removes GUI Debug's Data.</summary>
	public static void RemoveObject(Component _component)
	{
		if(Instance == null) return;

		Dictionary<int, OnGUIDebugData> dictionary = Instance.dataDictionary;
		int instanceID = _component.gameObject.GetInstanceID();

		if(dictionary.ContainsKey(instanceID)) dictionary.Remove(instanceID);
	}
#endregion

}
}