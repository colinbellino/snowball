using System.Collections.Generic;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	[CreateAssetMenu(menuName = "Secret Santa/RPG/Area")]
	public class Area : ScriptableObject
	{
		public Vector2Int Size;
		public int[] Tiles;
		public List<Vector2Int> AllySpawnPoints;
		public List<Vector2Int> FoeSpawnPoints;
	}
}
