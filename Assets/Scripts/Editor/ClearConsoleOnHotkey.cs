#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
#endif
using UnityEngine;

public static class ClearConsoleOnHotkey
{
#if UNITY_EDITOR
	[MenuItem("Tools/Clear Console %q")] // CTRL + Q
	private static void ClearConsole()
	{
		Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
		Type type = assembly.GetType("UnityEditor.LogEntries");
		MethodInfo method = type.GetMethod("Clear");
		method.Invoke(new object(), null);
	}
#endif
}