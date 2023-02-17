using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct LayerValue : ISerializationCallbackReceiver
{
	[SerializeField] private int _value; 		/// <summary>Layer Mask's Value (int required to GameObject's layer property).</summary>
	[SerializeField] private string _name; 		/// <summary>Layer's Name.</summary>
#if UNITY_EDITOR
	public int index; 							/// <summary>Property Drawer's Index.</summary>
#endif

	/// <summary>Gets and Sets value property.</summary>
	public int value
	{
		get { return _value; }
		set
		{
			if(value.IsValueALayerBit())
			{
				_value = value;
				_name = LayerMask.LayerToName(_value);
			}
			else Debug.LogError("[LayerValue] Layer cannot have either multiple LayerMask's values or a value greater than 32.");
		}
	}

	/// <summary>Gets and Sets name property.</summary>
	public string name
	{
		get { return _name; }
		set
		{
			_name = value;
			_value = LayerMask.NameToLayer(_name);
		}
	}

	/// <summary>Implicit string to LayerValue operator.</summary>
	public static implicit operator LayerValue(string _name) { return new LayerValue(_name); }

	/// <summary>Implicit int to LayerValue operator.</summary>
	public static implicit operator LayerValue(int _value) { return new LayerValue(_value); }

	/// <summary>Implicit LayerValue to int operator.</summary>
	public static implicit operator int(LayerValue _layerInteger) { return _layerInteger.value; }

	/// <summary>Implicit LayerValue to string operator.</summary>
	public static implicit operator string(LayerValue _layerInteger) { return _layerInteger.name; }

	/// <summary>LayerValue's constructor.</summary>
	/// <param name="_name">Layer Mask's name associated with this Layer Value.</param>
	public LayerValue(string _name) : this()
	{
		name = _name;
	}

	/// <summary>LayerValue's constructor.</summary>
	/// <param name="_value">Layer Mask's value.</param>
	public LayerValue(int _value) : this()
	{
		value = _value;
	}

	/// <returns>Value turned into mask [as LayerMask].</returns>
	public LayerMask ToLayerMask()
	{
		return 1 << value;
	}

	/// <summary>Implement this method to receive a callback before Unity serializes your object.</summary>
	public void OnBeforeSerialize()
    {
    	if(!string.IsNullOrEmpty(name))
    	{
    		value = LayerMask.NameToLayer(name);
#if UNITY_EDITOR
    		index = VLayerMask.NameToIndex(name);
#endif
    	}
    }

    /// <summary>Implement this method to receive a callback after Unity deserializes your object.</summary>
    public void OnAfterDeserialize()
    {
    	//...
    }

	/// <returns>String representing LayerValue.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("LayerValue's Data: ");
		builder.Append("\n{");
		builder.Append("\n\tLayer's Value: ");
		builder.Append(value.ToString());
		builder.Append("\n\tLayer's Name: ");
		builder.Append(name.ToString());
		builder.Append("\n}");

		return builder.ToString();
	}
}
}