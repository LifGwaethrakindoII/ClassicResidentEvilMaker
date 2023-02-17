using System;
using System.Text;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct Distance : ISerializationCallbackReceiver
{
	[SerializeField] private float _distance; 			/// <summary>Distance, without alteration.</summary>
	[SerializeField] private float _squareDistance; 	/// <summary>Distance raised to the square.</summary>

	/// <summary>Gets and Sets distance property.</summary>
 	public float distance
 	{
 		get { return _distance; }
 		set
 		{
 			_distance = value;
 			squareDistance = _distance * _distance;
 		}
 	} 

 	/// <summary>Gets and Sets squareDistance property.</summary>
 	public float squareDistance
 	{
 		get { return _squareDistance; }
 		private set { _squareDistance = value; }
 	}

 	/// <summary>Implicit float to Distance operator.</summary>
 	public static implicit operator Distance(float _distance) { return new Distance(_distance); }

 	/// <summary>Implicit Distance to float operator.</summary>
 	public static implicit operator float(Distance _distance) { return _distance.squareDistance; }

 	/// <summary>Distance constructor.</summary>
 	/// <param name="_distance">Distance to be internally referred as square distance.</param>
 	public Distance(float _distance) : this()
 	{
 		distance = _distance;
 	}

 	/// <summary>Implement this method to receive a callback before Unity serializes your object.</summary>
	public void OnBeforeSerialize()
    {
    	squareDistance = distance * distance;	
    }

    /// <summary>Implement this method to receive a callback after Unity deserializes your object.</summary>
    public void OnAfterDeserialize()
    {
    	squareDistance = distance * distance;
    }

 	/// <returns>String showing both distances.</returns>
 	public override string ToString()
 	{
 		StringBuilder builder = new StringBuilder();

 		builder.Append("{ ");
 		builder.Append("Distance : ");
 		builder.Append(distance.ToString());
 		builder.Append(", Square Distance: ");
 		builder.Append(squareDistance.ToString());
 		builder.Append(" }");

 		return builder.ToString();
 	}
}
}