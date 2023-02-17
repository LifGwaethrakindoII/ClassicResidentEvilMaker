using System.Collections.Generic;

namespace Voidless
{
public interface IAgentComponent<T, R> where T : IComponentAgent<T, R>
{
	/// <summary>Ticks the component.</summary>
	/// <param name="_agent">Component's Agent to tick.</param>
	/// <returns>Tick's result.</returns>
	R Tick(T _agent);
}
}