using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	[CreateAssetMenu(menuName = "Secret Santa/RPG/Area")]
	public class Area : ScriptableObject
	{
		// ^
		// y
		// |
		// • - x >
		public int[] Tiles;
		public Vector2Int Size;
	}
}
