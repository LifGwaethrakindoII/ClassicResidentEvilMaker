using System;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

/*============================================================
**
** Class:  SerializableDictionary
**
** Purpose: Class that inherits from Dictionary<K, V> and has
** also the SerializableAttribute, so it can be drawn thanks
** to Odin's Inspector.
**
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/
namespace Voidless
{
[Serializable]
public class SerializableDictionary<K, V> : Dictionary<K, V>, ISerializationCallbackReceiver
#if UNITY_EDITOR
        , ISerializable
#endif
{
    [SerializeField, HideInInspector] public List<K> _keysList;
    [SerializeField, HideInInspector] public List<V> _valuesList;

    /// <summary>Gets and Sets keysList property.</summary>
    public List<K> keysList
    {
        get { return _keysList; }
        set { _keysList = value; }
    }

    /// <summary>Gets and Sets valuesList property.</summary>
    public List<V> valuesList
    {
        get { return _valuesList; }
        set { _valuesList = value; }
    }

    /// <summary>SerializableDictionary default constructor.</summary>
    public SerializableDictionary() : base()
    {
        keysList = new List<K>();
        valuesList = new List<V>();
    }

#if UNITY_EDITOR
    public SerializableDictionary(SerializationInfo _information, StreamingContext _context) : base(_information, _context)
    {
        keysList = new List<K>();
        valuesList = new List<V>();
    }
#endif

    /// <summary>Implement this method to receive a callback before Unity serializes your object.</summary>
    /// <summary>Saves Dictionary to both Lists.</summary>
    public void OnBeforeSerialize()
    {
        keysList.Clear();
        valuesList.Clear();

        foreach(KeyValuePair<K, V> pair in this)
        {
            keysList.Add(pair.Key);
            valuesList.Add(pair.Value);
        }
    }

    /// <summary>Implement this method to receive a callback after Unity deserializes your object.</summary>
    public void OnAfterDeserialize()
    {
        Clear();
        
        int minSize = Mathf.Min(keysList.Count, valuesList.Count);

        for(int i = 0; i < minSize; i++)
        {
            this[keysList[i]] = valuesList[i];
        }
    }

    /// <summary>Clears internal dictionary and Keys' and Values' Lists.</summary>
    public void ClearAll()
    {
        Clear();
        keysList.Clear();
        valuesList.Clear();
    }

#if UNITY_EDITOR
    /// <summary>Populates a SerializationInfo with the data needed to serialize the target object.</summary>
    public override void GetObjectData( SerializationInfo _info, StreamingContext _context)
    {
        foreach(KeyValuePair<K, V> pair in this)
        {
            _info.AddValue(pair.Key.ToString(), pair.Value);
        }
    }
#endif

    /// <returns>String representing this Dictionary's Entries.</returns>
    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("Dictionary: ");
        builder.Append("\n{");
        builder.Append("\n");
        foreach(KeyValuePair<K, V> pair in this)
        {
            builder.Append("\t[ ");
            builder.Append("Key: ");
            builder.Append(pair.Key.ToString());
            builder.Append(", Value: ");
            builder.Append(pair.Value.ToString());
            builder.Append(" ]");
            builder.Append("\n");
        }
        builder.Append("}");

        return builder.ToString();
    }
}

[Serializable] public struct StringArray { public string[] array; }

/*======================================================================================================================
|   Char Dictionaries:                                                                                                 |
======================================================================================================================*/
[Serializable] public class CharKeyDictionary<V> : SerializableDictionary<char, V> { /*...*/ }
[Serializable] public class CharFloatDictionary : CharKeyDictionary<float> { /*...*/ }
[Serializable] public class CharTransformDictionary : CharKeyDictionary<Transform> { /*...*/ }
[Serializable] public class CharAudioClipDictionary : CharKeyDictionary<AudioClip> { /*...*/ }

/*======================================================================================================================
|   String Dictionaries:                                                                                               |
======================================================================================================================*/
[Serializable] public class StringKeyDictionary<V> : SerializableDictionary<string, V> { /*...*/ }
[Serializable] public class StringStringDictionary : StringKeyDictionary<string> { /*...*/ }
[Serializable] public class StringStringArrayDictionary : StringKeyDictionary<StringArray> { /*...*/ }
[Serializable] public class StringBoolDictionary : StringKeyDictionary<bool> { /*...*/ }
[Serializable] public class StringIntDictionary : StringKeyDictionary<int> { /*...*/ }
[Serializable] public class StringFloatDictionary : StringKeyDictionary<float> { /*...*/ }
[Serializable] public class StringVector2Dictionary : StringKeyDictionary<Vector2> { /*...*/ }
[Serializable] public class StringVector3Dictionary : StringKeyDictionary<Vector3> { /*...*/ }
[Serializable] public class StringQuaternionDictionary : StringKeyDictionary<Quaternion> { /*...*/ }
[Serializable] public class StringEulerRotationDictionary : StringKeyDictionary<EulerRotation> { /*...*/ }
[Serializable] public class StringAudioClipDictionary : StringKeyDictionary<AudioClip> { /*...*/ }
[Serializable] public class StringColorDictionary : StringKeyDictionary<Color> { /*...*/ }
[Serializable] public class StringTransformDictionary : StringKeyDictionary<Transform> { /*...*/ }
[Serializable] public class StringRendererDictionary : StringKeyDictionary<Renderer> { /*...*/ }
[Serializable] public class StringSkinnedMeshRendererDictionary : StringKeyDictionary<SkinnedMeshRenderer> { /*...*/ }
[Serializable] public class StringMaterialDictionary : StringKeyDictionary<Material> { /*...*/ }
[Serializable] public class StringMultiAimConstraintDictionary : StringKeyDictionary<MultiAimConstraint> { /*...*/ }
[Serializable] public class StringTwoBoneIKConstraintDictionary : StringKeyDictionary<TwoBoneIKConstraint> { /*...*/ }

/*======================================================================================================================
|   Int Dictionaries:                                                                                                  |
======================================================================================================================*/
[Serializable] public class IntKeyDictionary<V> : SerializableDictionary<int, V> { /*...*/ }
[Serializable] public class IntStringDictionary : IntKeyDictionary<string> { /*...*/ }
[Serializable] public class IntFloatDictionary : IntKeyDictionary<float> { /*...*/ }

/*======================================================================================================================
|   Float Dictionaries:                                                                                                |
======================================================================================================================*/
[Serializable] public class FloatKeyDictionary<V> : SerializableDictionary<float, V> { /*...*/ }
[Serializable] public class FloatStringDictionary : FloatKeyDictionary<string> { /*...*/ }
[Serializable] public class FloatIntDictionary : FloatKeyDictionary<int> { /*...*/ }
[Serializable] public class FloatColorDictionary : FloatKeyDictionary<Color> { /*...*/ }

/*======================================================================================================================
|   GameObjectTag Dictionaries:                                                                                        |
======================================================================================================================*/
[Serializable] public class TagKeyDictionary<V> : SerializableDictionary<GameObjectTag, V> { /*...*/ }
[Serializable] public class TagIndexDictionary : TagKeyDictionary<CollectionIndex> { /*...*/ }
[Serializable] public class TagIntDictionary : TagKeyDictionary<int> { /*...*/ }

/*======================================================================================================================
|   LayerValue Dictionaries:                                                                                           |
======================================================================================================================*/
[Serializable] public class LayerValueKeyDictionary<V> : SerializableDictionary<LayerValue, V> { /*...*/ }
[Serializable] public class LayerValueStringDictionary : LayerValueKeyDictionary<string> { /*...*/ }
[Serializable] public class LayerValueIntDictionary : LayerValueKeyDictionary<int> { /*...*/ }
[Serializable] public class LayerValueFloatDictionary : LayerValueKeyDictionary<float> { /*...*/ }

/*======================================================================================================================
|   LayerMask Dictionaries:                                                                                           |
======================================================================================================================*/
[Serializable] public class LayerMaskKeyDictionary<V> : SerializableDictionary<LayerMask, V> { /*...*/ }
[Serializable] public class LayerMaskStringDictionary : LayerMaskKeyDictionary<string> { /*...*/ }
[Serializable] public class LayerMaskIntDictionary : LayerMaskKeyDictionary<int> { /*...*/ }
[Serializable] public class LayerMaskFloatDictionary : LayerMaskKeyDictionary<float> { /*...*/ }

}