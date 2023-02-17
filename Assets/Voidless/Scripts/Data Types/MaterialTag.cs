using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct MaterialTag : ISerializationCallbackReceiver
{
	[SerializeField] private string _tag; 	/// <summary>Material Property's Tag.</summary>
	[SerializeField] private int _ID; 		/// <summary>Material Property's ID.</summary>

	/// <summary>Gets and Sets tag property.</summary>
	public string tag
	{
		get { return _tag; }
		set { ID = Shader.PropertyToID(_tag = value); }
	}

	/// <summary>Gets and Sets ID property.</summary>
	public int ID
	{
		get { return _ID; }
		private set { _ID = value; }
	}

	/// <summary>Implicit string to MaterialTag operator.</summary>
	public static implicit operator MaterialTag(string _tag) { return new MaterialTag(_tag); }

	/// <summary>Implicit Material to int property, by returning its ID.</summary>
	public static implicit operator int(MaterialTag _property) { return _property.ID; }

	/// <summary>MaterialPRoperty constructor.</summary>
	/// <param name="_tag">Material Property's Tag.</param>
	public MaterialTag(string _tag) : this()
	{
		tag = _tag;
	}

	/// <summary>Implement this method to receive a callback before Unity serializes your object.</summary>
	public void OnBeforeSerialize()
    {
    	ID = Shader.PropertyToID(tag);
    }

    /// <summary>Implement this method to receive a callback after Unity deserializes your object.</summary>
    public void OnAfterDeserialize()
    {
    	//ID = Shader.PropertyToID(tag);	
    }

	/// <returns>String representing the Material property's components.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("Material Properties = { Tag: ");
		builder.Append(tag);
		builder.Append(", ID: ");
		builder.Append(ID.ToString());

		return builder.ToString();
	}
}
}