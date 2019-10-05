//
// Adds empty GameObject as a Hierarchy separator. Sets tag to EditorOnly not to be included in build
// usage from Editor menu, or directly as a context menu item in Hieararchy
//
using UnityEngine;
using UnityEditor;

public class HierarchySeparator : MonoBehaviour
{
	const string separatorGOName = "---";
	const string editorOnlyTag = "EditorOnly";
	
	// add a menu item to create custom GameObjects
	[MenuItem("GameObject/--- EditorOnly Separator", false, 20)]
	static void CreateCustomGameObject(MenuCommand menuCommand)
	{
		GameObject go = new GameObject(separatorGOName);
		// ensure it gets reparented if this was a context click (otherwise does nothing)
		GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
		// register the creation in the undo system
		Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
		
		go.transform.position = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.tag = editorOnlyTag;
		
		Selection.activeObject = go;
	}
}