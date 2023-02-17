using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomEditor(typeof(WaypointGenerator))]
public class WaypointGeneratorInspector : BaseWaypointGeneratorInspector<WaypointGenerator, Waypoint>
{
	
}
}