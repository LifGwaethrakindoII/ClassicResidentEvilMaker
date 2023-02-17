namespace Voidless
{
public interface IVitality
{
	float HP { get; set; } 	/// <summary>Health Points property.</summary>
	float maxHP { get; } 	/// <summary>Maximum Health Points.</summary>

	/// <summary>Inflicts damage to implementer.</summary>
	/// <param name="_hp">Health points to take away in damage take.</param>
	void InflictDamage(float _hp);

	/// <summary>Recovers Health points to implementer.</summary>
	/// <param name="_hp">Health points to recover.</param>
	void RecoverHP(float _hp);

	/// <summary>Action that ought to be called when the Health Points goes below or equal to 0.</summary>
	void OnHPCeases();
}
}