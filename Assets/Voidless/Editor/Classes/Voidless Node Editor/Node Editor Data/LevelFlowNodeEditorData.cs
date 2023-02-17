using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Voidless.LevelFlowControl;


namespace Voidless.EditorNodes
{
[Serializable]
public class LevelFlowNodeEditorData : BaseNodeEditorData<BaseLevelFlowNode, LevelFlowNodeEditorAttributes>
{
	[HideInInspector] private LevelFlowRoot _root;
	[HideInInspector] private List<LevelFlowRoot> _roots;
	[HideInInspector] private List<LevelFlowSelector> _selectors;
	[HideInInspector] private List<LevelFlowSequence> _sequences;
	[HideInInspector] private List<LevelFlowCondition> _conditions;
	[HideInInspector] private List<LevelFlowAction> _actions;
	[HideInInspector] private List<LevelFlowRepeatNTimes> _repeatNTimes;

#region Properties:
	
#endregion

#region Getters:
	/// <summary>Gets and Sets root property.</summary>
	public LevelFlowRoot root
	{
		get { return _root; }
		set { _root = value; }
	}

	/// <summary>Gets and Sets roots property.</summary>
	public List<LevelFlowRoot> roots
	{
		get { return _roots; }
		set { _roots = value; }
	}

	/// <summary>Gets and Sets selectors property.</summary>
	public List<LevelFlowSelector> selectors
	{
		get { return _selectors; }
		set { _selectors = value; }
	}

	/// <summary>Gets and Sets sequences property.</summary>
	public List<LevelFlowSequence> sequences
	{
		get { return _sequences; }
		set { _sequences = value; }
	}

	/// <summary>Gets and Sets conditions property.</summary>
	public List<LevelFlowCondition> conditions
	{
		get { return _conditions; }
		set { _conditions = value; }
	}

	/// <summary>Gets and Sets actions property.</summary>
	public List<LevelFlowAction> actions
	{
		get { return _actions; }
		set { _actions = value; }
	}

	/// <summary>Gets and Sets repeatNTimes property.</summary>
	public List<LevelFlowRepeatNTimes> repeatNTimes
	{
		get { return _repeatNTimes; }
		set { _repeatNTimes = value; }
	}
#endregion

	/// <summary>Gets converted Nodes from the current Node Editor Window's Data.</summary>
	/// <returns>Interpreted Nodes.</returns>
	public override List<BaseLevelFlowNode> GetNodes()
	{
		nodes = new List<BaseLevelFlowNode>();

		if(roots != null)
		{
			for(int i = 0; i < roots.Count; i++)
			{
				nodes.Add(roots[i]);
			}
		}
		if(selectors != null)
		{
			for(int i = 0; i < selectors.Count; i++)
			{
				nodes.Add(selectors[i]);
			}
		}
		if(sequences != null)
		{
			for(int i = 0; i < sequences.Count; i++)
			{
				nodes.Add(sequences[i]);
			}
		}
		if(conditions != null)
		{
			for(int i = 0; i < conditions.Count; i++)
			{
				nodes.Add(conditions[i]);
			}
		}
		if(actions != null)
		{
			for(int i = 0; i < actions.Count; i++)
			{
				nodes.Add(actions[i]);
			}
		}
		if(repeatNTimes != null)
		{
			for(int i = 0; i < repeatNTimes.Count; i++)
			{
				nodes.Add(repeatNTimes[i]);
			}
		}

		return nodes;
	}

	/// <summary>Resets temporal memory of the current Node Editor Window's Data.</summary>
	public override void ResetTemporalMemory()
	{
		roots = new List<LevelFlowRoot>();
		selectors = new List<LevelFlowSelector>();
		sequences = new List<LevelFlowSequence>();
		conditions = new List<LevelFlowCondition>();
		actions = new List<LevelFlowAction>();
		repeatNTimes = new List<LevelFlowRepeatNTimes>();
	}

	/// <summary>Saves specific Node, by evaluating its inheritance.</summary>
	/// <param name="_node">Node that inherits from N.</param>
	public override void SaveNode<N>(N _node)
	{
		if(_node is LevelFlowRoot)
		{
			roots.Add(_node as LevelFlowRoot);
		}
		else if(_node is LevelFlowSelector)
		{
			selectors.Add(_node as LevelFlowSelector);
		}
		else if(_node is LevelFlowSequence)
		{
			sequences.Add(_node as LevelFlowSequence);
		}
		else if(_node is LevelFlowCondition)
		{
			conditions.Add(_node as LevelFlowCondition);
		}
		else if(_node is LevelFlowAction)
		{
			actions.Add(_node as LevelFlowAction);
		}
		else if(_node is LevelFlowRepeatNTimes)
		{
			repeatNTimes.Add(_node as LevelFlowRepeatNTimes);
		}
	}

	public RootLevelFlowNode CreateLevelFlowTree()
	{
		return (RootLevelFlowNode)root.ConvertToLevelFlowNodeOfType();
	}
}
}