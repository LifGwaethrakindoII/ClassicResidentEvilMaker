using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// Easings: https://easings.net/#

/*============================================================
**
** Class:  VMath
**
** Purpose: A library full of all-math-related functions. Not
** only for numbers, also for Vectors, Matrices, Quaternions,
** etc.
**
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/

namespace Voidless
{
[Flags]
public enum Easings
{
	Linear = 0, 																		/// <summary>Linear Easing.</summary>
	EaseIn = 1, 																		/// <summary>Ease-In Easing.</summary>
	EaseOut = 2, 																		/// <summary>Ease-Out Easing.</summary>
	Arc = 4, 																			/// <summary>Arc Easing.</summary>
	Sigmoid = 8, 																		/// <summary>Sigmoid Easing.</summary>

	EaseInOut = EaseIn | EaseOut 														/// <summary>Ease-In-Out Easing.</summary>
}

public enum CoordinatesModes 															/// <summary>Coordinates Modes.</summary>
{
	XY, 																				/// <summary>X and Y Coordinate Mode.</summary>
	YX, 																				/// <summary>Y and X Coordinate Mode.</summary>
	XZ, 																				/// <summary>X and Z Coordinate Mode.</summary>
	ZY, 																				/// <summary>Z and Y Coordinate Mode.</summary>
	YZ, 																				/// <summary>Y and Z Coordinate Mode.</summary>
	ZX 																					/// <summary>Z and X Coordinate Mode.</summary>
}

public enum SpeedChange
{
    Acceleration,
    Linear
}

/// <summary>Normalized Property parametric function.</summary>
/// <param name="t">Time, normalized between -1f and 1f.</param>
public delegate float NormalizedPropertyFunctionOC(float t, float x = 0.0f);

public delegate float ParameterizedNormalizedPropertyFunctionOC(float t, float x);

public static class VMath
{
	public const float PI = 3.1415926535897932384626433832795028841971693993751058f; 	/// <summary>Pi's Constant.</summary>
	public const float PHI = 1.61803398874989484820458683436563811772030917980576f; 	/// <summary>Golden Ratio Constant.</summary>
	public const float E = 2.71828182845904523536028747135266249775724709369995f; 		/// <summary>Euler's Number Constant</summary>
	public const float DEG_TO_RAD = PI / 180.0f; 										/// <summary>Degrees to Radians' conversion.</summary>
	public const float RAD_TO_DEG = 180.0f / PI; 										/// <summary>Radians to Degrees' conversion</summary>
	public const float DEGREES_REVOLUTION = 360.0f; 									/// <summary>Degrees that takes a revolution.</summary>
	public const float EPSILON = 0.000001f; 											/// <summary>Custom-defined Epsilon.</summary>

	public static readonly float[] sinTable; 											/// <summary>Sine's Lookup Table.</summary>
	public static readonly float[] cosTable; 											/// <summary>Cosine's Lookup Table.</summary>
	public static readonly float[] tanTable; 											/// <summary>Tangents's Lookup Table.</summary>

	/// <summary>VMath's Static Constructor.</summary>
	static VMath()
	{
		sinTable = new float[181];
		cosTable = new float[181];
		tanTable = new float[181];

		InitializeSinLookupTable();
		InitializeCosLookupTable();
		InitializeTanLookupTable();
	}

#region TrigonometricLookupTables:
	/// <summary>Initializes Sine's Lookup Table.</summary>
	private static void InitializeSinLookupTable()
	{
		sinTable[0] = 0.0f;
		sinTable[1] = 0.0175f; 		sinTable[62] = 0.8829f; 	sinTable[122] = 0.8480f;
		sinTable[2] = 0.0348f; 		sinTable[63] = 0.8910f; 	sinTable[123] = 0.8386f;
		sinTable[3] = 0.0523f; 		sinTable[64] = 0.8987f; 	sinTable[124] = 0.8290f;
		sinTable[4] = 0.0697f; 		sinTable[65] = 0.9063f; 	sinTable[125] = 0.8191f;
		sinTable[5] = 0.0871f; 		sinTable[66] = 0.9135f; 	sinTable[126] = 0.8090f;
		sinTable[6] = 0.1045f; 		sinTable[67] = 0.9205f; 	sinTable[127] = 0.7986f;
		sinTable[7] = 0.1218f; 		sinTable[68] = 0.9271f; 	sinTable[128] = 0.7880f;
		sinTable[8] = 0.1391f; 		sinTable[69] = 0.9335f; 	sinTable[129] = 0.7771f;
		sinTable[9] = 0.1564f; 		sinTable[70] = 0.9396f; 	sinTable[130] = 0.7660f;
		sinTable[10] = 0.1736f; 	sinTable[71] = 0.9455f; 	sinTable[131] = 0.7547f;
		sinTable[11] = 0.1908f; 	sinTable[72] = 0.9510f; 	sinTable[132] = 0.7431f;
		sinTable[12] = 0.2079f; 	sinTable[73] = 0.9563f; 	sinTable[133] = 0.7313f;
		sinTable[13] = 0.2249f; 	sinTable[74] = 0.9612f; 	sinTable[134] = 0.7193f;
		sinTable[14] = 0.2419f; 	sinTable[75] = 0.9659f; 	sinTable[135] = 0.7071f;
		sinTable[15] = 0.2588f; 	sinTable[76] = 0.9702f; 	sinTable[136] = 0.6946f;
		sinTable[16] = 0.2756f; 	sinTable[77] = 0.9743f; 	sinTable[137] = 0.6819f;
		sinTable[17] = 0.2923f; 	sinTable[78] = 0.9781f; 	sinTable[138] = 0.6691f;
		sinTable[18] = 0.3090f; 	sinTable[79] = 0.9816f; 	sinTable[139] = 0.6560f;
		sinTable[19] = 0.3255f; 	sinTable[80] = 0.9848f; 	sinTable[140] = 0.6427f;
		sinTable[20] = 0.3420f; 	sinTable[81] = 0.9876f; 	sinTable[141] = 0.6293f;
		sinTable[21] = 0.3583f; 	sinTable[82] = 0.9902f; 	sinTable[142] = 0.6156f;
		sinTable[22] = 0.3746f; 	sinTable[83] = 0.9925f; 	sinTable[143] = 0.6018f;
		sinTable[23] = 0.3907f; 	sinTable[84] = 0.9945f; 	sinTable[144] = 0.5877f;
		sinTable[24] = 0.4067f; 	sinTable[85] = 0.9961f; 	sinTable[145] = 0.5735f;
		sinTable[25] = 0.4226f; 	sinTable[86] = 0.9975f; 	sinTable[146] = 0.5591f;
		sinTable[26] = 0.4383f; 	sinTable[87] = 0.9986f; 	sinTable[147] = 0.5446f;
		sinTable[27] = 0.4539f; 	sinTable[88] = 0.9993f; 	sinTable[148] = 0.5299f;
		sinTable[28] = 0.4694f; 	sinTable[89] = 0.9998f; 	sinTable[149] = 0.5150f;
		sinTable[29] = 0.4848f; 	sinTable[90] = 1.0f; 		sinTable[150] = 0.5f;
		sinTable[30] = 0.5f;		sinTable[91] = 0.9998f; 	sinTable[151] = 0.4848f;
		sinTable[31] = 0.5150f; 	sinTable[92] = 0.9993f; 	sinTable[152] = 0.4694f;
		sinTable[32] = 0.5299f; 	sinTable[93] = 0.9986f; 	sinTable[153] = 0.4539f;
		sinTable[33] = 0.5446f; 	sinTable[94] = 0.9975f; 	sinTable[154] = 0.4383f;
		sinTable[34] = 0.5591f; 	sinTable[95] = 0.9961f; 	sinTable[155] = 0.4226f;
		sinTable[35] = 0.5735f; 	sinTable[96] = 0.9945f; 	sinTable[156] = 0.4067f;
		sinTable[36] = 0.5877f; 	sinTable[97] = 0.9925f; 	sinTable[157] = 0.3907f;
		sinTable[37] = 0.6018f; 	sinTable[98] = 0.9902f; 	sinTable[158] = 0.3746f;
		sinTable[38] = 0.6156f; 	sinTable[99] = 0.9876f; 	sinTable[159] = 0.3583f;
		sinTable[39] = 0.6293f; 	sinTable[100] = 0.9848f; 	sinTable[160] = 0.3420f;
		sinTable[40] = 0.6427f; 	sinTable[101] = 0.9816f; 	sinTable[161] = 0.3255f;
		sinTable[41] = 0.6560f; 	sinTable[102] = 0.9781f; 	sinTable[162] = 0.3090f;
		sinTable[42] = 0.6691f; 	sinTable[103] = 0.9743f; 	sinTable[163] = 0.2923f;
		sinTable[43] = 0.6819f; 	sinTable[104] = 0.9702f; 	sinTable[164] = 0.2756f;
		sinTable[44] = 0.6946f; 	sinTable[105] = 0.9659f; 	sinTable[165] = 0.2588f;
		sinTable[45] = 0.7071f; 	sinTable[106] = 0.9612f; 	sinTable[166] = 0.2419f;
		sinTable[46] = 0.7193f; 	sinTable[107] = 0.9563f; 	sinTable[167] = 0.2249f;
		sinTable[47] = 0.7313f; 	sinTable[108] = 0.9510f; 	sinTable[168] = 0.2079f;
		sinTable[48] = 0.7431f; 	sinTable[109] = 0.9455f; 	sinTable[169] = 0.1908f;
		sinTable[49] = 0.7547f; 	sinTable[110] = 0.9396f; 	sinTable[170] = 0.1736f;
		sinTable[51] = 0.7660f; 	sinTable[111] = 0.9335f; 	sinTable[171] = 0.1564f;
		sinTable[52] = 0.7880f; 	sinTable[112] = 0.9271f; 	sinTable[172] = 0.1391f;
		sinTable[53] = 0.7986f; 	sinTable[113] = 0.9205f; 	sinTable[173] = 0.1218f;
		sinTable[54] = 0.8090f; 	sinTable[114] = 0.9135f; 	sinTable[174] = 0.1045f;
		sinTable[55] = 0.8191f; 	sinTable[115] = 0.9063f; 	sinTable[175] = 0.0871f;
		sinTable[56] = 0.8290f; 	sinTable[116] = 0.8987f; 	sinTable[176] = 0.0697f;
		sinTable[57] = 0.8386f; 	sinTable[117] = 0.8910f; 	sinTable[177] = 0.0523f;
		sinTable[58] = 0.8480f; 	sinTable[118] = 0.8829f; 	sinTable[178] = 0.0348f;
		sinTable[59] = 0.8571f; 	sinTable[119] = 0.8746f; 	sinTable[179] = 0.0174f;
		sinTable[60] = 0.8660f; 	sinTable[120] = 0.8660f; 	sinTable[180] = 0.0f;
		sinTable[61] = 0.8746f; 	sinTable[121] = 0.8571f; 	
			
			//sinTable[181] = -0.0174f;

	}

	/// <summary>Initializes Cosine's Lookup Table.</summary>
	private static void InitializeCosLookupTable()
	{
		cosTable[0] = 1.0f;
		cosTable[1] = 0.9998f;		cosTable[61] = 0.4848f; 	cosTable[121] = -0.5150f;
		cosTable[2] = 0.9993f;		cosTable[62] = 0.4694f; 	cosTable[122] = -0.5299f;
		cosTable[3] = 0.9986f; 		cosTable[63] = 0.4539f; 	cosTable[123] = -0.5446f;
		cosTable[4] = 0.9975f; 		cosTable[64] = 0.4383f; 	cosTable[124] = -0.5591f;
		cosTable[5] = 0.9961f;		cosTable[65] = 0.4226f; 	cosTable[125] = -0.5735f;
		cosTable[6] = 0.9945f;		cosTable[66] = 0.4067f; 	cosTable[126] = -0.5877f;
		cosTable[7] = 0.9925f;		cosTable[67] = 0.3907f; 	cosTable[127] = -0.6018f;
		cosTable[8] = 0.9902f;		cosTable[68] = 0.3746f; 	cosTable[128] = -0.6156f;
		cosTable[9] = 0.9876f;		cosTable[69] = 0.3583f; 	cosTable[129] = -0.6293f;
		cosTable[10] = 0.9848f; 	cosTable[70] = 0.3420f; 	cosTable[130] = -0.6427f;
		cosTable[11] = 0.9816f; 	cosTable[71] = 0.3255f; 	cosTable[131] = -0.6560f;
		cosTable[12] = 0.9781f; 	cosTable[72] = 0.3090f; 	cosTable[132] = -0.6691f;
		cosTable[13] = 0.9743f; 	cosTable[73] = 0.2923f; 	cosTable[133] = -0.6819f;
		cosTable[14] = 0.9702f; 	cosTable[74] = 0.2756f; 	cosTable[134] = -0.6946f;
		cosTable[15] = 0.9659f; 	cosTable[75] = 0.2588f; 	cosTable[135] = -0.7071f;
		cosTable[16] = 0.9612f; 	cosTable[76] = 0.2419f; 	cosTable[136] = -0.7193f;
		cosTable[17] = 0.9563f; 	cosTable[77] = 0.2249f; 	cosTable[137] = -0.7313f;
		cosTable[18] = 0.9510f; 	cosTable[78] = 0.2079f; 	cosTable[138] = -0.7431f;
		cosTable[19] = 0.9455f; 	cosTable[79] = 0.1908f; 	cosTable[139] = -0.7547f;
		cosTable[20] = 0.9396f; 	cosTable[80] = 0.1736f; 	cosTable[140] = -0.7660f;
		cosTable[21] = 0.9335f; 	cosTable[81] = 0.1564f; 	cosTable[141] = -0.7771f;
		cosTable[22] = 0.9271f; 	cosTable[82] = 0.1391f; 	cosTable[142] = -0.7880f;
		cosTable[23] = 0.9205f; 	cosTable[83] = 0.1218f; 	cosTable[143] = -0.7986f;
		cosTable[24] = 0.9135f; 	cosTable[84] = 0.1045f; 	cosTable[144] = -0.8090f;
		cosTable[25] = 0.9063f; 	cosTable[85] = 0.0871f; 	cosTable[145] = -0.8191f;
		cosTable[26] = 0.8987f; 	cosTable[86] = 0.0697f; 	cosTable[146] = -0.8290f;
		cosTable[27] = 0.8910f; 	cosTable[87] = 0.0523f; 	cosTable[147] = -0.8386f;
		cosTable[28] = 0.8829f; 	cosTable[88] = 0.0348f; 	cosTable[148] = -0.8480f;
		cosTable[29] = 0.8746f; 	cosTable[89] = 0.0174f; 	cosTable[149] = -0.8571f;
		cosTable[30] = 0.8660f; 	cosTable[90] = 0.0f; 		cosTable[150] = -0.8660f;
		cosTable[31] = 0.8571f; 	cosTable[91] = -0.0174f; 	cosTable[151] = -0.8746f;
		cosTable[32] = 0.8480f; 	cosTable[92] = -0.0348f; 	cosTable[152] = -0.8829f;
		cosTable[33] = 0.8386f; 	cosTable[93] = -0.0523f; 	cosTable[153] = -0.8910f;
		cosTable[34] = 0.8290f; 	cosTable[94] = -0.0697f; 	cosTable[154] = -0.8987f;
		cosTable[35] = 0.8191f; 	cosTable[95] = -0.08715f; 	cosTable[155] = -0.9063f;
		cosTable[36] = 0.8090f; 	cosTable[96] = -0.1045f; 	cosTable[156] = -0.9135f;
		cosTable[37] = 0.7986f; 	cosTable[97] = -0.1218f; 	cosTable[157] = -0.9205f;
		cosTable[38] = 0.7880f; 	cosTable[98] = -0.1391f; 	cosTable[158] = -0.9271f;
		cosTable[39] = 0.7771f; 	cosTable[99] = -0.1564f; 	cosTable[159] = -0.9335f;
		cosTable[40] = 0.7660f; 	cosTable[100] = -0.1736f; 	cosTable[160] = -0.9396f;
		cosTable[41] = 0.7547f; 	cosTable[101] = -0.1908f; 	cosTable[161] = -0.9455f;
		cosTable[42] = 0.7431f; 	cosTable[102] = -0.2079f; 	cosTable[162] = -0.9510f;
		cosTable[43] = 0.7313f; 	cosTable[103] = -0.2249f; 	cosTable[163] = -0.9563f;
		cosTable[44] = 0.7193f; 	cosTable[104] = -0.2419f; 	cosTable[164] = -0.9612f;
		cosTable[45] = 0.7071f; 	cosTable[105] = -0.2588f; 	cosTable[165] = -0.9659f;
		cosTable[46] = 0.6946f; 	cosTable[106] = -0.2756f; 	cosTable[166] = -0.9702f;
		cosTable[47] = 0.6819f; 	cosTable[107] = -0.2923f; 	cosTable[167] = -0.9743f;
		cosTable[48] = 0.6691f; 	cosTable[108] = -0.3090f; 	cosTable[168] = -0.9781f;
		cosTable[49] = 0.6560f; 	cosTable[109] = -0.3255f; 	cosTable[169] = -0.9816f;
		cosTable[50] = 0.6427f; 	cosTable[110] = -0.3420f; 	cosTable[170] = -0.9848f;
		cosTable[51] = 0.6293f; 	cosTable[111] = -0.3583f; 	cosTable[171] = -0.9876f;
		cosTable[52] = 0.6156f; 	cosTable[112] = -0.3746f; 	cosTable[172] = -0.9902f;
		cosTable[53] = 0.6018f; 	cosTable[113] = -0.3907f; 	cosTable[173] = -0.9925f;
		cosTable[54] = 0.5877f; 	cosTable[114] = -0.4067f; 	cosTable[174] = -0.9945f;
		cosTable[55] = 0.5735f; 	cosTable[115] = -0.4226f; 	cosTable[175] = -0.9961f;
		cosTable[56] = 0.5591f; 	cosTable[116] = -0.4383f; 	cosTable[176] = -0.9975f;
		cosTable[57] = 0.5446f; 	cosTable[117] = -0.4539f; 	cosTable[177] = -0.9986f;
		cosTable[58] = 0.5299f; 	cosTable[118] = -0.4694f; 	cosTable[178] = -0.9993f;
		cosTable[59] = 0.5150f; 	cosTable[119] = -0.4848f; 	cosTable[179] = -0.9998f;
		cosTable[60] = 0.5f; 		cosTable[120] = -0.5f;		cosTable[180] = -1.0f;
																//cosTable[181] = -0.9998f;
	}

	/// <summary>Initializes Tangent's Lookup Table.</summary>
	private static void InitializeTanLookupTable()
	{
		tanTable[0] = 0.0f;
		tanTable[1] = 0.0174f;		tanTable[61] = 1.8040f; 							tanTable[121] = -1.6642f;
		tanTable[2] = 0.0349f;		tanTable[62] = 1.8807f; 							tanTable[122] = -1.6003f;
		tanTable[3] = 0.0524f;		tanTable[63] = 1.9626f; 							tanTable[123] = -1.5398f;
		tanTable[4] = 0.0699f;		tanTable[64] = 2.0503f; 							tanTable[124] = -1.4825f;
		tanTable[5] = 0.0874f;		tanTable[65] = 2.1445f; 							tanTable[125] = -1.4281f;
		tanTable[6] = 0.1051f;		tanTable[66] = 2.2460f; 							tanTable[126] = -1.3763f;
		tanTable[7] = 0.1227f;		tanTable[67] = 2.3558f; 							tanTable[127] = -1.3270f;
		tanTable[8] = 0.1405f;		tanTable[68] = 2.4750f; 							tanTable[128] = -1.2799f;
		tanTable[9] = 0.1583f;		tanTable[69] = 2.6050f; 							tanTable[129] = -1.2348f;
		tanTable[10] = 0.1763f; 	tanTable[70] = 2.7474f; 							tanTable[130] = -1.1917f;
		tanTable[11] = 0.1943f; 	tanTable[71] = 2.9042f; 							tanTable[131] = -1.1503f;
		tanTable[12] = 0.2125f; 	tanTable[72] = 3.0776f; 							tanTable[132] = -1.1106f;
		tanTable[13] = 0.2308f; 	tanTable[73] = 3.2708f; 							tanTable[133] = -1.0723f;
		tanTable[14] = 0.2493f; 	tanTable[74] = 3.4874f; 							tanTable[134] = -1.0355f;
		tanTable[15] = 0.2679f; 	tanTable[75] = 3.7320f; 							tanTable[135] = -1.0f;
		tanTable[16] = 0.2867f; 	tanTable[76] = 4.0107f; 							tanTable[136] = -0.9656f;
		tanTable[17] = 0.3057f; 	tanTable[77] = 4.3314f; 							tanTable[137] = -0.9325f;
		tanTable[18] = 0.3249f; 	tanTable[78] = 4.7046f; 							tanTable[138] = -0.9004f;
		tanTable[19] = 0.3443f; 	tanTable[79] = 5.1445f; 							tanTable[139] = -0.8692f;
		tanTable[20] = 0.3639f; 	tanTable[80] = 5.6712f; 							tanTable[140] = -0.8391f;
		tanTable[21] = 0.3838f; 	tanTable[81] = 6.3137f; 							tanTable[141] = -0.8097f;
		tanTable[22] = 0.4040f; 	tanTable[82] = 7.1153f; 							tanTable[142] = -0.7812f;
		tanTable[23] = 0.4244f; 	tanTable[83] = 8.1443f; 							tanTable[143] = -0.7535f;
		tanTable[24] = 0.4452f; 	tanTable[84] = 9.5143f; 							tanTable[144] = -0.7265f;
		tanTable[25] = 0.4663f; 	tanTable[85] = 11.4300f; 							tanTable[145] = -0.7002f;
		tanTable[26] = 0.4877f; 	tanTable[86] = 14.3006f; 							tanTable[146] = -0.6745f;
		tanTable[27] = 0.5095f; 	tanTable[87] = 19.0811f; 							tanTable[147] = -0.6494f;
		tanTable[28] = 0.5317f; 	tanTable[88] = 28.6362f; 							tanTable[148] = -0.6248f;
		tanTable[29] = 0.5543f; 	tanTable[89] = 57.2899f; 							tanTable[149] = -0.6008f;
		tanTable[30] = 0.5773f; 	tanTable[90] = Mathf.Tan(90 * Mathf.Deg2Rad); 		tanTable[150] = -0.5773f;
		tanTable[31] = 0.6008f; 	tanTable[91] = -57.2899f; 							tanTable[151] = -0.5543f;
		tanTable[32] = 0.6248f; 	tanTable[92] = -28.6362f; 							tanTable[152] = -0.5317f;
		tanTable[33] = 0.6494f; 	tanTable[93] = -19.0811f; 							tanTable[153] = -0.5095f;
		tanTable[34] = 0.6745f; 	tanTable[94] = -14.3006f; 							tanTable[154] = -0.4877f;
		tanTable[35] = 0.7002f; 	tanTable[95] = -11.4300f; 							tanTable[155] = -0.4663f;
		tanTable[36] = 0.7265f; 	tanTable[96] = -9.5143f; 							tanTable[156] = -0.4552f;
		tanTable[37] = 0.7535f; 	tanTable[97] = -8.1443f; 							tanTable[157] = -0.4244f;
		tanTable[38] = 0.7812f; 	tanTable[98] = -7.1153f; 							tanTable[158] = -0.4040f;
		tanTable[39] = 0.8097f; 	tanTable[99] = -6.3137f; 							tanTable[159] = -0.3838f;
		tanTable[40] = 0.8391f; 	tanTable[100] = -5.6712f; 							tanTable[160] = -0.3639f;
		tanTable[41] = 0.8692f; 	tanTable[101] = -5.1445f; 							tanTable[161] = -0.3443f;
		tanTable[42] = 0.9004f; 	tanTable[102] = -4.7046f; 							tanTable[162] = -0.3249f;
		tanTable[43] = 0.9325f; 	tanTable[103] = -4.3314f; 							tanTable[163] = -0.3057f;
		tanTable[44] = 0.9656f; 	tanTable[104] = -4.0107f; 							tanTable[164] = -0.2867f;
		tanTable[45] = 1.0f; 		tanTable[105] = -3.7320f; 							tanTable[165] = -0.2679f;
		tanTable[46] = 1.0355f; 	tanTable[106] = -3.4874f; 							tanTable[166] = -0.2493f;
		tanTable[47] = 1.0723f; 	tanTable[107] = -3.2708f; 							tanTable[167] = -0.2308f;
		tanTable[48] = 1.1106f; 	tanTable[108] = -3.0776f; 							tanTable[168] = -0.2125f;
		tanTable[49] = 1.1503f; 	tanTable[109] = -2.9042f; 							tanTable[169] = -0.1943f;
		tanTable[50] = 1.1917f; 	tanTable[110] = -2.7474f; 							tanTable[170] = -0.1763f;
		tanTable[51] = 1.2348f; 	tanTable[111] = -2.6050f; 							tanTable[171] = -0.1583f;
		tanTable[52] = 1.2799f; 	tanTable[112] = -2.4750f; 							tanTable[172] = -0.1405f;
		tanTable[53] = 1.3270f; 	tanTable[113] = -2.3558f; 							tanTable[173] = -0.1227f;
		tanTable[54] = 1.3763f; 	tanTable[114] = -2.2460f; 							tanTable[174] = -0.1051f;
		tanTable[55] = 1.4281f; 	tanTable[115] = -2.1445f; 							tanTable[175] = -0.0874f;
		tanTable[56] = 1.4825f; 	tanTable[116] = -2.0503f; 							tanTable[176] = -0.0699f;
		tanTable[57] = 1.5398f; 	tanTable[117] = -1.9626f; 							tanTable[177] = -0.0524f;
		tanTable[58] = 1.6003f; 	tanTable[118] = -1.8807f; 							tanTable[178] = -0.0349f;
		tanTable[59] = 1.6642f; 	tanTable[119] = -1.8040f; 							tanTable[179] = -0.0174f;
		tanTable[60] = 1.7320f; 	tanTable[120] = -1.7320f; 							tanTable[180] = 0.0f;
																						//tanTable[181] = 0.0174f;
	}


#endregion

/*---------------
| Easings: 		|
---------------*/
#region Easings:
	/// <returns>Ease-In Sine for Normalized Time t.</returns>
	public static float EaseInSine(float t)
	{
		return 1.0f - Mathf.Cos(t * PI * 0.5f);
	}

	/// <returns>Ease-In Quad for Normalized Time t.</returns>
	public static float EaseInQuad(float t)
	{
		return t * t;
	}

	/// <returns>Ease-In Cubic for Normalized Time t.</returns>
	public static float EaseInCubic(float t)
	{
		return t * t * t;
	}

	/// <returns>Ease-In Back for Normalized Time t.</returns>
	public static float EaseInBack(float t)
	{
		float c1 = 1.70158f;
		float c3 = c1 + 1.0f;

		return c3 * t * t * t - c1 * t * t;
	}

	/// <returns>Ease-Out Sine for Normalized Time t.</returns>
	public static float EaseOutSine(float t)
	{
		return Mathf.Sin(t * Mathf.PI * 0.5f);
	}

	/// <summary>Ease-Out Cubic for Normalized Time t.</summary>
	public static float EaseOutCubic(float t)
	{
		return 1.0f - Mathf.Pow(1.0f - t, 3.0f);
	}

	/// <returns>Ease-Out Bounce for Normalized Time t.</returns>
	public static float EaseOutBounce(float t)
	{
		float n1 = 7.5625f;
		float d1 = 2.75f;

		if(t < (1.0f / d1)) 		return n1 * t * t;
		else if(t < (2 / d1)) 		return n1 * (t -= 1.5f / d1) * t + 0.75f;
		else if(t < (2.5f / d1)) 	return n1 * (t -= 2.25f / d1) * t + 0.9375f;
		else 						return n1 * (t -= 2.625f / d1) * t + 0.984375f;
	}
#endregion

	/// <summary>Turns value into boolean representation [if above 0, it is considered true].</summary>
	public static bool ToBoolean(int x) { return x > 0; }

#region NormalizedPropertyFunctionOCs:
	/*public static float Interpolate(float a, float b, float t)
	{
		return a + (_t * (b - a));
	}*/

	public static NormalizedPropertyFunctionOC GetEasing(Easings _easing)
	{
		switch(_easing)
		{
			case Easings.Linear: 	return null;
			case Easings.EaseIn: 	return EaseIn;
			case Easings.EaseOut: 	return EaseOut;
			case Easings.Arc: 		return Arc;
			case Easings.Sigmoid: 	return Sigmoid;
		}

		return null;
	}

	/// <summary>Calculates the SoftMax function.</summary>
	/// <param name="i">Index from the set to calculate.</param>
	/// <param name="X">Set of values.</param>	
	/// <returns>Softmax function for value on index i.</returns>
	public static float SoftMax(int i, params float[] X)
	{
		float x = 0.0f;
		float sum = 0.0f;

		for(int j = 0; j < X.Length; j++)
		{
			float e = Mathf.Exp(X[j]);
			sum += e;
			if(i == j) x = e; 
		}

		return x / sum;
	}

	/// <summary>Calculates a SoftMax set given a set of values.</summary>
	/// <param name="z">Set of values Z.</param>
	/// <returns>Set of SoftMaxed values.</returns>
	public static float[] SoftMax(params float[] z)
	{
		if(z == null || z.Length == 0) return null;

		int l = z.Length;
		float[] s = new float[l];
		float sum = 0.0f;

		for(int i = 0; i < l; i++)
		{
			s[i] = Mathf.Exp(z[i]);
			sum += s[i];
		}

		for(int i = 0; i < l; i++)
		{
			s[i] = s[i] / sum;
		}

		return s;
	}

	/// <param name="t">Normalized Parameter T.</param>
	/// <returns>T as it is.</returns>
	public static float DefaultNormalizedPropertyFunction(float t) { return t; }

	/// <summary>Calculates a number to a given exponential.</summary>
	/// <param name="t">Number to elevate to given exponent.</param>
	/// <param name="_exponential">Exponential to raise number to [2 by default].</param>
	/// <returns>Number raised to given exponential.</returns>
	public static float EaseIn(float t, float _exponential = 2.0f)
	{
		return (1.0f - Mathf.Abs(EaseOut(t - 1.0f, _exponential)));
	}

	/// <summary>Calculates a number to a given exponential.</summary>
	/// <param name="t">Number to elevate to given exponent.</param>
	/// <param name="_exponential">Exponential to raise number to [2 by default].</param>
	/// <returns>Number raised to the inverse of the given exponential.</returns>
	public static float EaseOut(float t, float _exponential = 2.0f)
	{
		if(_exponential == 0.0f) return 1.0f;
		else if(_exponential == 1.0f) return t;
		else return Power(t, _exponential);
	}

	public static float EaseInEaseOut(float t, float _exponential = 2.0f)
	{
		return t < 0.5f ? EaseIn(t, _exponential) : EaseOut(t, _exponential);
	}

	public static float EaseInEaseOut(float t, float _easeInExponential = 2.0f, float _easeOutExponential = 2.0f)
	{
		return Blend(EaseIn(t, _easeInExponential), EaseOut(t, _easeOutExponential), t);
	}

	public static float Blend(float a, float b, float weightB)
	{
		return a + (weightB * (b - a));
		/*
		X^2.2 = Blend(t*t, t*t*t, 0.2);
		*/
	}

	/// <summary>Raises a number to a given power.</summary>
	/// <param name="x">Number to raise.</param>
	/// <param name="p">Power to raise the number to.</param>
	/// <returns>Number raised to power.</returns>
	public static float PowerInteger(float x, int p)
	{
		if(p == 0) return x != 0.0f ? 1.0f : 0.0f;
		else if(p == 1.0f) return x;

		for(int i = 0; i < p - 1; i++)
		{
			x *= x;
		}

		return p > 1 ? x : (1.0f / x);
	}

	/// <summary>Raises a number to a given power.</summary>
	/// <param name="x">Number to raise.</param>
	/// <param name="p">Power to raise the number to.</param>
	/// <returns>Number raised to power.</returns>
	public static float Power(float x, float p)
	{
		if(p == 0.0f) return x != 0.0f ? 1.0f : 0.0f;
		else if(p == 1.0f) return x;

		int floorPower = (int)(p);
		float difference = p - floorPower;
		float result = PowerInteger(x, floorPower);

		return (difference > 0.0f) ? Blend(result, result * (p > 1 ? x : (1.0f / x)), difference) : result;
	}

	/// <summary>Scales a t function to t.</summary>
	/// <param name="function">Function evaluating t.</param>
	/// <param name="t">Normalized Time t.</param>
	/// <returns>Evaluated t scaled to t.</returns>
	public static float Scale(Func<float, float> function, float t)
	{
		return t * function(t);
	}

	/// <summary>Inverts a scaled t function.</summary>
	/// <param name="function">Function evaluating t.</param>
	/// <param name="t">Normalized Time t.</param>
	/// <returns>Function scaled to t inverted.</returns>
	public static float ReverseScale(Func<float, float> function, float t)
	{
		return  (1.0f - t) * function(t);
	}

	/// <summary>Calculates position of Arc of a given t.</summary>
	/// <param name="t">Current time.</param>
	/// <returns>Time relative to t value.</returns>
	public static float Arc(float t, float _x = 0.0f)
	{
		return (t * (1.0f - t));
	}

	/// <summary>Interpolates linearly an initial value to a final value, on a normalized time, following the formula P = P0 + t(Pf - P0).</summary>
	/// <param name="a">Initial value [P0].</param>
	/// <param name="b">Destiny value [Pf].</param>
	/// <param name="t">Normalized t, clamped internally between -1 and 1.</param>
	/// <returns>Interpolated value on given normalized time.</returns>
	public static float Lerp(float a, float b, float t)
	{
		t = Mathf.Clamp(t, -1.0f, 1.0f);

		return a + (t * (b - a)); 
	}

	/// <summary>Evaluates sigmoid function by given x.</summary>
	/// <param name="x">Number to evaluate.</param>
	/// <param name="e">Exponential, 'e' constant by default.</param>
	/// <returns>Sigmoid evaluation.</returns>
	public static float Sigmoid(float x, float e = E)
	{
		return (1.0f / (1.0f + (1.0f / Mathf.Pow(e, x))));
	}

	/// <summary>Evaluates sigmoid function by given x.</summary>
	/// <param name="x">Number to evaluate.</param>
	/// <returns>Sigmoid evaluation.</returns>
	public static float Sigmoid(float x)
	{
		return (1.0f / (1.0f + (1.0f / Mathf.Exp(-x))));
	}

	public static float FunctionsSumatories(float _t, params NormalizedPropertyFunctionOC[] _functions)
	{
		float proportion = (1 / _functions.Length);

		for(int i = 0; i < (_functions.Length - 1); i++)
		{
			if((_t >= (proportion * i)) && (_t < (proportion * (i + 1)))) return _functions[i](_t);
		}

		return _t;
	}
#endregion

	/// <summary>Controlled Random function [you pass the seed].</summary>
	/// <param name="min">Min Random Value.</param>
	/// <param name="max">Max Random Value.</param>
	/// <param name="seed">Custom seed [it does not change with time].</param>
	public static float Rand(float min, float max, float seed)
	{
		return min + (seed % (max - min));
	}

	/// <summary>Linear Congruential Generator Random [you pass the seed and the previous state].</summary>
	/// <param name="min">Min Random Value.</param>
	/// <param name="max">Max Random Value.</param>
	/// <param name="seed">Custom seed [it does not change with time].</param>
	/// <param name="state">Previous Random value.</param>
	public static float Rand(float min, float max, float seed, ref float state)
	{
		/*
		x[t+1] = (a * x[t] + b) % m

		where:

			- x[t]: Previous state
			- a and b: Amplitude values
			- m: Output reduction
		*/
		return state = min + ((state + seed) % (max - min));
	}

	/// <summary>PID [Proportional-Integral-Derivative] Loop Function.</summary>
	/// <param name="x">Current Value.</param>
	/// <param name="y">Target Value.</param>
	/// <param name="i">Integral's reference.</param>
	/// <param name="e">Error's reference.</param>
	/// <param name="pF">Proportional's Factor.</param>
	/// <param name="iF">Integral's Factor.</param>
	/// <param name="dF">Derivative's Factor.</param>
	/// <param name="t">Time frame.</param>
	/// <returns>Value through a PID loop.</returns>
	public static float PID(float x, float y, ref float i, ref float e, float pF, float iF, float dF, float t)
	{
		float d = y - x;
		i += d * t;
		float dx = (d - e) / t;
		e = d;

		return d * pF + i * iF + dx * dF;
	}

#region TrigonometricFunctions:
	/// https://stackoverflow.com/questions/38917692/sin-cos-funcs-without-math-h

	/// sin(x) = x - (x^3)/3! + (x^5)/5! - (x^7)/7! .......
	/*public static float Sin(float d)
	{
		d *= DEG_TO_RAD;
		float x = d;

		for(float i = 1.0f; i < 7.0f; i++)
		{
			float di = (1 * 2.0f);
			float c = Power(x, di + 1.0f);

			c /= Factorial(di);
			i += ((int)(c) & 0x01) ? -c : c;
		}

		return x;
	}*/

	// Calculate the Sin of an angle and returns the value in Radians
	public static float SinInterpolation(float _x)
    {
			int i = (int)_x;
			float sin = sinTable[i];
			float difference = _x - Mathf.Floor(_x);

			if(difference >0.0f)
            {
				float sin2 = sinTable[i + 1];
				sin = Mathf.Lerp(sin, sin2, difference);
            }
			return sin;
    }
		public static float SinTaylorRads(float x)
        {
			x *= DEG_TO_RAD;
			float rad = x;
			float sin = 0;

            for (int i = 0; i < 10; i++)
            {
				float dividend = 1f;
				float divisor = 1f;
				float sign = 0;
				float c = 2 * i + 1;

				dividend = Power(rad, c);
				divisor = Factorial(c);
				sign = Power(-1, i);
				sin += (dividend / divisor)*sign ;
            }

			return sin;
        }

		// Calculate the Cos of an angle and returns the value in Radians
		public static float CosInterpolation (float _x)
        {
			int i = (int)_x;
			float cos = cosTable[i];
			float difference = _x - Mathf.Floor(_x);

			if (difference > 0.0f)
			{
				float cos2 = cosTable[i + 1];
				cos = Mathf.Lerp(cos, cos2, difference);
			}
			return cos;
		}

		// Calculate the Tan of an angle and returns the value in Radians
		public static float TanInterpolation(float _x)
		{
			int i = (int)_x;
			float tan = tanTable[i];
			float difference = _x - Mathf.Floor(_x);

			if (difference > 0.0f)
			{
				float tan2 = tanTable[i + 1];
				tan = Mathf.Lerp(tan, tan2, difference);
			}
			return tan;
		}


		/*/// cos(x) = 1 - (x^2)/2! + (x^4)/4! - (x^6)/6! .......
		public static float Cos(float d)
		{
			d *= DEG_TO_RAD;
			float x = 1.0f;

			for(float i = 1.0f; i < 7.0f; i++)
			{
				float di = (1 * 2.0f);
				float c = Power(x, di);

				c /= Factorial(di - 1.0f);
				i += ((int)(c) & 0x01) ? -c : c;
			}

			return x;
		}*/
		#endregion

		/// <summary>Calculates the spatial relationship of 2 given segments.</summary>
		/// <param name="aMin">Segment A's minimum value.</param>
		/// <param name="aMax">Segment A's maximum value.</param>
		/// <param name="bMin">Segment B's minimum value.</param>
		/// <param name="bMax">Segment B's maximum value.</param>
		/// <returns>Spatial relationship of 2 segments A and B.</returns>
		public static SpatialRelationship Get1DSpatialRelationship(float aMin, float aMax, float bMin, float bMax)
	{
		if(bMin > aMin && bMax < aMax) return SpatialRelationship.AContainsB;
		if(aMin > bMin && aMax < bMax) return SpatialRelationship.BContainsA;
		if((aMin <= bMin && aMax <= bMax) || (bMin <= aMin && bMax <= aMax)) return SpatialRelationship.Intersection;

		float minAMaxBMax = Mathf.Min(aMax, bMax);
		float maxAMinBMin = Mathf.Max(aMin, bMin);

		if(minAMaxBMax < maxAMinBMin) return SpatialRelationship.NonIntersection;
		if(minAMaxBMax == maxAMinBMin) return SpatialRelationship.Contact;

		return SpatialRelationship.Undefined;
	}

	/// \TODO Finish function:
	public static SpatialRelationship Get2DSpatialRelationship(Vector2 aMin, Vector2 aMax, Vector2 bMin, Vector2 bMax)
	{
		SpatialRelationship x = Get1DSpatialRelationship(aMin.x, aMax.x, bMin.x, bMax.x);
		SpatialRelationship y = Get1DSpatialRelationship(aMin.y, aMax.y, bMin.y, bMax.y);
		Vector2 xA = new Vector2(aMin.x, aMax.y);
		Vector2 xB = new Vector2(aMax.x, aMin.y);
		Vector2 yA = new Vector2(bMin.x, bMax.y);
		Vector2 yB = new Vector2(bMax.x, bMin.y);

		/// A Contains B:
		/*if(xA.x < 
		)*/

		if(x == SpatialRelationship.Intersection && y == SpatialRelationship.Intersection) return SpatialRelationship.Intersection;
		if(x == SpatialRelationship.AContainsB && y == SpatialRelationship.AContainsB) return SpatialRelationship.AContainsB;
		if(x == SpatialRelationship.BContainsA && y == SpatialRelationship.BContainsA) return SpatialRelationship.BContainsA;

		/// Shouldn't reach here:
		return SpatialRelationship.Undefined;
	}

	/// <summary>Gets size of segment that fits two given segments.</summary>
	/// <param name="aMin">Segment A's minimum value.</param>
	/// <param name="aMax">Segment A's maximum value.</param>
	/// <param name="bMin">Segment B's minimum value.</param>
	/// <param name="bMax">Segment B's maximum value.</param>
	/// <returns>Size of segment that fits 2 segments A and B.</returns>
	public static float GetSizeToFitSegments(float aMin, float aMax, float bMin, float bMax)
	{
		float min = Mathf.Min(aMin, bMin);
		float max = Mathf.Max(aMax, bMax);

		return max - min;

		/* All of this was unnecesary, though I'll keep it since it was a fun exercise to 
		leave in vain...

		Nonetheless, both methods reach the same results...*/

		SpatialRelationship relationship = Get1DSpatialRelationship(aMin, aMax, bMin, bMax);
		float sizeA = aMax - aMin;
		float sizeB = bMax - bMin;

		switch(relationship)
		{
			case SpatialRelationship.AContainsB:
			case SpatialRelationship.BContainsA:
			return Mathf.Max(sizeA, sizeB);

			case SpatialRelationship.Intersection:
			return (sizeA + sizeB) - (Mathf.Min(aMax, bMax) - Mathf.Max(aMin, bMin));

			case SpatialRelationship.NonIntersection:
			return (sizeA + sizeB) + (Mathf.Max(aMin, bMin) - Mathf.Min(aMax, bMax));

			case SpatialRelationship.Contact:
			return sizeA + sizeB;

			default:
			return 0.0f;
		}
	}

	/// <summary>Gets size of segment that fits two given segments [both with minimum value at '0'].</summary>
	/// <param name="sizeA">Size of Segment A.</param>
	/// <param name="sizeB">Size of Segment B.</param>
	/// <returns>Size of segment that fits 2 segments A and B.</returns>
	public static float GetSizeToFitSegments(float sizeA, float sizeB)
	{
		return GetSizeToFitSegments(0.0f, sizeA, 0.0f, sizeB);
	}

	/// <summary>Calculates segment [represented as Float Range] that better fits 2 given segments A and B.</summary>
	/// <param name="aMin">Segment A's minimum value.</param>
	/// <param name="aMax">Segment A's maximum value.</param>
	/// <param name="bMin">Segment B's minimum value.</param>
	/// <param name="bMax">Segment B's maximum value.</param>
	/// <returns>Segment that fits segments A and B [as FloatRange].</returns>
	public static FloatRange GetSegmentThatFitsPair(float aMin, float aMax, float bMin, float bMax)
	{
		return new FloatRange(Mathf.Min(aMin, bMin), Mathf.Max(aMax, bMax));
	}

	/// <summary>Calculates AABBs that fit into a pair of Bounds.</summary>
	/// <param name="a">A's Bounds.</param>
	/// <param name="b">B's Bounds.</param>
	/// <returns>Bounds' that fits into pair of bounds.</returns>
	public static Bounds GetBoundsToFitPair(Bounds a, Bounds b)
	{
		Vector3 center = new Vector3(
			GetMedian(Mathf.Min(a.min.x, b.min.x), Mathf.Max(a.max.x, b.max.x)),
			GetMedian(Mathf.Min(a.min.y, b.min.y), Mathf.Max(a.max.y, b.max.y)),
			GetMedian(Mathf.Min(a.min.z, b.min.z), Mathf.Max(a.max.z, b.max.z))
		);
		Vector3 size = new Vector3(
			VMath.GetSizeToFitSegments(a.min.x, a.max.x, b.min.x, b.max.x),
			VMath.GetSizeToFitSegments(a.min.y, a.max.y, b.min.y, b.max.y),
			VMath.GetSizeToFitSegments(a.min.z, a.max.z, b.min.z, b.max.z)
		);

		return new Bounds(center, size);
	}

	/// <summary>Calculates the Rule of 3 given three values A, B and C (A -> C; B -> X).</summary>
	/// <param name="a">Value A (Extreme).</param>
	/// <param name="b">Value B (Mean).</param>
	/// <param name="c">Value C (Mean).</param>
	/// <returns>Rule of 3 between given values.</returns>
	public static float RuleOf3(float a, float b, float c)
	{
		return (b * c) / a;
	}

	/// \TODO The formula fails when calculating the quotient, since it is multiplicating the divisor's inverse.
	/// <summary>Calculates the Modulo of X and Y, given that Y is a multiplicative inverse.</summary>
	/// <param name="x">Dividend value.</param>
	/// <param name="y">Multiplicative inverse of a divisor.</param>
	/// <returns>Modulo of X mod Y.</returns>
	public static float InverseDivisorMod(float x, float y)
	{
		float q = Mathf.Floor(x * y);
		return x - (q * y);
	}

	/// <summary>Calculates the Logarithm of an odd function.</summary>
	/// <param name="x">Variable to calculate the Logot.</param>
	/// <param name="b">Base [2 by default].</param>
	/// <returns>Logot function.</returns>
	public static float Logot(float x, float b = 2.0f)
	{
		return Log((x / (1.0f - x)), b);
	}

	/// <summary>Calculates the logarithm of a given number.</summary>
	/// <param name="x">Number to calculate logarithm.</param>
	/// <param name="b">Logarithm's base [2 by default].</param>
	/// <returns>Logarithm of number.</returns>
	public static float Log(float x, float b = 2.0f)
	{
		float count = 0.0f;

		while(x > (b - 1.0f))
		{
			x /= b;
			count++;
		}

		return count;
	}

	/// \TODO Re-work all 360 degree-related functions
	/// <summary>Counts how many revolutions the given degree has made.</summary>
	/// <param name="d">Degrees.</param>
	/// <returns>Revolutions made by given degree.</returns>
	public static float GetRevolutions(this float d)
	{
		return d < DEGREES_REVOLUTION ? 0.0f : Mathf.Abs(Mathf.Floor(d / DEGREES_REVOLUTION));
	}

	public static float To360(this float d)
	{
		return d > DEGREES_REVOLUTION ? d % DEGREES_REVOLUTION : d;
	}

	/// <summary>Converts negative degrees [clockwise] into positive degrees [counter-clockwise].</summary>
	/// <param name="d">Negative degrees.</param>
	/// <returns>Negative degrees into positive degrees.</returns>
	public static float ToPositiveDegrees(this float d)
	{
		return d < 0.0f ? DEGREES_REVOLUTION - d : d;
	}

	/// <summary>Converts positive degrees [counter-clockwise] into negative degrees [clockwise].</summary>
	/// <param name="d">Positive degrees.</param>
	/// <returns>Positive degrees into negative degrees.</returns>
	public static float ToNegativeDegrees(this float d)
	{
		return d < 0.0f ? d : d - DEGREES_REVOLUTION;
	}

#region Vector3Utilities:
	/// <summary>Calculates angle between 2 vectors around given axis.</summary>
	/// <param name="a">Vector A.</param>
	/// <param name="b">Vector B.</param>
	/// <param name="axis">Axis.</param>
	/// <returns>Signed angle around given axis.</returns>
	public static float AnlgeAroundAxis(Vector3 a, Vector3 b, Vector3 axis)
	{
		a = a - Vector3.Project(a, axis);
		b = b - Vector3.Project(b, axis);

		Vector3 cross = Vector3.Cross(a, b);
		float angle = Vector3.Angle(a, b);

		return angle * (Vector3.Dot(axis, cross) > 0.0f ? -1.0f : 1.0f);
	}

	/// <summary>Interpolates linearly an initial Vector3 to a final Vector3, on a normalized time, following the formula P = P0 + t(Pf - P0).</summary>
	/// <param name="a">Initial Vector3 [P0].</param>
	/// <param name="b">Destiny Vector3 [Pf].</param>
	/// <param name="t">Normalized t, clamped internally between -1 and 1.</param>
	/// <returns>Interpolated Vector3 on given normalized time.</returns>
	public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
	{
		t = Mathf.Clamp(t, -1.0f, 1.0f);

		return a + (t * (b - a)); 
	}

	public static Vector3 SoomthStartN(Vector3 a, Vector3 b, int _exponential, float t)
	{
		t = Mathf.Clamp(t, -1.0f, 1.0f);

		return a + (Mathf.Pow(t, _exponential) * (b - a));
	}

	public static Vector3 SoomthEndN(Vector3 a, Vector3 b, int _exponential, float t)
	{
		t = Mathf.Clamp(t, -1.0f, 1.0f);

		return a + ((1f - (1f - Mathf.Pow(t, _exponential))) * (b - a));
	}

	/// <summary>Calculates a Linear Beizer Curve point relative to the time, following the formula [B(t) = (1-t)P0 + tPf].</summary>
	/// <param name="a">Initial value [P0].</param>
	/// <param name="b">Destiny value [Pf].</param>
	/// <param name="t">Normalized t, clamped internally between -1 and 1.</param>
	/// <returns>Linear Beizer Curve point relative to given normalized time.</returns>
	public static Vector3 LinearBeizer(Vector3 a, Vector3 b, float t)
	{
		t = Mathf.Clamp(t, -1.0f, 1.0f);

		return ((1.0f - t) * a) + (t * b);
	}

	/// \TODO Clean the following Beizer Curve functions to a formula that doesn't call Linear Beizer n times.
	/// <summary>Calculates a Cuadratic Beizer Curve point relative to the time, following the formula [B(P0,P1,P2,t) = (1-t)B(P0,P1,t) + tB(P1,P2,t)].</summary>
	/// <param name="a">Initial value [P0].</param>
	/// <param name="b">Destiny value [Pf].</param>
	/// <param name="x">Tanget vector between initialPoint and finalPoint [P1].</param>
	/// <param name="t">Normalized t, clamped internally between -1 and 1.</param>
	/// <returns>Cuadratic Beizer Curve point relative to given normalized time.</returns>
	public static Vector3 CuadraticBeizer(Vector3 a, Vector3 b, Vector3 x, float t)
	{
		t = Mathf.Clamp(t, -1.0f, 1.0f);

		return LinearBeizer(
			LinearBeizer(a, x, t),
			LinearBeizer(x, b, t),
			t
		);
	}

	/// <summary>Calculates a Cubic Beizer Curve point relative to the time, following the formula [B(P0,P1,P2,t) = (1-t)B(P0,P1,t) + tB(P1,P2,t)].</summary>
	/// <param name="a">Initial value [P0].</param>
	/// <param name="b">Destiny value [Pf].</param>
	/// <param name="x">First tanget vector between initialPoint and finalPoint [P1].</param>
	/// <param name="y">Second tangent vector petween initialPoint and finalPoint [P2].</param>
	/// <param name="t">Normalized t, clamped internally between -1 and 1.</param>
	/// <returns>Cubic Beizer Curve point relative to given normalized time.</returns>
	public static Vector3 CubicBeizer(Vector3 a, Vector3 b, Vector3 x, Vector3 y, float t)
	{
		t = Mathf.Clamp(t, -1.0f, 1.0f);

		Vector3 ax = LinearBeizer(a, x, t);
		Vector3 xy = LinearBeizer(x, y, t);
		Vector3 yb = LinearBeizer(y, b, t);

		return LinearBeizer(
			LinearBeizer(ax, xy, t),
			LinearBeizer(xy, yb, t),
			t
		);
	}

	/// \TODO IDK!!
	public static Vector3 NBeizer(Vector3 a, Vector3 b, float t, params Vector3[] x)
	{
		if(x == null || x.Length <= 0) return LinearBeizer(a, b, t);

		Vector3[] lbs = new Vector3[x.Length - 1];

		for(int i = 0; i < x.Length; i++)
		{

		}

		return Vector3.zero;
	}

	/// <summary>Gets middle point between n number of points (positions).</summary>
	/// <param name="_points">The points from where the middle point will be calculated.</param>
	/// <returns>Middle point between n points.</returns>
	public static Vector3 GetMiddlePointBetween(params Vector3[] _points)
	{
		Vector3 middlePoint = Vector3.zero;

		for(int i = 0; i < _points.Length; i++)
		{
			middlePoint += _points[i];
		}

		return (middlePoint / _points.Length);
	}

	/// <summary>Gets normalized point between n number of points (positions).</summary>
	/// <param name="_normalizedValue">The normal of the points sumatory.</param>
	/// <param name="_points">The points from where the middle point will be calculated.</param>
	/// <returns>Normalized point between n points.</returns>
	public static Vector3 GetNormalizedPointBetween(float _normalizedValue, params Vector3[] _points)
	{
		Vector3 middlePoint = Vector3.zero;

		for(int i = 0; i < _points.Length; i++)
		{
			middlePoint += _points[i];
		}

		return (middlePoint * _normalizedValue.Clamp(-1.0f, 1.0f));
	}

	public static float DotProductAngle(Vector3 a, Vector3 b)
	{
		return Mathf.Acos(Vector3.Dot(a, b) / (a.magnitude * b.magnitude)) * RAD_TO_DEG;
	}

	public static float DotProductToAngle(float d)
	{
		return Mathf.Acos(d) * RAD_TO_DEG;
	}
	
	/*
		- (|A|*|B|) * Cos(a) = Dot(A, B)
		- a = Acos(Dot(A, B)) / (|A|*|B|)
		- Dot(A, B) = (|A|*|B|) * Cos(a)
	*/
	public static float AngleToDotProduct(float a)
	{
		return Mathf.Cos(a * DEG_TO_RAD);
	}

	/// <summary>Gets interpoilation's t, given an output.</summary>
	/// <param name="_initialInput">Interpolation's initial value [P0].</param>
	/// <param name="_finalInput">Interpolation's final value [Pf].</param>
	/// <param name="_output">Interpolation's Output.</param>
	/// <returns>T deducted from the given output and original interpolation's data.</returns>
	public static float GetInterpolationTime(float _initialInput, float _finalInput, float _output)
	{
		return ((_output - _initialInput) / (_finalInput - _initialInput));
	}
#endregion

#region Steering1DFunctions:
	/// <summary>Gets Seek force on 1-Axis (as if 1D).</summary>
	/// <param name="c">Current value.</param>
	/// <param name="t">Target Value.</param>
	/// <param name="v">Current Velocity's reference.</param>
	/// <param name="s">Maximum's Speed.</param>
	/// <param name="f">Maximum's Steering Force.</param>
	/// <param name="m">Maxx [1.0f by default].</param>
	public static float GetSeekforce(float c, float t, ref float v, float s, float f, float m = 1.0f)
	{
		float d = Mathf.Sign(t - c) * s; 				/// As if we normalized the delta multiplied by maxSpeed.
		float steering = Mathf.Clamp((d - v), -f, f); 	/// As if we clamped the magnitude by maxForce.

		if(m != 1.0f) steering /= m;

		v = Mathf.Clamp((v + steering), -s, s);

		return v;
	}

	/// \TODO Do the rest of the Steering Functions...
#endregion

	/// <summary>Accelerates x value into y value.</summary>
    /// <param name="x">Value to accelerate.</param>
    /// <param name="y">Target value.</param>
    /// <param name="v">Velocity's reference.</param>
    /// <param name="a">Acceleration rate (x / s^2).</param>
    /// <param name="dt">Time's Delta.</param>
    /// <param name="m">Speed Change's Mode. Acceleration by default.</param>
    /// <param name="e">Epsilon's tolerance.</param>
    public static float AccelerateTowards(float x, float y, ref float v, float a, float dt, SpeedChange m = SpeedChange.Acceleration, float e = EPSILON)
    {
        /*Where:
        - d = Delta or error margin between the target and current value
        - s = Sign of the desired direction
        - p = Projection of the current value with the new velocity value (for overlapping evaluation)*/

        float d = y - x;

        if(Mathf.Abs(d) <= e)
        {
            v = 0.0f;
            x = y;
            return x;
        }

        float s = Mathf.Sign(d);

        /*Reset the velocity if the direction is different than the velocity's.
        I do this reset in case the previous accumulated velocity is too much,
        which would make the velocity to take a while before pointing towards
        the same direction.*/
        if(Mathf.Sign(v) != s) v = 0.0f;

        switch(m)
        {
            // Accumulate velocity as the acceleration keeps happening (x / s^2)
            case SpeedChange.Acceleration:
                v += (s * a * dt * dt);
            break;

            // Apply linear acceleration (x / s)
            case SpeedChange.Linear:
                v = s * a * dt;
            break;
        }
        
        float p = x + v;

        /* If the projection would overlap the target value
        (being more or less than the target depending of) the direction,
        add delta instead of the velocity (setting x equal to y is also valid)*/
        if(s == 1.0f ? p > y : p < y)
        {
            v = 0.0f;
            x += d;
        }
        else x += v;

        /* Maybe clamping shouldn't be necessary if the overlapping operations are correct.
        Paranoia from my part?*/
        return s == 1.0f ? Mathf.Min(x, y) : Mathf.Max(x, y);
    }

#region Ray2DOperations:
	/// <summary>Calculates a 2X2 determinant, given two bidimensional Rays.</summary>
	/// <param name="a">Ray A.</param>
	/// <param name="b">Ray B.</param>
	/// <returns>2X2's determinant of Ray A and Ray B.</returns>
	public static float Determinant(Ray2D a, Ray2D b)
	{
		return ((a.direction.y * b.direction.x) - (a.direction.x * b.direction.y));
		//return ((b.direction.x * a.direction.y) - (b.direction.y * a.direction.x));
	}

	/// <summary>Interpolates ray towards direction, given a time t.</summary>
	/// <param name="_ray">Ray to interpolate.</param>
	/// <param name="t">Time reference.</param>
	/// <returns>Interpolation between Ray's origin and direction on t time, as a Vector2.</returns>
	public static Vector2 Lerp(this Ray2D _ray, float t)
	{
		return (_ray.origin + (t * _ray.direction));
	}

	/// <summary>Calculates for intersection between Ray A and B.</summary>
	/// <param name="a">Ray A.</param>
	/// <param name="b">Ray B.</param>
	/// <returns>Intersection between Rays A and B if there is, null otherwise.</returns>
	public static Vector2? CalculateIntersectionBetween(Ray2D a, Ray2D b)
	{
		float determinant = Determinant(a, b);
		if(determinant == 0.0f) return null;
		float determinantMultiplicativeInverse = (1.0f / determinant);
		float deltaX = (a.origin.x - b.origin.x);
		float deltaY = (b.origin.y - a.origin.y);
		float tA = ((deltaY * b.direction.x) + (deltaX * b.direction.y)) * determinantMultiplicativeInverse;
		float tB = ((deltaY * a.direction.x) + (deltaX * a.direction.y)) * determinantMultiplicativeInverse;

		return (tA >= 0.0f && tB >= 0.0f) ? a.Lerp(tA) : (Vector2?)null;
	}
#endregion

#region RandomOperations:
	/// <summary>Controlled Random function [you pass the seed].</summary>
	/// <param name="min">Min Random Value.</param>
	/// <param name="max">Max Random Value.</param>
	/// <param name="seed">Custom seed [it does not change with time].</param>
	public static int Rand(int min, int max, int seed)
	{
		return min + (seed % (max - min));
	}
	
	/// <summary>Gets a unique [not duplicate] set of random integers.</summary>
	/// <param name="_range">Random's Range [max gets excluded].</param>
	/// <param name="_count">Size of the set.</param>
	/// <returns>Set of random sorted unique integers.</returns>
	public static int[] GetUniqueRandomSet(IntRange _range, int _count)
	{
		HashSet<int> numbersSet = new HashSet<int>();

		for(int i = (_range.max - _count); i < _range.max; i++)
		{
			if(!numbersSet.Add(UnityEngine.Random.Range(_range.min, (i + 1))))
			numbersSet.Add(i);
		}

		int[] result = numbersSet.ToArray();

		for(int i = (result.Length - 1); i > 0; i--)
		{
			int n = UnityEngine.Random.Range(_range.min, (i + 1));
			int x = result[n];
			result[n] = result[i];
			result[i] = x;			
		}

		return result;
	}

	/// <summary>Gets a unique [not duplicate] set of random integers, from 0 to given count.</summary>
	/// <param name="_count">Size of the set.</param>
	/// <returns>Set of random sorted unique integers.</returns>
	public static int[] GetUniqueRandomSet(int _count)
	{
		HashSet<int> numbersSet = new HashSet<int>();

		for(int i = 0; i < _count; i++)
		{
			if(!numbersSet.Add(UnityEngine.Random.Range(0, (i + 1))))
			numbersSet.Add(i);
		}

		int[] result = numbersSet.ToArray();

		for(int i = (result.Length - 1); i > 0; i--)
		{
			int n = UnityEngine.Random.Range(0, (i + 1));
			int x = result[n];
			result[n] = result[i];
			result[i] = x;			
		}

		return result;
	}
#endregion

	/// <summary>Calculates Rectified Linear Unit of given number.</summary>
	/// <param name="x">Unit to rectify.</param>
	/// <returns>Number rectified.</returns>
	public static float RectifiedLinearUnit(float x)
	{
		return (x >= 0.0f ? x : 0.0f);
	}

	/// <summary>Checks if a dot product between 2 vectors is between an angle of tolerance.</summary>
	/// <param name="a">Vector A.</param>
	/// <param name="b">Vector b.</param>
	/// <param name="degreeTolerance">Degree Tolerance.</param>
	/// <returns>True if the dot product between two vectors is between given tolerance angle.</returns>
	public static bool DotProductWithinAngle(Vector3 a, Vector3 b, float degreeTolerance)
	{
		float dot = Vector3.Dot(a, b);
		float angleToDot = Mathf.Cos(degreeTolerance * DEG_TO_RAD);

		return dot >= 0.0f ? dot >= angleToDot : dot <= angleToDot;
	}

#region NumberUtilities:
	/// <summary>Remaps given input from map into given range.</summary>
	/// <param name="_input">Input value to remap.</param>
	/// <param name="_map">Original values mapping.</param>
	/// <param name="_range">Range to map the input to.</param>
	/// <returns>Input mapped into given range.</returns>
	public static float RemapValue(float _input, FloatRange _map, FloatRange _range)
	{
		return (((_range.max - _range.min) * (_input - _map.min)) / (_map.max - _map.min)) + _range.min;
	}

	/// <summary>Remaps given input from map into given range.</summary>
	/// <param name="_input">Input value to remap.</param>
	/// <param name="_mapMin">Original values mapping's minimum value.</param>
	/// <param name="_mapMax">Original values mapping's maximum value.</param>
	/// <param name="_rangeMin">Range's minimum value.</param>
	/// <param name="_rangeMax">Range's maximum value.</param>
	/// <returns>Input mapped into given range.</returns>
	public static float RemapValue(float _input, float _mapMin, float _mapMax, float _rangeMin, float _rangeMax)
	{
		return (((_rangeMax - _rangeMin) * (_input - _mapMin)) / (_mapMax - _mapMin)) + _rangeMin;
	}

	/// <summary>Remaps given input from map into normalized range.</summary>
	/// <param name="_input">Input value to remap.</param>
	/// <param name="_mapMin">Original values mapping's minimum value.</param>
	/// <param name="_mapMax">Original values mapping's maximum value.</param>
	/// <returns>Input mapped into normalizedRange.</returns>
	public static float RemapValueToNormalizedRange(float _input, FloatRange _map)
	{
		return ((_input - _map.min) / (_map.max - _map.min));
	}

	/// <summary>Remaps given input from map into normalized range.</summary>
	/// <param name="_input">Input value to remap.</param>
	/// <param name="_map">Original values mapping.</param>
	/// <returns>Input mapped into normalizedRange.</returns>
	public static float RemapValueToNormalizedRange(float _input, float _mapMin, float _mapMax)
	{
		return ((_input - _mapMin) / (_mapMax - _mapMin));
	}

	/// <summary>Calculates normalized t from input and given range.</summary>
	/// <param name="x">Input.</param>
	/// <param name="min">Range's Minimum Value.</param>
	/// <param name="max">Range's Maximum Value.</param>
	/// <returns>Normalized T.</returns>
	public static float T(float x, float min, float max)
	{
		return (x - min) / (max - min);
	}

	public static float Clamp(this float x, float min, float max)
	{
		return Mathf.Clamp(x, min, max);
	}

	public static int Clamp(this int x, int min, int max)
	{
		return Mathf.Clamp(x, min, max);
	}

	/// <summary>Sets Integer to clamped value.</summary>
	/// <param name="x">Integer that will be clamped.</param>
	/// <param name="min">Minimum value clamped.</param>
	/// <param name="max">Maximum value clamped.</param>
	/// <returns>Integer clamped (as int).</returns>
	public static int ClampSet(ref int x, int min, int max)
	{
		return x = x < min ? min : x > max ? max : x;
	}

	/// <summary>Sets float to clamped value.</summary>
	/// <param name="x">Float that will be clamped.</param>
	/// <param name="min">Minimum value clamped.</param>
	/// <param name="max">Maximum value clamped.</param>
	/// <returns>Float clamped (as float).</returns>
	public static float ClampSet(ref float x, float min, float max)
	{
		return x = x < min ? min : x > max ? max : x;
	}

	/// <summary>Calculates negative absolute of given number.</summary>
	/// <param name="x">Value to convert to negative absolute.</param>
	/// <returns>Number passed to negative absolute.</returns>
	public static float NegativeAbs(float x)
	{
		return (x < 0.0f ? x : (x * -1.0f));
	}	
#endregion

	/// <summary>Takes a set of Float Wrappers to make them dependable given a raange.</summary>
	/// <param name="min">Range's minimum value.</param>
	/// <param name="max">Range's maximum value.</param>
	/// <param name="numberSet">Set of Float Wrappers to modifyu so they are dependable to each other.</param>
	public static void ToDependableNumberSet(float min, float max, params Wrapper<float>[] numberSet)
	{
		float lastMin = min;

		foreach(Wrapper<float> x in numberSet)
		{
			x.value = Mathf.Clamp(x.value, lastMin, max);
			lastMin = x.value;
		}
	}

	/// <summary>Gets Range's Median.</summary>
	/// <param name="min">Minimum value.</param>
	/// <param name="max">Minimum value.</param>
	/// <returns>Range's Median.</returns>
	public static float GetMedian(float min, float max)
	{
		return min + ((max - min) * 0.5f);
	}

#region ShortFunctions:
	/// <summary>Clamps short to a maximum value.</summary>
	/// <param name="x">Short to clamp.</param>
	/// <param name="max">Maximum value possible.</param>
	/// <returns>Clamped short value.</returns>
	public static short Max(short x, short max)
	{
		return x > max ? max : x;
	}

	/// <summary>Clamps short to a minimum value.</summary>
	/// <param name="x">Short to clamp.</param>
	/// <param name="min">Minimum value possible.</param>
	/// <returns>Clamped short value.</returns>
	public static short Min(short x, short min)
	{
		return x < min ? min : x;
	}

	/// <summary>Clamps short between a minimum and maximum value.</summary>
	/// <param name="x">Short to clamp.</param>
	/// <param name="max">Maximum value possible.</param>
	/// <param name="min">Minimum value possible.</param>
	/// <returns>Clamped short value.</returns>
	public static short Clamp(short x, short min, short max)
	{
		return x < min ? min : x > max ? max : x;
	}
#endregion

#region LongFunctions:
	/// <summary>Clamps long to a maximum value.</summary>
	/// <param name="x">Long to clamp.</param>
	/// <param name="max">Maximum value possible.</param>
	/// <returns>Clamped long value.</returns>
	public static long Max(long x, long max)
	{
		return x > max ? max : x;
	}

	/// <summary>Clamps long to a minimum value.</summary>
	/// <param name="x">Long to clamp.</param>
	/// <param name="min">Minimum value possible.</param>
	/// <returns>Clamped long value.</returns>
	public static long Min(long x, long min)
	{
		return x < min ? min : x;
	}

	/// <summary>Clamps long between a minimum and maximum value.</summary>
	/// <param name="x">Long to clamp.</param>
	/// <param name="max">Maximum value possible.</param>
	/// <param name="min">Minimum value possible.</param>
	/// <returns>Clamped long value.</returns>
	public static long Clamp(long x, long min, long max)
	{
		return x < min ? min : x > max ? max : x;
	}
#endregion

#region AreasVolumes&Perimeters:
	/// <summary>Calculates radius from given area.</summary>
	/// <param name="a">Area.</param>
	/// <returns>Radius from given area.</returns>
	public static float RadiusFromArea(float a)
	{
		return Mathf.Sqrt(a / Mathf.PI);
	}

	/// <summary>Calculates the area of a circle.</summary>
	/// <param name="r">Circle's Radius.</param>
	/// <returns>Circle's Area.</returns>
	public static float AreaOfCircle(float r)
	{
		return Mathf.PI * (r * r);
	}

	/// <summary>Calculates the area of an ellipse [it does not matter the order of factors, they commute].</summary>
	/// <param name="a">Radius A.</param>
	/// <param name="b">Radius B.</param>
	/// <returns>Ellipse's Area.</returns>
	public static float AreaOfEllipse(float a, float b)
	{
		return Mathf.PI * (a * b);
	}

	/// <summary>Calculates the area of a sphere.</summary>
	/// <param name="r">Sphere's Radius.</param>
	/// <returns>Sphere's Volume.</returns>
	public static float VolumeOfASphere(float r)
	{
		float x = 1.3333333333333333333333333333333333333333333333f; // 4/3
		return x * Mathf.PI * (r * r * r);
	}

	/// <summary>Calculates the area of an irregular triangle using Heron's formula.</summary>
	/// <param name="a">Point of triangle A.</param>
	/// <param name="b">Point of triangle B.</param>
	/// <param name="c">Point of triangle C.</param>
	/// <returns>Area of irregular triangle.</returns>
	public static float AreaOfIrregularTriangle(Vector2 a, Vector2 b, Vector2 c)
	{
		return AreaOfIrregularTriangle(
			(a - b).magnitude,
			(b - c).magnitude,
			(a - c).magnitude
		);
	}

	/// <summary>Calculates the area of an irregular triangle using Heron's formula.</summary>
	/// <param name="a">Point of triangle A.</param>
	/// <param name="b">Point of triangle B.</param>
	/// <param name="c">Point of triangle C.</param>
	/// <returns>Area of irregular triangle.</returns>
	public static float AreaOfIrregularTriangle(Vector3 a, Vector3 b, Vector3 c)
	{
		return AreaOfIrregularTriangle(
			(a - b).magnitude,
			(b - c).magnitude,
			(a - c).magnitude
		);
	}

	/// <summary>Calculates the area of an irregular triangle using Heron's formula.</summary>
	/// <param name="a">Length of side A.</param>
	/// <param name="b">Length of side B.</param>
	/// <param name="c">Length of side C.</param>
	/// <returns>Area of irregular triangle.</returns>
	public static float AreaOfIrregularTriangle(float a, float b, float c)
	{
		// Semi-Perimeter:
		float s = (a + b + c) * 0.5f;

		return Mathf.Sqrt((s * (s - a)) * (s * (s - b)) * (s * (s - c)));
	}
#endregion

#region ComparissonUtilities:
	public static int Factorial(int x)
	{
		if(x <= 0)
		{
			throw new Exception("No Value below 1 can be received");
		
		} else if(x == 1) return 1;
		else return x * Factorial((x - 1));
	
		int f = x;

		while(x > 1)
		{
			f *= --x;
		}

		return f;
	}

	public static float Factorial(float x)
	{
		if(x <= 0.0f)
		{
			throw new Exception("No Value below 1 can be received");

		} else if(x == 1.0f) return 1.0f;
		else return x * Factorial((x - 1.0f));

		float f = x;

		while(x > 1.0f)
		{
			f *= --x;
		}

		return f;
	}

	/// <returns>Random 0.0f-360.0f degree.</returns>
	public static float RandomDegree()
	{
		return UnityEngine.Random.Range(0.0f, 360.0f);
	}

	/// <summary>Checks if to float values are equal [below or equal the difference tolerance].</summary>
	/// <param name="_f1">First float value.</param>
	/// <param name="_f2">Second float value.</param>
	/// <param name="_differenceTolerance">Difference's Tolerance between the two float values [Epsilon by default].</param>
	/// <returns>True if both float values are equal between epsilon's range.</returns>
	public static bool Equal(float _f1, float _f2, float _differenceTolerance = 0.0f)
	{
		return Mathf.Abs(_f1 - _f2) <= (_differenceTolerance == 0.0f ? Mathf.Epsilon : _differenceTolerance);
	}

	/// <summary>Checks if to float values are different [above or equal the difference tolerance].</summary>
	/// <param name="_f1">First float value.</param>
	/// <param name="_f2">Second float value.</param>
	/// <param name="_differenceTolerance">Difference's Tolerance between the two float values [Epsilon by default].</param>
	/// <returns>True if both float values are different between epsilon's range.</returns>
	public static bool Different(float _f1, float _f2, float _differenceTolerance = 0.0f)
	{
		return Mathf.Abs(_f1 - _f2) >= (_differenceTolerance == 0.0f ? Mathf.Epsilon : _differenceTolerance);
	}
#endregion

	public static float Get360Angle(Vector2 v)
	{
		if(v.sqrMagnitude == 0.0f) return 0.0f;

		float angle = Mathf.Atan2(v.x, v.y) * RAD_TO_DEG;
	
		return angle;
	}

	/// <summary>Gets 360 system angle between 2 points.</summary>
	/// <param name="_fromPoint">Point from where the angle starts.</param>
	/// <param name="_toPoint">Point the origin point is pointing towards.</param>
	/// <param name="_coordinatesMode">Coordinates Mode.</param>
	/// <returns>360 range angle (as float).</returns>
	public static float Get360Angle(Vector3 _fromPoint, Vector3 _toPoint, CoordinatesModes _coordinatesMode)
	{
		Vector2 direction = Vector2.zero;

		switch(_coordinatesMode)
		{
			case CoordinatesModes.XY:
			direction = new Vector2((_fromPoint.x - _toPoint.x), (_fromPoint.y - _toPoint.y));
			break;

			case CoordinatesModes.YX:
			direction = new Vector2((_fromPoint.y - _toPoint.y), (_fromPoint.x - _toPoint.x));
			break;

			case CoordinatesModes.XZ:
			direction = new Vector2((_fromPoint.x - _toPoint.x), (_fromPoint.z - _toPoint.z));
			break;

			case CoordinatesModes.ZY:
			direction = new Vector2((_fromPoint.z - _toPoint.z), (_fromPoint.y - _toPoint.y));
			break;

			case CoordinatesModes.YZ:
			direction = new Vector2((_fromPoint.y - _toPoint.y), (_fromPoint.z - _toPoint.z));
			break;

			case CoordinatesModes.ZX:
			direction = new Vector2((_fromPoint.z - _toPoint.z), (_fromPoint.x - _toPoint.x));
			break;
		}

		return direction.y < 0f || direction.x < 0f &&direction.y < 0f ? (Mathf.Atan2(direction.y, direction.x) + (PI * 2)) * RAD_TO_DEG : Mathf.Atan2(direction.y, direction.x) * RAD_TO_DEG;
	}
}
}