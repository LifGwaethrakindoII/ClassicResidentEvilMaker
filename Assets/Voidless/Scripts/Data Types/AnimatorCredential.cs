using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct AnimatorCredential : ISerializationCallbackReceiver
{
	[SerializeField] private string _tag; 	/// <summary>Animator Controller's Parameter Tag.</summary>
	[SerializeField] private int _ID; 		/// <summary>Animator Controller's Parameter Tag.</summary>

	/// <summary>Gets and Sets tag property.</summary>
	public string tag
	{
		get { return _tag; }
		set { _ID = Animator.StringToHash(_tag = value); }
	}

	/// <summary>Gets and Sets ID property.</summary>
	public int ID
	{
		get { return _ID; }
		set { _ID = value; }
	}

	/// <summary>Implicit AnimatorCredential equals AnimatorCredential value bool operator.</summary>
	public static bool operator == (AnimatorCredential a, AnimatorCredential b) { return a.ID == b.ID; }

	/// <summary>Implicit AnimatorCredential not-equals AnimatorCredential value bool operator.</summary>
	public static bool operator != (AnimatorCredential a, AnimatorCredential b) { return a.ID != b.ID; }

	/// <summary>Implicit string to AnimatorCredential.</summary>
	public static implicit operator AnimatorCredential(string _tag) { return new AnimatorCredential(_tag); }

	/// <summary>Implicit AnimatorCredential to int.</summary>
	public static implicit operator int(AnimatorCredential _credential) { return _credential.ID; }

	/// <summary>AnimatorCredential constructor.</summary>
	/// <param name="_tag">Parameter's string tag.</param>
	public AnimatorCredential(string _tag) : this()
	{
		tag = _tag;
	}

	/// <summary>Implement this method to receive a callback before Unity serializes your object.</summary>
	public void OnBeforeSerialize()
    {
    	ID = Animator.StringToHash(tag);
    }

    /// <summary>Implement this method to receive a callback after Unity deserializes your object.</summary>
    public void OnAfterDeserialize()
    {
    	ID = Animator.StringToHash(tag);	
    }

	/// <returns>String representing the Animator Controller's identifiers.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("Animator Credential's Data = { Tag: ");
		builder.Append(tag);
		builder.Append(", ID: ");
		builder.Append(ID.ToString());

		return builder.ToString();
	}
}
}