using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class BehaviorTree<T> where T : IComponentAgent<T, IEnumerator<TreeState>>
{
	private static BehaviorTree<T> _Instance; 	/// <summary>Singleton Instance's reference.</summary>

	private SelectorComposite<T> _root; 		/// <summary>Behavior Tree's Root.</summary>

	/// <summary>Gets and Sets Instance property.</summary>
	public static BehaviorTree<T> Instance
	{
		get { return _Instance; }
		set { _Instance = value; }
	}

	/// <summary>Gets and Sets root property.</summary>
	public SelectorComposite<T> root
	{
		get { return _root; }
		private set { _root = value; }
	}

	/// <summary>BehaviorTree default constructor.</summary>
	public BehaviorTree(params BehaviorTreeComponent<T>[] _treeNodes)
	{
		root = new SelectorComposite<T>(_treeNodes);
	}

	/// <summary>Updates Behavior Tree.</summary>
	/// <param name="_agent">Agent requesting an update.</param>
	public void Update(T _agent)
	{
		#pragma warning disable 0642
		if(_agent.currentComponentIterator != null && _agent.currentComponentIterator.MoveNext());
		else _agent.currentComponentIterator = root.Tick(_agent);
	}

	public void Reset(T _agent)
	{
		_agent.currentComponentIterator = root.Tick(_agent);
	}

	public string ToString(T _agent)
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("Current Tree Component's State: ");
		builder.Append(_agent.currentComponentIterator.Current.ToString());

		return builder.ToString();
	}
}
}