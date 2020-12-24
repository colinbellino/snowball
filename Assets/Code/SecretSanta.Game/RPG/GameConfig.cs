using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.SecretSanta.Game.RPG
{
	[CreateAssetMenu(menuName = "Secret Santa/RPG/Game Config")]
	public class GameConfig : ScriptableObject
	{
		[Header("Tilemap")]
		// 0: Empty
		// 1: Ground
		public TileBase[] Tiles;
		public TileBase AllySpawnTile;
		public TileBase FoeSpawnTile;
		public TileBase[] WalkableTiles;
		public TileBase[] BlockingTiles;

		[Header("Effects")]
		public ParticleSystem HitEffectPrefab;

		[Header("Data")]
		public Encounter[] Encounters;
		public UnitComponent UnitPrefab;
		public GameObject SnowballPrefab;
	}
}
