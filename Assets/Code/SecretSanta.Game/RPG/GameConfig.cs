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
		public EncounterAuthoring[] Encounters;
		public UnitFacade UnitPrefab;
		public GameObject SnowballPrefab;
		public FloatingText DamageTextPrefab;

		[Title("Debug")]
		[SerializeField] private List<UnitAuthoring> _startingParty;
		[SerializeField] private List<EncounterAuthoring> _encountersOrder;
		public List<int> StartingParty => _startingParty.Select(unit => unit.Id).ToList();
		public List<int> EncountersOrder => _encountersOrder.Select(encounter => encounter.Id).ToList();
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
