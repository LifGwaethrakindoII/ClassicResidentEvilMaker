using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Voidless
{
[Serializable]
public class CompositeNode<T> : Node<T>, IEnumerable<CompositeNode<T>>
{
    [SerializeField] private List<CompositeNode<T>> _children;

    /// <summary>Gets and Sets children property.</summary>
    public List<CompositeNode<T>> children
    {
        get { return _children; }
        set { _children = value; }
    }

    /// <summary>Parameterless' Constructor.</summary>
    public CompositeNode() { /*...*/ }

    /// <summary>CompositeNode's Constructor.</summary>
    /// <param name="_data">Node's Data.</param>
    /// <param name="_children">Composite's Children.</param>
    public CompositeNode(T _data, params CompositeNode<T>[] _children) : base(_data)
    {
        children = _children.ToList();
    }

    /// <summary>CompositeNode overload constructor.</summary>
    /// <param name="_data">CompositeNode's Data.</param>
    /// <param name="_children">CompositeNode's Children.</param>
    public CompositeNode(T _data, List<CompositeNode<T>> _children) : base(_data)
    {
        children = _children;
    }

    ///<summary> Implementing IEnumerable<T> interface.</summary>
    public virtual IEnumerator<CompositeNode<T>> GetEnumerator()
    {
        return children.GetEnumerator();
    }
}
}