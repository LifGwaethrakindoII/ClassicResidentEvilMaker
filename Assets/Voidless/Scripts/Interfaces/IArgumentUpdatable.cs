namespace Voidless
{
public interface IArgumentUpdatable<T>
{
	/// <summary>Callback invoked when the implementer needs to be updated.</summary>
	/// <param name="_argument">Additional argument provided.</param>
	void OnUpdate(T _argument);	
}
}