using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Exposes property of type int as a dropdown filled with Unity Physics Layers
/// </summary>
public class PhysicsLayerAttribute : PropertyAttribute
{
}



#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(PhysicsLayerAttribute))]
public class PhysicsLayerDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);
		string extendedLabel = string.Format("{0} ({1})", label.text, property.intValue);
		property.intValue = EditorGUI.LayerField(position, extendedLabel, property.intValue);
		EditorGUI.EndProperty();
	}
}
#endif