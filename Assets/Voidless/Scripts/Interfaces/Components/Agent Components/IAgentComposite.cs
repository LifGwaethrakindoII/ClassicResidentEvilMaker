using System.Collections.Generic;

namespace Voidless
{
public interface IAgentComposite<T, R> : IAgentComponent<T, R> where T : IComponentAgent<T, R>
{
	List<IAgentComponent<T, R>> children { get; set; } 	/// <summary>Composite's Children.</summary>
}
}