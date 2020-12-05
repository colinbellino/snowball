using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Spawn))]
public class SpawnPropertyDrawer : PropertyDrawer
{
	public override VisualElement CreatePropertyGUI(SerializedProperty property)
	{
		var container = new VisualElement();

		var timeField = new PropertyField(property.FindPropertyRelative("Time"));
		var positionField = new PropertyField(property.FindPropertyRelative("Position"));
		var prefabField = new PropertyField(property.FindPropertyRelative("Data"));

		container.Add(timeField);
		container.Add(positionField);
		container.Add(prefabField);

		return container;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		// Don't make child fields be indented
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		var timeRect = new Rect(position.x, position.y, 40, position.height);
		var positionRect = new Rect(position.x + 50, position.y, 100, position.height);
		var prefabRect = new Rect(position.x + 160, position.y, position.width - 160, position.height);

		// Draw fields - pass GUIContent.none to each so they are drawn without labels
		EditorGUI.PropertyField(timeRect, property.FindPropertyRelative("Time"), GUIContent.none);
		EditorGUI.PropertyField(positionRect, property.FindPropertyRelative("Position"), GUIContent.none);
		EditorGUI.PropertyField(prefabRect, property.FindPropertyRelative("Data"), GUIContent.none);

		// Set indent back to what it was
		EditorGUI.indentLevel = indent;

		EditorGUI.EndProperty();
	}
}
