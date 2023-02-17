using System.Collections.Generic;

namespace Voidless
{
public interface IComposite<T> : IComponent<T>
{
	List<IComponent<T>> children { get; set; } 	/// <summary>Composite's Children.</summary>
}
}