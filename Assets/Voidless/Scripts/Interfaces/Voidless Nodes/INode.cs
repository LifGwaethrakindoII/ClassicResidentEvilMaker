using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface INode<N, R> where N : INode <N, R>
{
	/// <summary>Gets and Sets ID property.</summary>
	int ID { get; set; }

	/// <summary>Gets and Sets rootNode property.</summary>
	N rootNode { get; set; }

	/// <summary>Gets and Sets parentNode property.</summary>
	N parentNode { get; set; }

	/// <summary>Gets and Sets childNodes property.</summary>
	List<N> childNodes { get; set; }

	/// <summary>Adds Node to collection of child nodes.</summary>
	/// <param name="_childNode">Node that will be added to the collection of child nodes.</param>
	void AddNode(N _childNode);

	/// <summary>Adds nodes to collection of child nodes.</summary>
	/// <param name="_childNodes">Nodes that will be added to the collecion of child nodes.</param>
	void AddNodes(params N[] _childNodes);

	/// <summary>Ticks Node.</summary>
	/// <returns>Node's R result.</returns>
	R Tick();

	/// <summary>Ticks Node with delta time reference.</summary>
	/// <param name="_deltaTime">Delta of time since the last time this node was ticked.</param>
	/// <returns>Node's R result.</returns>
	R Tick(float _deltaTime);
}
}