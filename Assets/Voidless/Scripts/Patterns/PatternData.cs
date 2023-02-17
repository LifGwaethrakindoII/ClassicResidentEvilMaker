using System.Collections;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[System.Serializable]
[XmlRoot(Namespace="Voidless", IsNullable = false)]
public struct PatternData
{
	[SerializeField][XmlElement("Head_Orientation")] public OrientationSemantics headOrientation; 					/// <summary>Head's Orientation.</summary>
	[SerializeField][XmlElement("Left_Hand_Orientation")] public OrientationSemantics leftHandOrientation; 			/// <summary>LeftHand's Orientation.</summary>
	[SerializeField][XmlElement("Left_Paddle_Orientation")] public OrientationSemantics leftPaddleOrientation; 		/// <summary>Left Paddle's Orientation.</summary>
	[SerializeField][XmlElement("Right_Hand_Orientation")] public OrientationSemantics rightHandOrientation; 		/// <summary>Right Hand's Orientation.</summary>
	[SerializeField][XmlElement("Right_Paddle_Orientation")] public OrientationSemantics rightPaddleOrientation; 	/// <summary>Right Paddle's Orientation.</summary>

	/// <summary>Equals operator.</summary>
	public static bool operator == (PatternData a, PatternData b)
	{
		return (a.headOrientation.HasFlag(b.headOrientation)
		&& a.leftHandOrientation.HasFlag(b.leftHandOrientation)
		&& a.leftPaddleOrientation.HasFlag(b.leftPaddleOrientation)
		&& a.rightHandOrientation.HasFlag(b.rightHandOrientation)
		&& a.rightPaddleOrientation.HasFlag(b.rightPaddleOrientation));
	}

	/// <summary>Not Equal operator.</summary>
	public static bool operator != (PatternData a, PatternData b)
	{
		return (!a.headOrientation.HasFlag(b.headOrientation)
		|| !a.leftHandOrientation.HasFlag(b.leftHandOrientation)
		|| !a.leftPaddleOrientation.HasFlag(b.leftPaddleOrientation)
		|| !a.rightHandOrientation.HasFlag(b.rightHandOrientation)
		|| !a.rightPaddleOrientation.HasFlag(b.rightPaddleOrientation));
	}

	public PatternData(OrientationSemantics _headOrientation, OrientationSemantics _leftHandOrientation, OrientationSemantics _leftPaddleOrientation, OrientationSemantics _rightHandOrientation, OrientationSemantics _rightPaddleOrientation)
	{
		this.headOrientation = _headOrientation;
		this.leftHandOrientation = _leftHandOrientation;
		this.leftPaddleOrientation = _leftPaddleOrientation;
		this.rightHandOrientation = _rightHandOrientation;
		this.rightPaddleOrientation = _rightPaddleOrientation;
	}

	/// <summary>Determines whether two object instances are equal.</summary>
	/// <param name="_object">Object to compare this object against.</param>
	/// <returns>True if both objects are considered equal, false otherwise.</returns>
	public override bool Equals(object _object)
	{
		return (_object.GetType() == typeof(PatternData) && this == (PatternData)_object);
	}

	/// <returns>Default hash function.</returns>
	public override int GetHashCode()
	{
		return (headOrientation
		| leftHandOrientation
		| leftPaddleOrientation
		| rightHandOrientation
		| rightPaddleOrientation).GetHashCode();
	}

	/// <returns>String representing all Orientation semantics of the User's joints.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("Head Orientation: ");
		builder.AppendLine(headOrientation.ToString());
		builder.Append("Left Hand Orientation: ");
		builder.AppendLine(leftHandOrientation.ToString());
		builder.Append("Left Paddle Orientation: ");
		builder.AppendLine(leftPaddleOrientation.ToString());
		builder.Append("Right Hand Orientation: ");
		builder.AppendLine(rightHandOrientation.ToString());
		builder.Append("Right Paddle Orientation: ");
		builder.AppendLine(rightPaddleOrientation.ToString());

		return builder.ToString();
	}
}
}