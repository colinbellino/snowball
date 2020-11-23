using UnityEngine;

public static class Helpers
{
	public static Player SpawnPlayer(Player prefab)
	{
		var player = GameObject.Instantiate(prefab);
		return player;
	}
}
