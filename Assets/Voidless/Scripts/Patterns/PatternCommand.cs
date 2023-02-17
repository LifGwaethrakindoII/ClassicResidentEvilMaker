using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

namespace Voidless
{
public enum Command 																					/// <summary>Marshall Signal's Commands.</summary>
{
	None = 0, 																							/// <summary>No Command.</summary>
	Turn = 128, 																						/// <summary>Turn Command.</summary>
	Chocks = 256, 																						/// <summary>Chocks [Incomplete] Command.</summary>
	GPS = 512, 																							/// <summary>GPS [Incomplete] Command.</summary>
	SlowDown = 1024, 																					/// <summary>Slow Down Command.</summary>
	Fire = 2048, 																						/// <summary>Fire [Incomplete] Command.</summary>
	ThisWay, 																							/// <summary>This Way Command.</summary>
	MoveAhead, 																							/// <summary>Move Ahead Command.</summary>
	Brake, 																								/// <summary>Brake Command.</summary>
	Proceed, 																							/// <summary>Proceed Command.</summary>
	Stop, 																								/// <summary>Stop Command.</summary>
	TakeOff, 																							/// <summary>Take Off Command.</summary>

	TurnLeft = Turn | OrientationSemantics.Left, 														/// <summary>Turn Left Command.</summary>
	TurnRight = Turn | OrientationSemantics.Right, 														/// <summary>Turn Right Command.</summary>
	SlowDownLeft = SlowDown | OrientationSemantics.Left, 												/// <summary>Slow Down Left Command.</summary>
	SlowDownRight = SlowDown | OrientationSemantics.Right, 												/// <summary>Slow Down Right Command.</summary>
	RemoveChocks = Chocks | Boolean.False, 																/// <summary>Remove Chocks Command.</summary>
	InsertChocks = Chocks | Boolean.True, 																/// <summary>Insert Chocks Command.</summary>
	DisconnectGPS = GPS | Boolean.False, 																/// <summary>Disconnect GPS Command.</summary>
	ConnectGPS = GPS | Boolean.True, 																	/// <summary>Connect GPS Command.</summary>
	FireLeftUp = Fire | OrientationSemantics.Left | OrientationSemantics.Up, 							/// <summary>Fire Left Up Command.</summary>
	FireLeftDown = Fire | OrientationSemantics.Left | OrientationSemantics.Down,						/// <summary>Fire Left Down Command.</summary>
	FireRightUp = Fire | OrientationSemantics.Right | OrientationSemantics.Up, 							/// <summary>Fire Right Up Command.</summary>
	FireRightDown = Fire | OrientationSemantics.Right | OrientationSemantics.Down 						/// <summary>Fire Right Down Command.</summary>
}

public enum Boolean 																					/// <summary>Humble Boolean in an Enum form.</summary>
{
	False = 1, 																							/// <summary>False Value.</summary>
	True = 2 																							/// <summary>True Value.</summary>
}

[System.Serializable]
[XmlRoot(Namespace="Voidless", IsNullable = true)]
public class PatternCommand
{
	[SerializeField][XmlElement("Command")] public Command command; 									/// <summary>Command Infered from the Pattern.</summary>
	[SerializeField][XmlArray("KeyPoses"), XmlArrayItem("KeyPose")] public PatternData[] keyPoses; 		/// <summary>Patter's Key Poses.</summary>

	/// <summary>Parameterless PatterCommand's Constructor.</summary>
	public PatternCommand() { /*...*/ }
}
}