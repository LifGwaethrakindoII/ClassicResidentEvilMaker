using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.REMaker
{
[Serializable]
public class DialogueNode : IEnumerable<DialogueNode>
{
    [SerializeField] private DialogueText _data;
    [SerializeField] private List<DialogueNode> _children;

    /// <summary>Gets and Sets data property.</summary>
    public DialogueText data
    {
        get { return _data; }
        set { _data = value; }
    }

    /// <summary>Gets and Sets children property.</summary>
    public List<DialogueNode> children
    {
        get { return _children; }
        set { _children = value; }
    }

    /// <summary>DialogueNode default constructor.</summary>
    /// <param name="_data">DialogueNode's Data.</param>
    public DialogueNode(DialogueText _data)
    {
        data = _data;
    }

    /// <summary>DialogueNode's Constructor.</summary>
    /// <param name="_data">DialogueText's Data.</param>
    /// <param name="_childrenData">DialogueText data that will be converted into children nodes.</param>
    public DialogueNode(DialogueText _data, params DialogueText[] _childrenData) : this(_data)
    {
        if(_childrenData != null)
        {
            List<DialogueNode> newChildren = new List<DialogueNode>();

            foreach(DialogueText childData in _childrenData)
            {
                newChildren.Add(new DialogueNode(childData));
            }

            children = newChildren;
        }
    }

    ///<summary> Implementing IEnumerable<T> interface.</summary>
    public virtual IEnumerator<DialogueNode> GetEnumerator()
    {
        return children.GetEnumerator();
    }

    ///<summary> Implementing IEnumerable interface (non-generic version).</summary>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <returns>String representing this Dialogue Node.</returns>
    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("{ Text: ");
        builder.Append(data.ToString());
        builder.Append(", Children: ");
        if(children != null) foreach(DialogueNode child in children)
        {
            builder.Append(child.ToString());
            builder.Append(", ");
        }
        else
        {
            builder.Append("NULL");
        }
        builder.Append(" }");

        return builder.ToString();
    }
}
}