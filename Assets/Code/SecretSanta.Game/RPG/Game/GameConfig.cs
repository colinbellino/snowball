using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
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
		[Required] public Texture2D MouseCursor;

		[Title("Worldmap")]
		[SerializeField] [Required] private List<EncounterAuthoring> _encounters;
		[Required] public List<int> Encounters => _encounters.Select(encounter => encounter.Id).ToList();

		[Title("Audio")]
		[Required] public AudioMixer AudioMixer;
		[Required] public AudioMixerGroup MusicAudioMixerGroup;
		[Required] public AudioMixerGroup SoundsAudioMixerGroup;
		[Required] public AudioClip TitleMusic;
		[Required] public AudioClip WorldmapMusic;
		[Required] public AudioClip BattleMusic;
		[Required] public AudioClip MenuErrorClip;
		[Required] public AudioClip MenuConfirmClip;
		[Required] public AudioClip MenuCancelClip;
		[Range(0f, 1f)] public float MusicVolume = 1f;
		[Range(0f, 1f)] public float SoundVolume = 1f;

		[Title("Debug")]
		[SerializeField] private List<UnitAuthoring> _startingParty;
		[Required] public List<int> StartingParty => _startingParty.Select(unit => unit.Id).ToList();
		[Required] public Vector3Int WorldmapStart;
		[SerializeField] private UnitAuthoring _snowmanUnit;
		[Required] public int SnowmanUnitId => _snowmanUnit.Id;
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
