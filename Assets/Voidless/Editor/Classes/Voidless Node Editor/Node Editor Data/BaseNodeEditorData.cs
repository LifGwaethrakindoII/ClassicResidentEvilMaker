using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voidless.EditorNodes
{
[System.Serializable]
public abstract class BaseNodeEditorData<N, A> : ScriptableObject where N : Node where A : BaseNodeEditorAttributes
{
	[SerializeField] private List<MonoBehaviour> _monoBehaviour; 		/// <summary>Editor Window's reference MonoBehaviour.</summary>
	[SerializeField] private A _attributes; 							/// <summary>Base Node Editor's Attributes data.</summary>
	[SerializeField] private List<N> _nodes; 							/// <summary>Edityor Window's Nodes stored.</summary>
	[SerializeField] private List<Connection> _connections; 			/// <summary>Editor Window's Nodes Connections stored.</summary>
	[SerializeField] private List<ConnectionPoint> _connectionPoints; 	/// <summary>Editor Window's Nodes Connection Points.</summary>

	/// <summary>Gets and Sets monoBehaviour property.</summary>
	public List<MonoBehaviour> monoBehaviour
	{
		get { return _monoBehaviour; }
		set { _monoBehaviour = value; }
	}

	/// <summary>Gets and Sets attributes property.</summary>
	public A attributes
	{
		get { return _attributes; }
		set { _attributes = value; }
	}

	/// <summary>Gets and Sets nodes property.</summary>
	public List<N> nodes
	{
		get { return _nodes; }
		set { _nodes = value; }
	}

	/// <summary>Gets and Sets connections property.</summary>
	public List<Connection> connections
	{
		get { return _connections; }
		set { _connections = value; }
	}

	/// <summary>Gets and Sets connectionPoints property.</summary>
	public List<ConnectionPoint> connectionPoints
	{
		get { return _connectionPoints; }
		set { _connectionPoints = value; }
	}

	void OnEnable()
	{
		//hideFlags = HideFlags.HideAndDontSave;
		hideFlags = HideFlags.None;
	}

	/// <summary>Deletes all data stored, and sets it to its default value [Except for the Attributes reference].</summary>
	public void DeleteData()
	{
		ResetTemporalMemory();
		monoBehaviour = null;
		nodes = null;
		connections = null;
		connectionPoints = null;
	}

	/// <summary>Gets converted Nodes from the current Node Editor Window's Data.</summary>
	/// <returns>Interpreted Nodes.</returns>
	public abstract List<N> GetNodes();

	/// <summary>Resets temporal memory of the current Node Editor Window's Data.</summary>
	public abstract void ResetTemporalMemory();

	/// <summary>Saves specific Node, by evaluating its inheritance.</summary>
	/// <param name="_node">Node that inherits from N.</param>
	public virtual void SaveNode<T>(T _node) where T : N { /*...*/ }

	/*[MenuItem(PATH_URL)]
	public static void CreateAsset()
	{
		ScriptableObject scriptableObject = ScriptableObject.CreateInstance<ScriptableObject>() as ScriptableObject;
		AssetDatabase.CreateAsset(scriptableObject, AssetDatabase.CreateUniqueAssetPath(NEW_ASSET_PATH));
	}*/
}
}