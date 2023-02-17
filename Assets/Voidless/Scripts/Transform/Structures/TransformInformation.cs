using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public class TransformInformation : IEnumerable<TransformInformation>
{
	[SerializeField] private TransformInformation _parent; 			/// <summary>Transform's Parent.</summary>
	[SerializeField] private List<TransformInformation> _children; 	/// <summary>Transform's Children.</summary>
	[SerializeField] private TransformData _data; 					/// <summary>Transform's Data.</summary>

	/// <summary>Gets and Sets parent property.</summary>
	public TransformInformation parent
	{
		get { return _parent; }
		private set { _parent = value; }
	}

	/// <summary>Gets and Sets children property.</summary>
	public List<TransformInformation> children
	{
		get { return _children; }
		private set { _children = value; }
	}

	/// <summary>Gets and Sets data property.</summary>
	public TransformData data
	{
		get { return _data; }
		private set { _data = value; }
	}

	/// <summary>Gets childCount property.</summary>
	public int childCount { get { return children.Count; } }

	/// <summary>Transform to TransformInformation's implicit operator.</summary>
	public static implicit operator TransformInformation(Transform _transform) { return new TransformInformation(_transform); }

	/// <summary>TransformInformation to TransformData's implicit operator.</summary>
	public static implicit operator TransformData(TransformInformation _transformInformation) { return _transformInformation.data; }

	/// <summary>TransformInformation's Contructor.</summary>
	/// <param name="_transform">Transform to retrieve information from.</param>
	/// <param name="_registerParent">Register Parent's Information? true by default.</param>
	/// <param name="_registerChildren">Register Children's Information? true by default.</param>
	public TransformInformation(Transform _transform, bool _registerParent = true, bool _registerChildren = true)
	{
		children = new List<TransformInformation>();
		parent = _registerParent ? _transform.parent : null;
		if(_registerChildren) children.Add(_transform);
		data = _transform;
	}

	/// <returns>Iterates through all parents recursively, starting from this Transform's parent.</returns>
	public IEnumerator<TransformInformation> GetParents()
	{
		TransformInformation currentParent = parent;

		while(currentParent != null)
		{
			yield return currentParent;
			currentParent = currentParent.parent;
		}
	}

	/// <returns>Returns an enumerator that iterates through the children.</returns>
	public IEnumerator<TransformInformation> GetEnumerator()
    {
        return children.GetEnumerator();
    }

    /// <returns>Returns an enumerator that iterates through the children.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
		return GetEnumerator();
    }

    /// <returns>String representing this Transform's Information.</returns>
    public override string ToString()
    {
    	StringBuilder builder = new StringBuilder();

    	builder.Append("Transform's Information: ");
    	if(parent != null)
    	{
    		builder.Append("\nParent's Data: ");
	    	builder.Append(parent.data.ToString());
    	}
    	if(childCount > 0)
    	{
    		builder.Append("\nChild Count: ");
    		builder.Append(childCount.ToString());
    	}
    	
    	builder.Append("\n");
    	builder.Append(data.ToString());	

    	return builder.ToString();
    }
}
}