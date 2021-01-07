using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.SecretSanta.Game.RPG
{

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
		public UnitFacade UnitPrefab;
		public GameObject SnowballPrefab;
		public FloatingText DamageTextPrefab;

		[Title("Worldmap")]
		public List<EncounterAuthoring> _encounters;
		public List<int> Encounters => _encounters.Select(encounter => encounter.Id).ToList();

		[Title("Debug")]
		[SerializeField] private List<UnitAuthoring> _startingParty;
		public List<int> StartingParty => _startingParty.Select(unit => unit.Id).ToList();
	}

	public class TilesData : Dictionary<int, TileData> { }

	public class TileData
	{
		public TileBase Tile;
		public bool Walkable;
		public bool Climbable;
		public bool Blocking;
	}
}
