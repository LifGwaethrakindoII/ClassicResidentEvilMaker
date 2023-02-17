using System.Collections.Generic;

namespace Voidless
{
public interface IAgentLeaf<T, R> : IAgentComponent<T, R> where T : IComponentAgent<T, R>
{
	
}
}