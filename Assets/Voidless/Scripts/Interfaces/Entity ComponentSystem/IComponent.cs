namespace Voidless
{
public interface IComponent<T>
{
	/// <summary>Ticks the component.</summary>
	/// <param name="_deltaTime">Additional Time Delta's reference.</param>
	/// <returns>Tick's result.</returns>
	T Tick(float _deltaTime = -1);
}
}