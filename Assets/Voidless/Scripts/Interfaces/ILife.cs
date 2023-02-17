using System;

namespace Voidless
{
public interface ILife<T> where T : IComparable<T>
{
	T HP { get; set; } 	/// <summary>Health Points property.</summary>
	T maxHP { get; } 	/// <summary>Maximum Health Points.</summary>

	/// <summary>Callbakc invoked when the implementer receives damage.</summary>
	/// <param name="_hp">Health points to take away in damage take.</param>
	void OnDamageTaken(T _hp);

	/// <summary>Recovers Health points to implementer.</summary>
	/// <param name="_hp">Health points to recover.</param>
	void OnHpRecover(T _hp);

	/// <summary>Action that ought to be called when the Health Points goes below or equal to 0.</summary>
	void OnHPCeases();
}
}