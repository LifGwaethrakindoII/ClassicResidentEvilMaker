using UnityEngine;

namespace Voidless
{
public interface IEventProcessor
{
	/// <summary>Process EventSytem events.</summary>
	/// <param name="_event">Current Event.</param>
	/// <returns>State of the Event Processing.</returns>
	bool ProcessEvents(Event _event);
}
}