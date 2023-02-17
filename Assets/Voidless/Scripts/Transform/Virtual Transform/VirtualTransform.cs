using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[ExecuteInEditMode]
public class VirtualTransform : MonoBehaviour
{
	[Header("Transform's Properties:")]
	[SerializeField] private Vector3 _localPosition; 								/// <summary>Virtual Local Position.</summary>
	[SerializeField] private Vector3 _localEulerRotation; 							/// <summary>Virtual Local Euler Rotation.</summary>
	[SerializeField] private Vector3 _localScale; 									/// <summary>Virtual Local Scale.</summary>
	[Space(5f)]
	[Header("Constraints:")]
	[SerializeField] private TransformProperties _inheritProperties; 				/// <summary>Transform's properties to inherit from virtual parent.</summary>
	[SerializeField] private Axes3D _positionConstraints;							/// <summary>Position's components to to ignore.</summary>
	[SerializeField] private Axes3D _rotationConstraints; 							/// <summary>Rotation's components to ignore.</summary>
	[SerializeField] private Axes3D _scaleConstraints; 								/// <summary>Scale's components to ignore.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Editor Test:")]
	[SerializeField] private VirtualTransform[] _editorVirtualParents; 				/// <summary>Editor's Virtual Parents.</summary>
#endif
	private List<VirtualTransform> _virtualParents; 								/// <summary>Virtual Parents.</summary>
	private List<VirtualTransform> _virtualChildren; 								/// <summary>Virtual Children.</summary>
	private Quaternion _localRotation; 												/// <summary>Virtual Local Rotation.</summary>

#region Getter/Setters:
	/// <summary>Gets and Sets localPosition property.</summary>
	public Vector3 localPosition
	{
		get { return _localPosition; }
		set { _localPosition = value; }
	}

	/// <summary>Gets and Sets localEulerRotation property.</summary>
	public Vector3 localEulerRotation
	{
		get { return _localEulerRotation; }
		set { _localEulerRotation = value; }
	}

	/// <summary>Gets and Sets localScale property.</summary>
	public Vector3 localScale
	{
		get { return _localScale; }
		set { _localScale = value; }
	}

	/// <summary>Gets and Sets localRotation property.</summary>
	public Quaternion localRotation
	{
		get { return _localRotation; }
		set { _localRotation = value; }
	}

	/// <summary>Gets and Sets inheritProperties property.</summary>
	public TransformProperties inheritProperties
	{
		get { return _inheritProperties; }
		set { _inheritProperties = value; }
	}

	/// <summary>Gets and Sets positionConstraints property.</summary>
	public Axes3D positionConstraints
	{
		get { return _positionConstraints; }
		set { _positionConstraints = value; }
	}

	/// <summary>Gets and Sets rotationConstraints property.</summary>
	public Axes3D rotationConstraints
	{
		get { return _rotationConstraints; }
		set { _rotationConstraints = value; }
	}

	/// <summary>Gets and Sets scaleConstraints property.</summary>
	public Axes3D scaleConstraints
	{
		get { return _scaleConstraints; }
		set { _scaleConstraints = value; }
	}

	/// <summary>Gets and Sets virtualParents property.</summary>
	public List<VirtualTransform> virtualParents
	{
		get { return _virtualParents; }
		set { _virtualParents = value; }
	}

	/// <summary>Gets and Sets virtualChildren property.</summary>
	public List<VirtualTransform> virtualChildren
	{
		get { return _virtualChildren; }
		set { _virtualChildren = value; }
	}
#endregion

#if UNITY_EDITOR
	/// <summary>Gets and Sets editorVirtualParents property.</summary>
	public VirtualTransform[] editorVirtualParents
	{
		get { return _editorVirtualParents; }
		private set { _editorVirtualParents = value; }
	}
#endif

#region UnityMethods:
	/// <summary>VirtualTransform's instance initialization.</summary>
	void Awake()
	{
		if(virtualChildren == null) virtualChildren = new List<VirtualTransform>();
		if(virtualParents == null) virtualParents = new List<VirtualTransform>();

#if UNITY_EDITOR
		if(Application.isPlaying) RegisterParents();
#endif
	}
	
	/// <summary>VirtualTransform's tick at each frame.</summary>
	void Update()
	{
		InheritParents();
	}
#endregion

#if UNITY_EDITOR
	public void RegisterParents()
	{
		if(editorVirtualParents != null) AddParents(editorVirtualParents);
		if(Application.isPlaying) editorVirtualParents = null;
	}
#endif

	public void AddParent(VirtualTransform _parent)
	{
		virtualParents.Add(_parent);
		_parent.virtualChildren.Add(this);

		UpdateLocalProperties();
	}

	public void AddParents(params VirtualTransform[] _parents)
	{
		if(_parents != null)
		{
			foreach(VirtualTransform virtualParent in _parents)
			{
				virtualParents.Add(virtualParent);
				virtualParent.virtualChildren.Add(this);	
			}

			UpdateLocalProperties();
		}
	}

	public void AddChild(VirtualTransform _child)
	{
		virtualChildren.Add(_child);
		_child.virtualParents.Add(this);
	}

	public void AddChildren(params VirtualTransform[] _children)
	{
		foreach(VirtualTransform virtualChild in _children)
		{
			virtualChildren.Add(virtualChild);
			virtualChild.virtualParents.Add(this);	
		}
	}

	public void RemoveParent(VirtualTransform _parent)
	{
		virtualParents.Remove(_parent);
		_parent.virtualChildren.Remove(this);

		UpdateLocalProperties();
	}

	private void UpdateLocalProperties()
	{
		if(virtualParents.Count > 0)
		{
			Vector3 accumulatedParentPositions = Vector3.zero;
			Vector3 accumulatedParentEulerRotations = Vector3.zero;
			Vector3 accumulatedParentScales = Vector3.zero;
			float countInverse = (1.0f / (float)virtualParents.Count);

			foreach(VirtualTransform parent in virtualParents)
			{
				accumulatedParentPositions += parent.localPosition;
				accumulatedParentEulerRotations += parent.localEulerRotation;
				accumulatedParentScales += parent.localScale;

				/*
				if(parent.inheritProperties.HasFlag(TransformProperties.Position))
				{
					accumulatedParentPositions += new Vector3(
						!parent.positionConstraints.HasFlag(Axes3D.X) ? parent.localPosition.x : 0.0f,
						!parent.positionConstraints.HasFlag(Axes3D.Z) ? parent.localPosition.y : 0.0f,
						!parent.positionConstraints.HasFlag(Axes3D.Y) ? parent.localPosition.z : 0.0f
					);
				}
				if(parent.inheritProperties.HasFlag(TransformProperties.Rotation))
				{
					accumulatedParentEulerRotations += new Vector3(
						!parent.rotationConstraints.HasFlag(Axes3D.X) ? parent.localEulerRotation.x : 0.0f,
						!parent.rotationConstraints.HasFlag(Axes3D.Y) ? parent.localEulerRotation.y : 0.0f,
						!parent.rotationConstraints.HasFlag(Axes3D.Z) ? parent.localEulerRotation.z : 0.0f
					);
				}
				if(parent.inheritProperties.HasFlag(TransformProperties.Scale))
				{
					accumulatedParentScales += new Vector3(
						!parent.rotationConstraints.HasFlag(Axes3D.X) ? parent.localScale.x : 1.0f,
						!parent.rotationConstraints.HasFlag(Axes3D.Y) ? parent.localScale.z : 1.0f,
						!parent.rotationConstraints.HasFlag(Axes3D.Z) ? parent.localScale.z : 1.0f
					);
				}
				*/
			}

			localPosition = transform.position - (accumulatedParentPositions * countInverse);
			localEulerRotation = transform.eulerAngles - (accumulatedParentEulerRotations * countInverse);
			localScale = Vector3.Scale(transform.localScale, (accumulatedParentScales * countInverse));
		}
	}

	private void UpdateChildren()
	{
		foreach(VirtualTransform child in virtualChildren)
		{
			//child.transform.position = 
		}
	}

	private void InheritParents()
	{
		if(virtualParents.Count > 0)
		{
			Vector3 accumulatedParentPositions = Vector3.zero;
			Vector3 accumulatedParentEulerRotations = Vector3.zero;
			Vector3 accumulatedParentScales = Vector3.zero;
			Quaternion accumulatedRotations = Quaternion.identity;
			float countInverse = (1.0f / (float)virtualParents.Count);

			foreach(VirtualTransform parent in virtualParents)
			{
				if(parent.inheritProperties.HasFlag(TransformProperties.Position))
				{
					accumulatedRotations *= parent.localRotation;
					accumulatedParentPositions += new Vector3(
						!parent.positionConstraints.HasFlag(Axes3D.X) ? parent.localPosition.x : 0.0f,
						!parent.positionConstraints.HasFlag(Axes3D.Z) ? parent.localPosition.y : 0.0f,
						!parent.positionConstraints.HasFlag(Axes3D.Y) ? parent.localPosition.z : 0.0f
					);
				}
				if(parent.inheritProperties.HasFlag(TransformProperties.Rotation))
				{
					accumulatedParentEulerRotations += new Vector3(
						!parent.rotationConstraints.HasFlag(Axes3D.X) ? parent.localEulerRotation.x : 0.0f,
						!parent.rotationConstraints.HasFlag(Axes3D.Y) ? parent.localEulerRotation.y : 0.0f,
						!parent.rotationConstraints.HasFlag(Axes3D.Z) ? parent.localEulerRotation.z : 0.0f
					);
				}
				if(parent.inheritProperties.HasFlag(TransformProperties.Scale))
				{
					accumulatedParentScales += new Vector3(
						!parent.rotationConstraints.HasFlag(Axes3D.X) ? parent.localScale.x : 0.0f,
						!parent.rotationConstraints.HasFlag(Axes3D.Y) ? parent.localScale.z : 0.0f,
						!parent.rotationConstraints.HasFlag(Axes3D.Z) ? parent.localScale.z : 0.0f
					);
				}
			}

			if(inheritProperties.HasFlag(TransformProperties.Position)) transform.position = (accumulatedParentPositions * countInverse) + (accumulatedRotations * localPosition);
			if(inheritProperties.HasFlag(TransformProperties.Rotation)) transform.eulerAngles = (accumulatedParentEulerRotations * countInverse) + localEulerRotation;
			if(inheritProperties.HasFlag(TransformProperties.Scale)) transform.localScale = Vector3.Scale(localScale, (accumulatedParentScales * countInverse));
		}
		else
		{
			localPosition = transform.position;
			localRotation = transform.rotation;
			localEulerRotation = transform.eulerAngles;
			localScale = transform.localScale;
		}
	}
}
}