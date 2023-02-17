using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.LevelFlowControl
{
public struct ActionLevelFlowNode : ILevelFlowNode
{
	private int _id; 										/// <summary>Level Flow Node's ID.</summary>
	private ILevelFlowNode _rootNode; 						/// <summary>This Node's root.</summary>
	private ILevelFlowNode _parentNode; 					/// <summary>This Node's parent.</summary>
	private List<ILevelFlowNode> _childNodes; 				/// <summary>This Node's childs.</summary>
	private bool _hasIDAssigned; 							/// <summary>Does this Node have ID assigned? [not the default 0].</summary>
	private Action _action; 								/// <summary>Action's Node action, executed by each Tick.</summary>

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

	/// <summary>Gets and Sets action property.</summary>
	public Action action
	{
		get { return _action; }
		set { _action = value; }
	}
#endregion

	/// <summary>ActionLevelFlowNode constructor.</summary>
	/// <param name="_conditionPredicate">Condition Node's predicate.</param>
	public ActionLevelFlowNode(Action _action) : this()
	{
		hasIDAssigned = false;
		action = _action;
	}

#region Methods:
	/// <summary>Adds Node to collection of child nodes.</summary>
	/// <param name="_childNode">Node that will be added to the collection of child nodes.</param>
	public void AddNode(ILevelFlowNode _childNode)
	{
		Debug.LogError("[ActionLevelFlowNode] Cannot have Child Nodes from Leaf Node");
	}

	/// <summary>Adds nodes to collection of child nodes.</summary>
	/// <param name="_childNodes">Nodes that will be added to the collecion of child nodes.</param>
	public void AddNodes(params ILevelFlowNode[] _childNodes)
	{
		Debug.LogError("[ActionLevelFlowNode] Cannot have Child Nodes from Leaf Node");
	}

	/// <summary>Ticks Node.</summary>
	/// <returns>Node's bool result.</returns>
	public bool Tick()
	{
		action();
		return true;
	}

	/// <summary>Ticks Node with delta time reference.</summary>
	/// <param name="_deltaTime">Delta of time since the last time this node was ticked.</param>
	/// <returns>Node's bool result.</returns>
	public bool Tick(float _deltaTime)
	{
		action();
		return true;
	}
#endregion
}
}