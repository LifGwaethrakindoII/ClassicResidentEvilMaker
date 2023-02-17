using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VPatriarchy
{
	/// <summary>Gets all childs from parent.</summary>
	/// <param name="_transform">The Transform that owns the childs.</param>
	/// <returns>List of childs.</returns>
	public static List<Transform> GetChilds(this Transform _transform)
	{
		List<Transform> newList = new List<Transform>();

		foreach(Transform child in _transform)
		{
			newList.Add(child);
		}

		return newList;
	}

	/// <summary>Gets all childs from parent.</summary>
	/// <param name="_transform">The Transform that owns the childs.</param>
	/// <returns>List of childs.</returns>
	public static List<Transform> GetChildsWith<T>(this Transform _transform) where T : UnityEngine.Object
	{
		List<Transform> newList = new List<Transform>();

		foreach(Transform child in _transform)
		{
			if(child.gameObject.Has<T>()) newList.Add(child);		
		}

		return newList;
	}

	/// <summary>Destroys all childs from parent.</summary>
	/// <param name="_transform">The Transform that owns the childs.</param>
	public static void KillAllChilds(this Transform _transform)
	{
		foreach(Transform child in _transform)
		{
			if(Application.isPlaying) UnityEngine.Object.Destroy(child.gameObject);
			else if(Application.isEditor) UnityEngine.Object.DestroyImmediate(child.gameObject);
		}
	}

	/// <summary>Destroys all childs from parent with T component.</summary>
	/// <param name="_transform">The Transform that owns the childs.</param>
	public static void KillAllChildsWith<T>(this Transform _transform) where T : UnityEngine.Object
	{
		foreach(Transform child in _transform)
		{
			if(child.gameObject.GetComponent<T>() != null)
			{
				if(Application.isPlaying) UnityEngine.Object.Destroy(child.gameObject);
				else if(Application.isEditor) UnityEngine.Object.DestroyImmediate(child.gameObject);
			}		
		}
	}

	/// <summary>Adopts child from former Transform.</summary>
	/// <param name="_transform">The new parent of the childs.</param>
	/// <param name="_formerParent">The former parent of the childs.</param>
	/// <returns>List of new childs.</returns>
	public static List<Transform> AdoptChilds(this Transform _transform, Transform _formerParent)
	{
		List<Transform> newList = new List<Transform>();

		foreach(Transform child in _formerParent)
		{
			child.parent = _transform;
			newList.Add(child);
		}

		return newList;
	}

	/// <summary>Gets a List of child's T Components.</summary>
	/// <param name="_transform">The Transform that owns the Childs.</summary>
	/// <returns>List of T Components contained in Childs.</returns>
	public static List<T> GetComponentsFromChilds<T>(this Transform _transform) where T : UnityEngine.Object
	{
		List<T> newList = new List<T>();

		foreach(Transform child in _transform)
		{
			if(child.gameObject.Has<T>()) newList.Add(child.gameObject.GetComponent<T>());
		}

		return newList;
	}

	/// <summary>Adopts child that posses T component from former Transform.</summary>
	/// <param name="_transform">The new parent of the childs.</param>
	/// <param name="_formerParent">The former parent of the childs.</param>
	/// <returns>List of new childs with T Component.</returns>
	public static List<GameObject> AdoptChildsWith<T>(this Transform _transform, Transform _formerParent) where T : UnityEngine.Object
	{
		List<GameObject> newList = new List<GameObject>();

		foreach(Transform child in _formerParent)
		{
			if(child.gameObject.GetComponent<T>() != null)
			{
				child.parent = _transform;
				newList.Add(child.gameObject);
			}
		}

		return newList;
	}
}
}