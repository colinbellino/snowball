using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Bark))]
public class BarkPropertyDrawer : PropertyDrawer
{
	public override VisualElement CreatePropertyGUI(SerializedProperty property)
	{
		var container = new VisualElement();

		var textField = new PropertyField(property.FindPropertyRelative("Text"));

		container.Add(textField);

		return container;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Bark"));

		// Don't make child fields be indented
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		var textRect = new Rect(position.x, position.y, position.width, position.height);

		// Draw fields - pass GUIContent.none to each so they are drawn without labels
		EditorGUI.PropertyField(textRect, property.FindPropertyRelative("Text"), GUIContent.none);

		// Set indent back to what it was
		EditorGUI.indentLevel = indent;

		EditorGUI.EndProperty();
	}
}
