using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace Voidless.REMaker
{
[Serializable]
public struct DialogueText
{
    [SerializeField] private string _ID;
    [SerializeField][TextArea] private string[] _lines;

    /// <summary>Gets and Sets ID property.</summary>
    public string ID
    {
        get { return _ID; }
        set { _ID = value; }
    }

    /// <summary>Gets and Sets lines property.</summary>
    public string[] lines
    {
        get { return _lines; }
        set { _lines = value; }
    }

    /// <summary>DialogueText's Constructor.</summary>
    /// <param name="_ID">ID.</param>
    /// <param name="_lines">Lines.</param>
    public DialogueText(string _ID, params string[] _lines) : this()
    {
        ID = _ID;
        lines = _lines;
    }

    /// <returns>String representing this Dialogue Text.</returns>
    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("{ ID: ");
        builder.Append(ID);
        builder.Append(", Text: ");
        if(lines == null || lines.Length == 0)
        {
            builder.Append("NULL");

        } else foreach(string line in lines)
        {
            builder.Append(line);
            builder.Append(", ");
        }
        builder.Append(" }");

        return builder.ToString();
    }
}
}