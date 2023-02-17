using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.LevelFlowControl
{
public struct InverterLevelFlowNode : ILevelFlowNode
{
	private int _id; 										/// <summary>Level Flow Node's ID.</summary>
	private ILevelFlowNode _rootNode; 						/// <summary>This Node's root.</summary>
	private ILevelFlowNode _parentNode; 					/// <summary>This Node's parent.</summary>
	private List<ILevelFlowNode> _childNodes; 				/// <summary>This Node's childs.</summary>
	private bool _hasIDAssigned; 							/// <summary>Does this Node have ID assigned? [not the default 0].</summary>
	private Predicate<bool> _inverterPolicy; 				/// <summary>Inverter's Policy.</summary>

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

	/// <summary>Gets and Sets inverterPolicy property.</summary>
	public Predicate<bool> inverterPolicy
	{
		get { return _inverterPolicy; }
		set { _inverterPolicy = value; }
	}
#endregion

	/// <summary>InverterLevelFlowNode constructor.</summary>
	/// <param name="_conditionPredicate">Condition Node's predicate.</param>
	public InverterLevelFlowNode(Predicate<bool> _inverterPolicy, ILevelFlowNode _childNode) : this()
	{
		hasIDAssigned = false;
		inverterPolicy = _inverterPolicy;
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
		Debug.LogError("[InverterLevelFlowNode] Cannot have Child Nodes from Decorator Node");
	}

	/// <summary>Ticks Node.</summary>
	/// <returns>Node's bool result.</returns>
	public bool Tick()
	{
		if(childNodes[0] != null)
		{
			return inverterPolicy(childNodes[0].Tick());
		}
		else
		{
			return false;
		}
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