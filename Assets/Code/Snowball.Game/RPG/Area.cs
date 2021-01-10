using System.Collections.Generic;
using UnityEngine;

namespace Snowball.Game
{
	[CreateAssetMenu(menuName = "Snowball/RPG/Area")]
	public class Area : ScriptableObject
	{
		public Vector2Int Size;
		public int[] Tiles;
		public List<Vector2Int> AllySpawnPoints;
		public List<Vector2Int> FoeSpawnPoints;
	}
}
