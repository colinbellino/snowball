using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.SecretSanta.Game.RPG
{
	public class TilesData : Dictionary<int, TileData> { }

	[CreateAssetMenu(menuName = "Secret Santa/RPG/Game Config")]
	public class GameConfig : SerializedScriptableObject
	{
		public Vector2Int TilemapSize = new Vector2Int(32, 18);

		[Title("Tilemap")]
		public TilesData TilesData;
		public TileBase AllySpawnTile;
		public TileBase FoeSpawnTile;
		public TileBase EmptyTile;
		public TileBase HighlightTile;

		[Title("Effects")]
		public ParticleSystem HitEffectPrefab;

		[Title("Data")]
		public EncounterAuthoring[] Encounters;
		public UnitFacade UnitPrefab;
		public GameObject SnowballPrefab;
		public FloatingText DamageTextPrefab;

		[Title("Debug")]
		public List<int> StartingParty;

	}

	public class TileData
	{
		public TileBase Tile;
		public bool Walkable;
		public bool Climbable;
		public bool Blocking;
	}
}
