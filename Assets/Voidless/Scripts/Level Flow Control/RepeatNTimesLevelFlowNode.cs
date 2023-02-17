using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.LevelFlowControl
{
public struct RepeatNTimesLevelFlowNode : ILevelFlowNode
{
	private int _id; 										/// <summary>Level Flow Node's ID.</summary>
	private ILevelFlowNode _rootNode; 						/// <summary>This Node's root.</summary>
	private ILevelFlowNode _parentNode; 					/// <summary>This Node's parent.</summary>
	private List<ILevelFlowNode> _childNodes; 				/// <summary>This Node's childs.</summary>
	private bool _hasIDAssigned; 							/// <summary>Does this Node have ID assigned? [not the default 0].</summary>
	private int _numberOfTimes; 							/// <summary>Number of times this decorator will repeat its Leaf child.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets ID property.</summary>
	public int ID
	{
		get { return _id; }
		set { _id = value; }
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

	/// <summary>Gets and Sets numberOfTimes property.</summary>
	public int numberOfTimes
	{
		get { return _numberOfTimes; }
		set { _numberOfTimes = value; }
	}
#endregion

	/// <summary>RepeatNTimesLevelFlowNode constructor.</summary>
	/// <param name="_conditionPredicate">Condition Node's predicate.</param>
	public RepeatNTimesLevelFlowNode(int _numberOfTimes, ILevelFlowNode _childNode) : this()
	{
		hasIDAssigned = false;
		numberOfTimes = _numberOfTimes;
		AddNode(_childNode);
	}

#region Methods:
	/// <summary>Adds Node to collection of child nodes.</summary>
	/// <param name="_childNode">Node that will be added to the collection of child nodes.</param>
	public void AddNode(ILevelFlowNode _childNode)
	{
		_childNode.rootNode = rootNode;
		_childNode.parentNode = parentNode;
		childNodes.Add(_childNode);
	}

	/// <summary>Adds nodes to collection of child nodes.</summary>
	/// <param name="_childNodes">Nodes that will be added to the collecion of child nodes.</param>
	public void AddNodes(params ILevelFlowNode[] _childNodes)
	{
		Debug.LogError("[RepeatNTimesLevelFlowNode] Cannot have Child Nodes from Decorator Node");
	}

	/// <summary>Ticks Node.</summary>
	/// <returns>Node's bool result.</returns>
	public bool Tick()
	{
		if(--numberOfTimes > 0)
		{
			return childNodes[0] != null ? childNodes[0].Tick() : parentNode is SequenceLevelFlowNode ? true : false;
		}

		return parentNode is SequenceLevelFlowNode ? true : false;
	}

	/// <summary>Ticks Node with delta time reference.</summary>
	/// <param name="_deltaTime">Delta of time since the last time this node was ticked.</param>
	/// <returns>Node's bool result.</returns>
	public bool Tick(float _deltaTime)
	{
		return true;
	}
#endregion
}
}