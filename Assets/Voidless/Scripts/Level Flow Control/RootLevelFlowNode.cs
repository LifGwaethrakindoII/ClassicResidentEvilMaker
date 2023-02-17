using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.LevelFlowControl
{
[System.Serializable]
public class RootLevelFlowNode : ScriptableClass<RootLevelFlowNode>, ILevelFlowNode
{
	[SerializeField] private Blackboard<int, ILevelFlowNode> _blackboard; 	/// <summary>Root Node's Blackboard.</summary>
	[SerializeField] private int _id; 										/// <summary>Level Flow Node's ID.</summary>
	[SerializeField] private ILevelFlowNode _rootNode; 						/// <summary>This Node's root.</summary>
	[SerializeField] private ILevelFlowNode _parentNode; 					/// <summary>This Node's parent.</summary>
	[SerializeField] private List<ILevelFlowNode> _childNodes; 				/// <summary>This Node's childs.</summary>
	[SerializeField] private bool _hasIDAssigned; 							/// <summary>Does this Node have ID assigned? [not the default 0].</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets blackboard property.</summary>
	public Blackboard<int, ILevelFlowNode> blackboard
	{
		get { return _blackboard; }
		set { _blackboard = value; }
	}
	
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

	/*/// <summary>RootLevelFlowNode constructor.</summary>
	/// <param name="_id">This Node's ID.</param>
	public RootLevelFlowNode(int _id) : this()
	{
		blackboard = new Blackboard<int, ILevelFlowNode>();
		blackboard.AddEntry(ID = _id, this);
	}

	/// <summary>RootLevelFlowNode constructor.</summary>
	/// <param name="_childNodes">Root Node's childs.</param>
	public RootLevelFlowNode(params ILevelFlowNode[] _childNodes) : this()
	{
		blackboard = new Blackboard<int, ILevelFlowNode>();
		hasIDAssigned = false;
		AddNodes(_childNodes);
	}

	/// <summary>RootLevelFlowNode constructor.</summary>
	/// <param name="_id">This Node's ID.</param>
	/// <param name="_childNodes">Root Node's childs.</param>
	public RootLevelFlowNode(int _id, params ILevelFlowNode[] _childNodes) : this()
	{
		blackboard = new Blackboard<int, ILevelFlowNode>();
		blackboard.AddEntry(ID = _id, this);
		AddNodes(_childNodes);
	}*/

	/// <summary>RootLevelFlowNode ScriptableObject's constructor.</summary>
	/// <param name="_id">This Node's ID.</param>
	public static RootLevelFlowNode NewInstance(int _id)
	{
		RootLevelFlowNode newRoot = CreateInstance<RootLevelFlowNode>();

		newRoot.blackboard = new Blackboard<int, ILevelFlowNode>();
		newRoot.blackboard.AddEntry(newRoot.ID = _id, newRoot);

		return newRoot;
	}

	/// <summary>RootLevelFlowNode ScriptableObject's constructor.</summary>
	/// <param name="_childNodes">Root Node's childs.</param>
	public static RootLevelFlowNode NewInstance(params ILevelFlowNode[] _childNodes)
	{
		RootLevelFlowNode newRoot = CreateInstance<RootLevelFlowNode>();

		newRoot.blackboard = new Blackboard<int, ILevelFlowNode>();
		newRoot.hasIDAssigned = false;
		newRoot.AddNodes(_childNodes);

		return newRoot;
	}

	/// <summary>RootLevelFlowNode ScriptableObject's constructor.</summary>
	/// <param name="_id">This Node's ID.</param>
	/// <param name="_childNodes">Root Node's childs.</param>
	public static RootLevelFlowNode NewInstance(int _id, params ILevelFlowNode[] _childNodes)
	{
		RootLevelFlowNode newRoot = CreateInstance<RootLevelFlowNode>();

		newRoot.blackboard = new Blackboard<int, ILevelFlowNode>();
		newRoot.blackboard.AddEntry(newRoot.ID = _id, newRoot);
		newRoot.AddNodes(_childNodes);

		return newRoot;
	}

#region Methods:
	/// <summary>Adds Node to collection of child nodes.</summary>
	/// <param name="_childNode">Node that will be added to the collection of child nodes.</param>
	public void AddNode(ILevelFlowNode _childNode)
	{
		_childNode.rootNode = this;
		_childNode.parentNode = this;
		if(_childNode.hasIDAssigned)
		{ /// If there was an ID assigned by the user, add this Node to the Root's blackboard.
			blackboard.AddEntry(_childNode.ID, _childNode as ILevelFlowNode);
		}
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
		if(blackboard.hasPriority)
		{
			ILevelFlowNode childNode = blackboard.GetEntry(blackboard.FetchPriorityKey());
			if(childNode != null) return childNode.Tick();
		}

		for(int i = 0; i < childNodes.Count; i++)
		{
			if(childNodes[i].Tick()) return true;
			else continue;	
		}

		return false;
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