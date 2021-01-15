using System.IO;
using UnityEngine;

namespace Snowball.Game
{
	public static class SaveHelpers
	{
		private static readonly string _path = Path.Combine(Application.persistentDataPath, "GameState.json");

		public static void SaveToFile(GameState state)
		{
			var json = JsonUtility.ToJson(state, true);
			File.WriteAllText(_path, json);
			Debug.Log("Saving game state: " + _path + "\n" + json);
		}

		public static bool HasFile()
		{
			return File.Exists(_path);
		}

		public static GameState LoadFromFile()
		{
			var json = File.ReadAllText(_path);
			Debug.Log("Loading game state: " + _path + "\n" + json);
			return JsonUtility.FromJson<GameState>(json);
		}
	}
}
