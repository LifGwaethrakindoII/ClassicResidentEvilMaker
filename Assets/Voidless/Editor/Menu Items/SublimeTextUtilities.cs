using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
public static class SublimeTextUtilities
{
	[MenuItem("Voidless/Sublime Text Utilities/Create Sublime Project")]
	public static void CreateSublimeProject()
	{
		Debug.Log("[SublimeTextUtilities] Creating Window for Sublime-Project Creation for project " + VString.GetProjectName());
		Debug.Log("[SublimeTextUtilities] Project Path: " + VString.GetProjectPath());
	}
}
}

/*
StringBuilder builder = new StringBuilder();

builder.Append("{\n\t\t");
builder.Append("\"folders:\"");


string AddPath(string path)
{
	StringBuilder.AppendLine("{");
	StringBuilder.Append("\tfile_excludepatterns");
} 
*/