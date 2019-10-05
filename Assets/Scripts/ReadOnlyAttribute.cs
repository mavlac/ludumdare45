using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Makes property read-only in inspector
/// 
/// name of another bool property can be defined as optional attribute argument,
/// then read-only state is controlled by that bool state and target property is visualy intended
/// 
/// useful for making generic inspectors with optional setup
/// </summary>
public class ReadOnlyAttribute : PropertyAttribute
{
	public string linkedBoolPropertyName;
	public bool inverseLinkedBoolValue = false;
	public bool indentPropertyLabel = true;

	public ReadOnlyAttribute()
	{
	}
	public ReadOnlyAttribute(string linkedBoolPropertyName, bool inverseLinkedBoolValue = false, bool indentPropertyLabel = false)
	{
		this.linkedBoolPropertyName = linkedBoolPropertyName;
		this.inverseLinkedBoolValue = inverseLinkedBoolValue;
		this.indentPropertyLabel = indentPropertyLabel;
	}
}



#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property, label, true);
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var att = (ReadOnlyAttribute)attribute;
		var linkedProperty = property.serializedObject.FindProperty(att.linkedBoolPropertyName);
		
		if (linkedProperty != null)
		{
			// enable/disable based on linked property value
			bool linkedBoolPropertyValue = linkedProperty.boolValue;
			if (att.inverseLinkedBoolValue) linkedBoolPropertyValue = !linkedBoolPropertyValue;
			GUI.enabled = linkedBoolPropertyValue;

			if (att.indentPropertyLabel)
				EditorGUI.indentLevel++;
		}
		else
		{
			// basic functionality - just disable this component
			GUI.enabled = false;
		}

		EditorGUI.PropertyField(position, property, label, true);
		if (linkedProperty != null && att.indentPropertyLabel) EditorGUI.indentLevel--;
		
		GUI.enabled = true;
	}
}
#endif