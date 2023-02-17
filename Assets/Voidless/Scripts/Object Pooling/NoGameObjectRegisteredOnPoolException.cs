using System;

namespace Voidless
{
[Serializable]
public class NoGameObjectRegisteredOnPoolException : Exception
{
	/// <summary>[Override] Gets a message that describes the current exception.</summary>
	public override string Message { get { return "Pool does not contain a Queue for the given GameObject."; } }

	/// <summary>Default NoGameObjectRegisteredOnPoolException constructor.</summary>
	public NoGameObjectRegisteredOnPoolException() { }

	/// <summary>Overload NoGameObjectRegisteredOnPoolException constructor.</summary>
	/// <param name="_message">Additional message to debug.</param>
    public NoGameObjectRegisteredOnPoolException(string _message) : base(_message) { }

    /// <summary>Overload NoGameObjectRegisteredOnPoolException constructor.</summary>
	/// <param name="_message">Additional message to debug.</param>
	/// <param name="_innerException">Inner's Exception.</param>
    public NoGameObjectRegisteredOnPoolException(string _message, Exception _innerException) : base(_message, _innerException) { }
}
}