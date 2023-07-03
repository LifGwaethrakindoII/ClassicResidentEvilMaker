using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

/*============================================================
**
** Struct:  UnityHash
**
** Purpose: A key container that automagically converts into
** a hash-code for internal Unity collections such as:
**
**  - Animator's Parameters
**  - Shader's Properties
**
** This struct overrides GetHashCode() and Equals(object), so
** it can be used as key for HashSets and Dictionaries.
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/

namespace Voidless
{
public enum HashType
{
    Animator,
    Shader
}

[Serializable]
public struct UnityHash : IComparable
{
    [SerializeField] private HashType _hashType;     /// <summary>Type of Hash.</summary>
    [SerializeField] private string _key;            /// <summary>Hash's Key.</summary>
    private int _hash;                               /// <summary>Key's Hash.</summary>

    /// <summary>Gets and Sets hashType property.</summary>
    public HashType hashType
    {
        get { return _hashType; }
        set { _hashType = value; }
    }

    /// <summary>Gets and Sets key property.</summary>
    public string key
    {
        get { return _key; }
        set
        {
            _key = value;
            UpdateHash();
        }
    }

    /// <summary>Gets and Sets hash property.</summary>
    public int hash
    {
        get
        {
            if(_hash == 0 && !string.IsNullOrEmpty(key))  UpdateHash();

            return _hash;
        }
        private set { _hash = value; }
    }

    /// <summary>String to UnityHash operator.</summary>
    public static implicit operator UnityHash(string _key) { return new UnityHash(_key); }

    /// <summary>UnityHash to int operator.</summary>
    public static implicit operator int(UnityHash _unityHash) { return _unityHash.hash; }

    /// <summary>UnityHash's constructor.</summary>
    /// <param name="_key">Hash's Key.</param>
    public UnityHash(string _key) : this()
    {
        key = _key;
    }

    /// <summary>Updates Hash.</summary>
    private void UpdateHash()
    {
        switch(hashType)
        {
            case HashType.Animator:
                hash = Animator.StringToHash(key);
            break;

            case HashType.Shader:
                hash = Shader.PropertyToID(key);
            break;
        }
    }

    /// <summary>Determines whether the specified object is equal to the current object.</summary>
    /// <param name="_object">Object to compare against.</param>
    public override bool Equals (object _object)
    {
        return CompareTo(_object) == 0;
    }

    /// <returns>Hash Code [for HashSet, Dictionary, etc.].</returns>
    public override int GetHashCode()
    {
        return hash.GetHashCode();
    }

    /// <summary>Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.</summary>
    /// <param name="_object">Object to compare againsts.</param>
    public int CompareTo(object _object)
    {
        int x = (int)_object;

        if(hash == x)
        {
            return 0;
        } else if(hash > x)
        {
            return 1;
        } else if(hash < x)
        {
            return -1;
        }

        return -1;
    }

    /// <returns>String representing this UnityHash.</returns>
    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("{ Hash Type: ");
        builder.Append(hashType.ToString());
        builder.Append(", Key: ");
        builder.Append(key.ToString());
        builder.Append(", Hash: ");
        builder.Append(hash.ToString());
        builder.Append(" }");

        return builder.ToString();
    }
}
}