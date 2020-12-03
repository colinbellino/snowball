using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelAuthoring))]
public class LevelAuthoringEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		var level = (LevelAuthoring)target;

		if (GUILayout.Button("Validate"))
		{
			level.Sync();
			Debug.Log($"Level saved: spawners -> {level.Spawners.Count} | win trigger -> {level.WinTriggerPosition}");
		}
	}
}
