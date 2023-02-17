using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// TODO: Housekeeping Tool:
/*
	- https://stackoverflow.com/questions/40172220/unity3d-editor-how-can-i-find-all-usages-of-a-given-asset
	- https://docs.unity3d.com/ScriptReference/SceneManagement.Scene.html
	- https://docs.unity3d.com/ScriptReference/SceneManagement.Scene.GetRootGameObjects.html
*/

namespace Voidless
{
	public struct NodeRecord
	{
		public BaseNode<bool, float> current;
		public BaseNode<bool, float> connection;
		public float totalCost;
	}

public class DijkstraPathfinding : MonoBehaviour
{
	public GameObject[] dijkstraWaypoints;

	/// <summary>DijkstraPathfinding default constructor.</summary>
	public DijkstraPathfinding()
	{
		
	}

	public HashSet<BaseNode<bool, float>> CalculateRoute(BaseNode<bool, float> start, BaseNode<bool, float> end)
	{
		GameObject[] open = new GameObject[dijkstraWaypoints.Length];
		Array.Copy(dijkstraWaypoints, open, dijkstraWaypoints.Length);
		NodeRecord record;
		record.current = start;
		record.connection = null;
		record.totalCost = 0f;

		while(open.Length > 0)
		{

		}

		return null;
	}
}
}