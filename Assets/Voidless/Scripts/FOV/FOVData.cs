using System;
using System.Text;

namespace Voidless
{
[Serializable]
public struct FOVData
{
	public float angle; 		/// <summary>FOV's Angle.</summary>
	public float nearPlane; 	/// <summary>Near's Plane.</summary>
	public float farPlane; 		/// <summary>Far's Plane.</summary>
	public float aspect; 		/// <summary>FOV's Aspect.</summary>
	public float width; 		/// <summary>FOV's Width.</summary>
	public float height; 		/// <summary>FOV's. Height</summary>
	public float planeDelta; 	/// <summary>Difference between Near's Plane and Far's Plane.</summary>
	public float inverseZ; 		/// <summary>Z's Multiplicative Inverse.</summary>

	/// <returns>String representing DOV's Data.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("Angle: ");
		builder.AppendLine(angle.ToString());
		builder.Append("Near Plane: ");
		builder.AppendLine(nearPlane.ToString());
		builder.Append("Far Plane: ");
		builder.AppendLine(farPlane.ToString());
		builder.Append("Width: ");
		builder.AppendLine(width.ToString());
		builder.Append("Height: ");
		builder.AppendLine(height.ToString());
		builder.Append("Aspect: ");
		builder.AppendLine(aspect.ToString());
		/*builder.Append("Plane's Delta: ");
		builder.AppendLine(planeDelta.ToString());
		builder.Append("Z's Multiplicative Inverse: ");
		builder.AppendLine(inverseZ.ToString());*/

		return builder.ToString();
	}
}
}