using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct VirtualTransformNode
{
	public VirtualTransform parent; 				/// <summary>Virtual Parent.</summary>
	public TransformProperties inheritProperties; 	/// <summary>Properties to inherit from this VirtualTransform.</summary>
	public Axes3D positionConstraints;				/// <summary>Position's components to to ignore.</summary>
	public Axes3D rotationConstraints; 				/// <summary>Rotation's components to ignore.</summary>
	public Axes3D scaleConstraints; 				/// <summary>Scale's components to ignore.</summary>

	/// <summary>VirtualTransformNode's constructor.</summary>
	/// <param name="_parent">Virtual Parent.</param>
	/// <param name="_inheritProperties">Properties to inherit from virtual parent.</param>
	/// <param name="_positionConstraints">Position's components to ignore.</param>
	/// <param name="_rotationConstraints">Rotation's components to ignore.</param>
	/// <param name="_scaleConstraints">Scale's components to ignore.</param>
	public VirtualTransformNode(VirtualTransform _parent, TransformProperties _inheritProperties, Axes3D _positionConstraints, Axes3D _rotationConstraints, Axes3D _scaleConstraints)
	{
		parent = _parent;
		inheritProperties = _inheritProperties;
		positionConstraints = _positionConstraints;
		rotationConstraints = _rotationConstraints;
		scaleConstraints = _scaleConstraints;
	}
}
}