using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Tilemaps;

namespace Snowball.Game
{
	public class TilesData : Dictionary<int, TileData> { }

	[CreateAssetMenu(menuName = "Snowball/RPG/Game Config")]
	public class GameConfig : SerializedScriptableObject
	{
		public Vector2Int GridSize = new Vector2Int(32, 18);
		public Vector2Int GridOffset = new Vector2Int(1, 1);

		[Title("Tilemap")]
		[Required] public TilesData TilesData;
		[Required] public TileBase AllySpawnTile;
		[Required] public TileBase FoeSpawnTile;
		[Required] public TileBase DebugTile;
		[Required] public TileBase HighlightTile;

		[Title("Effects")]
		[Required] public ParticleSystem HitEffectPrefab;
		[Required] public ParticleSystem SnowfallPrefab;

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
		[Required] public AudioMixerSnapshot DefaultAudioSnapshot;
		[Required] public AudioMixerSnapshot PauseAudioSnapshot;
		[Required] public AudioClip TitleMusic;
		[Required] public AudioClip WorldmapMusic;
		[Required] public AudioClip BattleMusic;
		[Required] public AudioClip MenuErrorClip;
		[Required] public AudioClip MenuConfirmClip;
		[Required] public AudioClip MenuCancelClip;
		[Range(0f, 1f)] public float MusicVolume = 1f;
		[Range(0f, 1f)] public float SoundVolume = 1f;

		[Required] public AudioClip UnitHitClip;
		[Required] public AudioClip UnitMissClip;
		[Required] public AudioClip UnitDeathClip;

		[Title("Debug")]
		public bool DebugSkipTitle;
		public bool DebugTurn;
		public bool DebugFSM;
		[SerializeField] private List<UnitAuthoring> _startingParty;
		[Required] public List<int> StartingParty => _startingParty.Select(unit => unit.Id).ToList();
		[Required] public Vector3Int WorldmapStart;
		[SerializeField] private UnitAuthoring _snowpalUnit;
		[Required] public int SnowmanUnitId => _snowpalUnit.Id;

		public static bool GetDebug(bool value)
		{
			#if UNITY_EDITOR
				return value;
			#endif

			return false;
		}
	}
}
