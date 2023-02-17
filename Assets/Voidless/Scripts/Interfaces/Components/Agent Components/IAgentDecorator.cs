using System.Collections.Generic;

namespace Voidless
{
public interface IAgentDecorator<T, R> : IAgentComponent<T, R> where T : IComponentAgent<T, R>
{
	IAgentComponent<T, R> child { get; set; } 	/// <summary>Decorator's Child.</summary>
}
}