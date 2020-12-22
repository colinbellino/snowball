using UnityEngine;
using UnityEngine.Serialization;
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

		public Encounter[] Encounters;
		public UnitComponent UnitPrefab;
		public GameObject SnowballPrefab;

		[FormerlySerializedAs("HitEffect")] [Header("Effects")]
		public ParticleSystem HitEffectPrefab;
	}
}
