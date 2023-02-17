using UnityEngine;

namespace Voidless
{
public static class VColor
{
	public const float WEIGHT_DISTANCE_RED = 0.3f;
	public const float WEIGHT_DISTANCE_GREEN = 0.59f;
	public const float WEIGHT_DISTANCE_BLUE = 0.11f;
	public const float MAX_COLOR_SQUAREDISTANCE = 9.0f; // (3.0f + 4.0f + 2.0f) from Euclidean Distance weights

	/// <summary>Sets Color Alpha.</summary>
	/// <param name="_color">The Color that will have its Alpha modified.</param>
	/// <param name="_alpha">Updated Color Alpha Component.</param>
	/// <returns>New modified Color.</returns>
	public static Color WithAlpha(this Color _color, float _alpha)
	{
		return _color = new Color(_color.r, _color.g, _color.b, _alpha.Clamp(-1.0f, 1.0f));
	}

	/// <summary>Sets Color Red.</summary>
	/// <param name="_color">The Color that will have its Red modified.</param>
	/// <param name="_red">Updated Color Red Component.</param>
	/// <returns>New modified Color.</returns>
	public static Color WithRed(this Color _color, float _red)
	{
		return _color = new Color(_red.Clamp(-1.0f, 1.0f), _color.g, _color.b, _color.a);
	}

	/// <summary>Sets Color Green.</summary>
	/// <param name="_color">The Color that will have its Green modified.</param>
	/// <param name="_green">Updated Color Green Component.</param>
	/// <returns>New modified Color.</returns>
	public static Color WithtGreen(this Color _color, float _green)
	{
		return _color = new Color(_color.r, _green.Clamp(-1.0f, 1.0f), _color.b, _color.a);
	}

	/// <summary>Sets Color Blue.</summary>
	/// <param name="_color">The Color that will have its Blue modified.</param>
	/// <param name="_blue">Updated Color Blue Component.</param>
	/// <returns>New modified Color.</returns>
	public static Color WithBlue(this Color _color, float _blue)
	{
		return _color = new Color(_color.r, _color.g, _blue.Clamp(-1.0f, 1.0f), _color.a);
	}

	/// <summary>Sets Color Alpha.</summary>
	/// <param name="_color">The Color that will have its Alpha modified.</param>
	/// <param name="_alpha">Updated Color Alpha Component.</param>
	public static void SetAlpha(ref Color _color, float _alpha)
	{
		_color = new Color(_color.r, _color.g, _color.b, _alpha.Clamp(-1.0f, 1.0f));
	}

	/// <summary>Sets Color Red.</summary>
	/// <param name="_color">The Color that will have its Red modified.</param>
	/// <param name="_red">Updated Color Red Component.</param>
	public static void SetRed(ref Color _color, float _red)
	{
		_color = new Color(_red.Clamp(-1.0f, 1.0f), _color.g, _color.b, _color.a);
	}

	/// <summary>Sets Color Green.</summary>
	/// <param name="_color">The Color that will have its Green modified.</param>
	/// <param name="_green">Updated Color Green Component.</param>
	public static void SetGreen(ref Color _color, float _green)
	{
		_color = new Color(_color.r, _green.Clamp(-1.0f, 1.0f), _color.b, _color.a);
	}

	/// <summary>Sets Color Blue.</summary>
	/// <param name="_color">The Color that will have its Blue modified.</param>
	/// <param name="_blue">Updated Color Blue Component.</param>
	public static void SetBlue(ref Color _color, float _blue)
	{
		_color = new Color(_color.r, _color.g, _blue.Clamp(-1.0f, 1.0f), _color.a);
	}

	/// <returns>Random Color.</returns>
	public static Color Random(bool _randomAlpha = false)
	{
		return new Color(
			UnityEngine.Random.Range(0.0f, 1.0f),
			UnityEngine.Random.Range(0.0f, 1.0f),
			UnityEngine.Random.Range(0.0f, 1.0f),
			_randomAlpha ? UnityEngine.Random.Range(0.0f, 1.0f) : 1.0f
		);
	}

	/// <returns>Color with Perlin Noise.</returns>
	public static Color PerlinNoiseColor(float x, float y, bool _randomAlpha = false)
	{
		float c = Mathf.PerlinNoise(x, y);

		return new Color(
			c,
			c,
			c,
			_randomAlpha ? UnityEngine.Random.Range(0.0f, 1.0f) : 1.0f
		);
	}

	/// <summary>Interpolates between an array of pixels A towards an array of pixels B.</summary>
	/// <param name="a">Pixels from A.</param>
	/// <param name="b">Pixels from B.</param>
	/// <param name="t">Normalized Time [internally clamped between 0.0f and 1.0f].</param>
	/// <returns>Array of pixels from the interpolation of pixels of A and pixels of B at normalized time t.</returns>
	public static Color[] Interpolate(Color[] a, Color [] b, float t)
	{
		//if((a == null || b == null) || (a.Length == 0 || b.Length == 0)) return null;

		int count = Mathf.Min(a.Length, b.Length);
		Color[] colors = new Color[count];
		t = Mathf.Clamp(t, 0.0f, 1.0f);

		/*
		- Path w can be parameterized by two general motion vectors mA and mB, w = (mA, mB)
		- Optical Flow: Given a pixel p in A, determine which pixel in B matches best. Forward: A -> B; Backward: B -> A
		- The transition points are related to path parameters by pA = p + mA; pB = p - mB
		- mA and mB are constrained to have the same direction: vA = mA / |mA| = mB / |mB| = vB
		- The distance between transition points can be used to predict forward (vA) and backward (vB) flows: vA = pB - pA; vB = pA - pB

		Implementation:

		- Let L denote the finite set of all possible discrete paths, the path computation is done hierarchically.
		*/

		for(int i = 0; i < count; i++)
		{
			colors[i] = Color.Lerp(a[i], b[i], t);
		}

		return colors;
	}

	/// <summary>Interpolates between an array of pixels A towards an array of pixels B.</summary>
	/// <param name="a">Pixels from A.</param>
	/// <param name="b">Pixels from B.</param>
	/// <param name="t">Normalized Time [internally clamped between 0.0f and 1.0f].</param>
	/// <returns>Array of pixels from the interpolation of pixels of A and pixels of B at normalized time t.</returns>
	public static Color32[] Interpolate(Color32[] a, Color32 [] b, float t)
	{
		int count = Mathf.Min(a.Length, b.Length);
		Color32[] colors = new Color32[count];
		t = Mathf.Clamp(t, 0.0f, 1.0f);

		for(int i = 0; i < count; i++)
		{
			colors[i] = Color32.Lerp(a[i], b[i], t);
		}

		return colors;
	}

	/// <summary>[Experiment] returns a Beizer Color between 2 colors and a reference color.</summary>
	/// <param name="a">Color A.</param>
	/// <param name="b">Color B.</param>
	/// <param name="c">Color C [as tangent color].</param>
	/// <param name="t">Normalized time t [internally clamped].</param>
	/// <returns>Beizer curve of 2 colors given a tangent color.</returns>
	public static Color CuadraticBeizer(Color a, Color b, Color c, float t)
	{
		t = Mathf.Clamp(t, 0.0f, 1.0f);

		return Color.Lerp(Color.Lerp(a, c, t), Color.Lerp(c, b, t), t);
	}

	/// <summary>Calculates the average value of provided Color.</summary>
	/// <param name="_color">Color to extract average from.</param>
	/// <param name="_includeAlpha">Include Alpha in the average? false by default.</param>
	/// <returns>Average value of provided color.</returns>
	public static float Average(this Color _color, bool _includeAlpha = false)
	{
		float sum = _color.r + _color.g + _color.g;
		float n = 3.0f;

		if(_includeAlpha)
		{
			sum += _color.a;
			n++;
		} 

		return sum / n;
	}

	/// <summary>Calculates the Euclidean square distance between 2 colors.</summary>
	/// <param name="a">Color A.</param>
	/// <param name="b">Color B.</param>
	/// <returns>Euclidean square Distance between 2 colors.</returns>
	public static float SquareDistance(Color a, Color b)
	{
		float dr = (a.r - b.r);
		float dg = (a.g - b.g);
		float db = (a.b - b.b);
		float R = (a.r + b.r) * 0.5f;

		dr *= dr;
		dg *= dg;
		db *= db;

		return R < 128.0f ? (2.0f * dr) + (4.0f * dg) + (3.0f * db) : (3.0f * dr) + (4.0f * dg) + (2.0f * db);
	}

	/// <summary>Calculates the Euclidean distance between 2 colors.</summary>
	/// <param name="a">Color A.</param>
	/// <param name="b">Color B.</param>
	/// <returns>Euclidean Distance between 2 colors.</returns>
	public static float Distance(Color a, Color b)
	{
		return Mathf.Sqrt(SquareDistance(a, b));
	}

	public static float GetNormalizedValue(this Color a)
	{
		return SquareDistance(a, Color.black) /  MAX_COLOR_SQUAREDISTANCE;
	}
}
}