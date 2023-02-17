using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*	*
	* Where R is the restult type.
	* Where D is the Data this Node holds.
	*	*/

namespace Voidless
{
public abstract class BaseNode<R, D>
{
	private D _data; 							/// <summary>Node's Data.</summary>
	private List<BaseNode<R, D>> _connections; 	/// <summary>Connections this Node may have.</summary>

	/// <summary>Gets and Sets data property.</summary>
	public D data
	{
		get { return _data; }
		set { _data = value; }
	}

	/// <summary>Gets and Sets connections property.</summary>
	public List<BaseNode<R, D>> connections
	{
		get { return _connections; }
		protected set { _connections = value; }
	}

	/// <summary>Default BaseNode's constructor.</summary>
	public BaseNode()
	{
		connections = new List<BaseNode<R, D>>();
	}

	/// <summary>Overload BaseNode's constructor. The compiler will use this constructor if you are going to add just one connection.</summary>
	/// <param name="_data">Node's Data.</param>
	/// <param name="_connection">Node's direct connection.</param>
	public BaseNode(D _data, BaseNode<R, D> _connection)
	{
		data = _data;
		connections = new List<BaseNode<R, D>>();
		connections.Add(_connection);
	}

	/// <summary>Default BaseNode's constructor. The compiler will use this constructor if you are going to add more than one connection.</summary>
	/// <param name="_data">Node's Data.</param>
	/// <param name="_connections">Node's direct connections.</param>
	public BaseNode(D _data, params BaseNode<R, D>[] _connections)
	{
		data = _data;
		connections = new List<BaseNode<R, D>>();
		connections.AddRange(_connections);
	}

	/// <summary>Adds new connection to this Node.</summary>
	/// <param name="_newNode">New Node that is going to be added to this Node.</param>
	public virtual void AddNode(BaseNode<R, D> _newNode)
	{
		if(connections == null) connections = new List<BaseNode<R, D>>();
		connections.Add(_newNode);
	}

	/// <summary>Adds set of new connection to this Node.</summary>
	/// <param name="_newConnections">New Nodes that are going to be added to this Node.</param>
	public virtual void AddNodes(params BaseNode<R, D>[] _newConnections)
	{
		if(connections == null) connections = new List<BaseNode<R, D>>();
		connections.AddRange(_newConnections);
	}

	/// <summary>Ticks this Node.</summary>
	/// <returns>State of the Ticking.</returns>
	public abstract R Tick();
}
}