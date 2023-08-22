using System.Collections;
using System.Collections.Generic;

namespace Voidless
{
public interface INode<T> : IEnumerable<INode<T>>
{
    T data { get; set; }
    INode<T> next { get; set; }

    /// <summary>Gets updated data from this Node.</summary>
    T Update();
}
}