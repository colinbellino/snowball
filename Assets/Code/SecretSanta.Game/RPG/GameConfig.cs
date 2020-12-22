using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.SecretSanta.Game.RPG
{
	[CreateAssetMenu(menuName = "Secret Santa/RPG/Game Config")]
	public class GameConfig : ScriptableObject
	{
		// 0: Empty
		// 1: Ground
		// 2: Ally Spawn
		// 3: Foe Spawn
		public TileBase[] Tiles;

		public UnitComponent UnitPrefab;

		[Header("Effects")]
		public ParticleSystem HitEffect;
	}
}
