using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

namespace Voidless
{
[Serializable]
public class SerializableHashSet<T> : HashSet<T>, ISerializationCallbackReceiver
#if UNITY_EDITOR
        , ISerializable
#endif
{
	[SerializeField, HideInInspector] public List<T> _list;

	/// <summary>Gets and Sets list property.</summary>
	public List<T> list
	{
		get { return _list; }
		set { _list = value; }
	}

	/// <summary>SerializableHashSet's constructor.</summary>
	public SerializableHashSet() : base()
	{
		list = new List<T>();
	}

#if UNITY_EDITOR
	/// <summary>Constructor for Editor mode.</summary>
    public SerializableHashSet(SerializationInfo _information, StreamingContext _context) : base(_information, _context)
    {
        list = new List<T>();
    }
#endif

    /// <summary>Implement this method to receive a callback before Unity serializes your object.</summary>
    /// <summary>Saves HashSet to List.</summary>
    public void OnBeforeSerialize()
    {
        HashSet<T> copy = new HashSet<T>(list); 
        //list.Clear();

        foreach(T item in this)
        {
            if(!copy.Contains(item)) list.Add(item);
        }
    }

    /// <summary>Implement this method to receive a callback after Unity deserializes your object.</summary>
    public void OnAfterDeserialize()
    {
        Clear();
        
        foreach(T item in list)
        {
        	Add(item);
        }
    }

    /// <summary>Clears internal HashSet and List.</summary>
    public void ClearAll()
    {
        Clear();
        list.Clear();
    }

#if UNITY_EDITOR
    /// <summary>Populates a SerializationInfo with the data needed to serialize the target object.</summary>
    public override void GetObjectData(SerializationInfo _info, StreamingContext _context)
    {
        foreach(T item in this)
        {
            _info.AddValue(item.GetHashCode().ToString(), item);
        }
    }
#endif

    /// <returns>String representing this Dictionary's Entries.</returns>
    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("HashSet: ");
        builder.Append("\n{");
        builder.Append("\n");
        foreach(T item in this)
        {
            builder.Append("\t[ ");
            builder.Append(item.ToString());
            builder.Append(" ]");
            builder.Append("\n");
        }
        builder.Append("}");

        return builder.ToString();
    }
}

[Serializable] public class PoolGameObjectHashSet : SerializableHashSet<PoolGameObject> { /*...*/ }
[Serializable] public class VCameraTargetHashSet : SerializableHashSet<VCameraTarget> { /*...*/ }
}