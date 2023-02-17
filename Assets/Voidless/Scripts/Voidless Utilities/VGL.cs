using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VGL
{
	/// <summary>Draws a Cuadratic Beizer Curve Line between an initial point, a tangent and a final point on the scene.</summary>
	/// <param name="_initial">Curve's initial point [P0].</param>
	/// <param name="_finalPoint">Curve's final point [Pf].</param>
	/// <param name="_tangent">Curve's tangent [P1].</param>
	/// <param name="_timeSplit">Time split. The bigger the time split, the smoother the curve will look.</param>
	/// <param name="_color">Line's color.</param>
	public static void DrawCuadraticBeizerCurve(Vector3 _initialPoint, Vector3 _finalPoint, Vector3 _tangent, int _timeSplit, Color _color)
	{
		float timeSplitInverseMultiplicative = (1f / (1f * _timeSplit));

#if UNITY_5_6
		GL.Begin(GL.LINE_STRIP);
		GL.Color(_color);

		for(int i = 0; i < _timeSplit; i++)
		{
			Vector3 newPosition = VMath.CuadraticBeizer(_initialPoint, _finalPoint, _tangent, (i * timeSplitInverseMultiplicative));
			GL.Vertex3(newPosition.x, newPosition.y, newPosition.z);	
		}

		GL.End();

#else
		for(int i = 0; i < _timeSplit - 1; i++)
		{
			GL.Begin(GL.LINES);
			GL.Color(_color);

			Vector3 newStartPosition = VMath.CuadraticBeizer(_initialPoint, _finalPoint, _tangent, (i * timeSplitInverseMultiplicative));
			Vector3 newFinalPosition = VMath.CuadraticBeizer(_initialPoint, _finalPoint, _tangent, ((i + 1) * timeSplitInverseMultiplicative));
			
			GL.Vertex3(newStartPosition.x, newStartPosition.y, newStartPosition.z);
			GL.Vertex3(newFinalPosition.x, newFinalPosition.y, newFinalPosition.z);

			GL.End();
		}
#endif

	}

	/// <summary>Draws a Cubic Beizer Curve Line between an initial point, a tangent and a final point on the scene.</summary>
	/// <param name="_initial">Curve's initial point [P0].</param>
	/// <param name="_finalPoint">Curve's final point [Pf].</param>
	/// <param name="_startTangent">Curve's first tangent [P1].</param>
	/// <param name="_endTangent">Curve's end tangent [P2].</param>
	/// <param name="_timeSplit">Time split. The bigger the time split, the smoother the curve will look.</param>
	/// <param name="_color">Line's color.</param>
	public static void DrawCubicBeizerCurve(Vector3 _initialPoint, Vector3 _finalPoint, Vector3 _startTangent, Vector3 _endTangent, int _timeSplit, Color _color)
	{
		float timeSplitInverseMultiplicative = (1f / (1f * _timeSplit));

#if UNITY_5_6
		GL.Begin(GL.LINE_STRIP);
		GL.Color(_color);

		for(int i = 0; i < _timeSplit; i++)
		{
			Vector3 newPosition = VMath.CubicBeizer(_initialPoint, _finalPoint, _startTangent, _endTangent, (i * timeSplitInverseMultiplicative));
			GL.Vertex3(newPosition.x, newPosition.y, newPosition.z);	
		}

		GL.End();

#else
		for(int i = 0; i < _timeSplit - 1; i++)
		{
			GL.Begin(GL.LINES);
			GL.Color(_color);

			Vector3 newStartPosition = VMath.CubicBeizer(_initialPoint, _finalPoint, _startTangent, _endTangent, (i * timeSplitInverseMultiplicative));
			Vector3 newFinalPosition = VMath.CubicBeizer(_initialPoint, _finalPoint, _startTangent, _endTangent, ((i + 1) * timeSplitInverseMultiplicative));
			
			GL.Vertex3(newStartPosition.x, newStartPosition.y, newStartPosition.z);
			GL.Vertex3(newFinalPosition.x, newFinalPosition.y, newFinalPosition.z);

			GL.End();
		}
#endif
	}
}
}