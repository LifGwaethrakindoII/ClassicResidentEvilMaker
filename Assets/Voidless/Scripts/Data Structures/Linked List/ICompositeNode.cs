using System.Collections;
using System.Collections.Generic;

namespace Voidless
{
public interface ICompositeNode<T> : INode<T>, IEnumerable<ICompositeNode<T>>
{
    List<ICompositeNode<T>> children { get; set; }
}
}