using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct GameObjectTag
{
	[SerializeField] public string _tag; 	/// <summary>GameObject's Tag.</summary>
#if UNITY_EDITOR
	public int index; 						/// <summary>Index of the current Tag on the Project's Tags.</summary>
#endif

	/// <summary>Gets and Sets tag property.</summary>
	public string tag
	{
		get { return _tag; }
		set
		{
#if UNITY_EDITOR
			string[] tags = UnityEditorInternal.InternalEditorUtility.tags;

			foreach(string gameObjectTag in tags)
			{
				if(gameObjectTag == value)
				{
					_tag = value;
					return;
				}
			}

			return;
#endif
			_tag = value;
		}
	}

	/// <summary>Implicit string to GameObjectTag converter.</summary>
	public static implicit operator GameObjectTag(string _tag) { return new GameObjectTag(_tag); }

	/// <summary>Implicit GameObjectTag to string converter.</summary>
	public static implicit operator string(GameObjectTag _objTag) { return _objTag.tag; }

	/// <summary>GameObjectTag's Constructor.</summary>
	/// <param name="_tag">GameObject's Tag.</param>
	public GameObjectTag(string _tag) : this()
	{
		tag = _tag;
	}

	/// <summary>Compares object to this GameObjectTag.</summary>
	/// <param name="obj">Object to evaluate.</param>
	public override bool Equals(object obj)
	{
		try
		{
			GameObjectTag objTag = (GameObjectTag)obj;

			return tag == objTag;
		}
		catch(Exception e)
		{
			Debug.LogError("[GameObjectTag] Exception Caught: " + e.Message);
			return false;
		}
	}

	/// <returns>A hash code for the current object.</returns>
	public override int GetHashCode()
	{
		return tag.GetHashCode();
	}

	/// <returns>GameObject's Tag.</returns>
	public override string ToString() { return tag; }

	/// <summary>Performs callback if GameObject's tag is on GameObjectTags.</summary>
	/// <param name="obj">GameObject to compare.</param>
	/// <param name="tags">Tags to compare with GameObject's tag.</param>
	/// <param name="onTagEqual">Callback invoked if any on the tags on the array matches.</param>
	public static void DoIfGameObjectTagMatches(GameObject obj, GameObjectTag[] tags, Action onTagEqual)
	{
		if(obj == null || tags == null) return;

		foreach(GameObjectTag tag in tags)
		{
			if(obj.CompareTag(tag))
			{
				if(onTagEqual != null) onTagEqual();
				return;
			}
		}
	}
}
}