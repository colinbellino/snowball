using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		var level = (Level)target;

		if (GUILayout.Button("Validate"))
		{
			level.Sync();
			Debug.Log($"Level saved: spawners -> {level.Spawners.Count} | start -> {level.StartPosition} | win trigger -> {level.WinTrigger.position}");
		}
	}
}
