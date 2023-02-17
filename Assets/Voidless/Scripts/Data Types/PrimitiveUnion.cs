using System.Runtime.InteropServices;
using System;
using UnityEngine;

namespace Voidless
{
public enum DataType
{
	String,
	Character,
	Float,
	Integer,
	Boolean
}
//
[Serializable]
//[StructLayout(LayoutKind.Explicit)]
public /*unsafe*/ struct PrimitiveUnion
{
	/*union Popo{
	//[FieldOffset(0)]
	[SerializeField] public DataType type;
	//[FieldOffset(1)]
	[SerializeField] public string stringData;
	//[FieldOffset(1)]
	[SerializeField] public char charData;
	//[FieldOffset(1)]
	[SerializeField] public float floatData;
	//[FieldOffset(1)]
	[SerializeField] public int intData;
	//[FieldOffset(1)]
	[SerializeField] public bool boolData;
	}

	[SerializeField] public Popo caca;

	public override string ToString()
	{
		return Marshal.SizeOf(typeof(PrimitiveUnion)).ToString();
	}*/
}
}