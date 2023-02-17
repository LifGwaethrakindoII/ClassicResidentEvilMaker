namespace Voidless
{
public interface IDecorator<T> : IComponent<T>
{
	IComponent<T> child { get; set; } 	/// <summary>Decorator's Child.</summary>
}
}