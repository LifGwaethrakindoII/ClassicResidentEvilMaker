using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Voidless
{
[CreateAssetMenu]
public class ColorSwatches : ScriptableObject
{
	[SerializeField] private StringColorDictionary _map; 	/// <summary>Colors' Mapping.</summary>
	[SerializeField] private FloatColorDictionary test; 	/// <summary>Description.</summary>

	/// <summary>Gets and Sets map property.</summary>
	public StringColorDictionary map
	{
		get { return _map; }
		set { _map = value; }
	}

	/// <summary>Gets orderedMap property.</summary>
	public FloatColorDictionary orderedMap { get { return test; } }

	/// <returns>Color by given key.</returns>
	public Color Get(string _key)
	{
		Color color = default(Color);

		if(map != null) map.TryGetValue(_key, out color);

		return color;
	}

	[Button("Test")]
	private void TEST()
	{
		test = new FloatColorDictionary();
		float min = Mathf.Infinity;
		float max = Mathf.NegativeInfinity;

		foreach(KeyValuePair<string, Color> pair in map)
		{
			string key = pair.Key;
			Color color = pair.Value;
			float t = color.GetNormalizedValue();
			if(!test.ContainsKey(t)) test.Add(t, color);

			Debug.Log("[ColorSwatches] Normalized Value for " + key + ": " + color.GetNormalizedValue());
		}
	}
}
}