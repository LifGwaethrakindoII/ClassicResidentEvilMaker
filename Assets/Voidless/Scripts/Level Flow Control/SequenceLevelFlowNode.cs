using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.LevelFlowControl
{
public struct SequenceLevelFlowNode : ILevelFlowNode
{
	private int _id; 										/// <summary>Level Flow Node's ID.</summary>
	private ILevelFlowNode _rootNode; 						/// <summary>This Node's root.</summary>
	private ILevelFlowNode _parentNode; 					/// <summary>This Node's parent.</summary>
	private List<ILevelFlowNode> _childNodes; 				/// <summary>This Node's childs.</summary>
	private bool _hasIDAssigned; 							/// <summary>Does this Node have ID assigned? [not the default 0].</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets ID property.</summary>
	public int ID
	{
		get { return _id; }
		set
		{ 
			_id = value;
			hasIDAssigned = true;
		}
	}

	/// <summary>Gets and Sets rootNode property.</summary>
	public ILevelFlowNode rootNode
	{
		get { return _rootNode; }
		set { _rootNode = value; }
	}

	/// <summary>Gets and Sets parentNode property.</summary>
	public ILevelFlowNode parentNode
	{
		get { return _parentNode; }
		set { _parentNode = value; }
	}

	/// <summary>Gets and Sets childNodes property.</summary>
	public List<ILevelFlowNode> childNodes
	{
		get { return _childNodes; }
		set { _childNodes = value; }
	}

	/// <summary>Gets and Sets hasIDAssigned property.</summary>
	public bool hasIDAssigned
	{
		get { return _hasIDAssigned; }
		set { _hasIDAssigned = value; }
	}
#endregion

	/// <summary>SequenceLevelFlowNode constructor.</summary>
	/// <param name="_id">This Node's ID.</param>
	public SequenceLevelFlowNode(int _id) : this()
	{
		ID = _id;
	}

	/// <summary>SequenceLevelFlowNode constructor.</summary>
	/// <param name="_childNodes">Root Node's childs.</param>
	public SequenceLevelFlowNode(params ILevelFlowNode[] _childNodes) : this()
	{
		hasIDAssigned = false;
		AddNodes(_childNodes);
	}

	/// <summary>SequenceLevelFlowNode constructor.</summary>
	/// <param name="_id">This Node's ID.</param>
	/// <param name="_childNodes">Root Node's childs.</param>
	public SequenceLevelFlowNode(int _id, params ILevelFlowNode[] _childNodes) : this()
	{
		ID = _id;
		AddNodes(_childNodes);
	}

#region Methods:
	/// <summary>Adds Node to collection of child nodes.</summary>
	/// <param name="_childNode">Node that will be added to the collection of child nodes.</param>
	public void AddNode(ILevelFlowNode _childNode)
	{
		_childNode.rootNode = rootNode;
		_childNode.parentNode = this;
		childNodes.Add(_childNode);
	}

	/// <summary>Adds nodes to collection of child nodes.</summary>
	/// <param name="_childNodes">Nodes that will be added to the collecion of child nodes.</param>
	public void AddNodes(params ILevelFlowNode[] _childNodes)
	{
		for(int i = 0; i < childNodes.Count; i++)
		{
			AddNode(childNodes[i]);	
		}
	}

	/// <summary>Ticks Node.</summary>
	/// <returns>Node's bool result.</returns>
	public bool Tick()
	{
		for(int i = 0; i < childNodes.Count; i++)
		{
			if(!childNodes[i].Tick()) return false;
			else continue;	
		}

		return true;
	}

	/// <summary>Ticks Node with delta time reference.</summary>
	/// <param name="_deltaTime">Delta of time since the last time this node was ticked.</param>
	/// <returns>Node's bool result.</returns>
	public bool Tick(float _deltaTime)
	{
		return false;
	}
#endregion
}
}