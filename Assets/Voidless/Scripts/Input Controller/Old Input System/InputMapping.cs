using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public class InputMapping
{
	[SerializeField] private string[] _keyNames; 											/// <summary>Key Names.</summary>
	[SerializeField] private PCControllerSetup _PCControllerSetup; 							/// <summary>PC's Controller Setup.</summary>
	[SerializeField] private XBoxControllerSetup _XBoxControllerSetup; 						/// <summary>XBox's Controller Setup.</summary>
	[SerializeField] private NintendoSwitchControllerSetup _NintendoSwitchControllerSetup; 	/// <summary>Nintendo Switch's Controller Setup.</summary>
	[SerializeField] private N3DSControllerSetup _N3DSControllerSetup; 						/// <summary>N3DS's Controller Setup.</summary>

	/// <summary>Gets and Sets keyNames property.</summary>
	public string[] keyNames
	{
		get { return _keyNames; }
		set { _keyNames = value; }
	}

	/// <summary>Gets and Sets PCControllerSetup property.</summary>
	public PCControllerSetup PCControllerSetup
	{
		get { return _PCControllerSetup; }
		set { _PCControllerSetup = value; }
	}

	/// <summary>Gets and Sets XBoxControllerSetup property.</summary>
	public XBoxControllerSetup XBoxControllerSetup
	{
		get { return _XBoxControllerSetup; }
		set { _XBoxControllerSetup = value; }
	}

	/// <summary>Gets and Sets NintendoSwitchControllerSetup property.</summary>
	public NintendoSwitchControllerSetup NintendoSwitchControllerSetup
	{
		get { return _NintendoSwitchControllerSetup; }
		set { _NintendoSwitchControllerSetup = value; }
	}

	/// <summary>Gets and Sets N3DSControllerSetup property.</summary>
	public N3DSControllerSetup N3DSControllerSetup
	{
		get { return _N3DSControllerSetup; }
		set { _N3DSControllerSetup = value; }
	}

	/// <summary>InputMapping's Constructor.</summary>
	public InputMapping()
	{
		keyNames = new string[0];
		PCControllerSetup = new PCControllerSetup();
		XBoxControllerSetup = new XBoxControllerSetup();
		NintendoSwitchControllerSetup = new NintendoSwitchControllerSetup();
		N3DSControllerSetup = new N3DSControllerSetup();
	}

	/// <summary>Resizes the mapping of all controller setups so they all match the maximum size.</summary>
	public void ResizeMappings()
	{
		int maxSize = Mathf.Max
		(
			PCControllerSetup.keyMapping.Length,
			XBoxControllerSetup.keyMapping.Length,
			NintendoSwitchControllerSetup.keyMapping.Length,
			N3DSControllerSetup.keyMapping.Length
		);

		PCControllerSetup.ResizeMapping(maxSize);
		XBoxControllerSetup.ResizeMapping(maxSize);
		NintendoSwitchControllerSetup.ResizeMapping(maxSize);
		N3DSControllerSetup.ResizeMapping(maxSize);
	}

	/// <summary>Resizes Keys' Mapping.</summary>	
	/// <param name="_newSize">Mapping's New Size.</param>
	public void ResizeMapping(int _newSize)
	{
		Array.Resize(ref _keyNames, _newSize);
	}

	/// <returns>String representing this Input's Controller.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("PC's Controller Setup: ");
		builder.AppendLine(PCControllerSetup.ToString());	
		builder.Append("XBox's Controller Setup: ");
		builder.AppendLine(XBoxControllerSetup.ToString());
		builder.Append("Nintendo Switch' Controller Setup: ");
		builder.AppendLine(NintendoSwitchControllerSetup.ToString());
		builder.Append("Nintendo 3DS' Controller Setup: ");
		builder.AppendLine(N3DSControllerSetup.ToString());

		return builder.ToString();
	}
}
}