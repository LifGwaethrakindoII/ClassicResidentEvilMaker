namespace Voidless
{
public interface IEnergy
{
	float energy { get; set; } 	/// <summary>Energy's property.</summary>
	float maxEnergy { get; } 	/// <summary>Maximum Energy's Points.</summary>

	/// <summary>Callbakc invoked when the implementer consumes energy.</summary>
	/// <param name="_energy">Energy points to take away in energy consumption.</param>
	void OnConsumeEnergy(float _energy);

	/// <summary>Recovers Energy points to implementer.</summary>
	/// <param name="_energy">Energy points to recover.</param>
	void OnEnergyRecover(float _hp);

	/// <summary>Action that ought to be called when the Energy Points goes below or equal to 0.</summary>
	void OnEnergyCeases();
}
}