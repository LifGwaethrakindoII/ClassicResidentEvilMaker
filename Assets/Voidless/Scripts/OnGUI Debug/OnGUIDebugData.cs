using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class OnGUIDebugData
{
	public Component mainComponent; 	/// <summary>Main Component's Reference.</summary>
	public Component[] components; 		/// <summary>Main Component's Components.</summary>
	public Vector2 scrollPosition; 		/// <summary>Scroll Position's Reference.</summary>
	public int componentIndex; 			/// <summary>Current Component's Index [starting as '-1' as default].</summary>

	/// <summary>OnGUIDebugData's Constructor.</summary>
	/// <param name="_mainComponent">Main Component.</param>
	/// <param name="_components">Main Component's Components.</param>
	public OnGUIDebugData(Component _mainComponent, Component[] _components)
	{
		mainComponent = _mainComponent;
		components = _components;
		scrollPosition = Vector2.zero;
		componentIndex = -1;
	}
}
}