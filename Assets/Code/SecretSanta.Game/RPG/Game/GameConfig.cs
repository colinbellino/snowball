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
		[Required] public TilesData TilesData;
		[Required] public TileBase AllySpawnTile;
		[Required] public TileBase FoeSpawnTile;
		[Required] public TileBase EmptyTile;
		[Required] public TileBase HighlightTile;

		[Title("Effects")]
		[Required] public ParticleSystem HitEffectPrefab;

		[Title("Data")]
		[Required] public UnitFacade UnitPrefab;
		[Required] public UnitFacade UnitWorldmapPrefab;
		[Required] public GameObject SnowballPrefab;
		[Required] public FloatingText DamageTextPrefab;

		[Title("Worldmap")]
		[SerializeField] [Required] private List<EncounterAuthoring> _encounters;
		[Required] public List<int> Encounters => _encounters.Select(encounter => encounter.Id).ToList();

		[Title("Debug")]
		[SerializeField] private List<UnitAuthoring> _startingParty;
		[Required] public List<int> StartingParty => _startingParty.Select(unit => unit.Id).ToList();
		[Required] public Vector3Int WorldmapStart;
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