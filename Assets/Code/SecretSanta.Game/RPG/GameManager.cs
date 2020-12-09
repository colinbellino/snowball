using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.SecretSanta.Game.RPG
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private Tilemap _tilemap;

		private void Start()
		{
			var encounter = Resources.Load<Encounter>("Encounters/Encounter1");
			StartBattle(encounter);
		}

		private void StartBattle(Encounter encounter)
		{
			Helpers.LoadArea(encounter.Area, _tilemap, Game.Instance.Config.Tiles);

			for (var index = 0; index < encounter.Allies.Count; index++)
			{
				SpawnUnit(encounter.Allies[index], encounter.Area.AllySpawnPoints[index]);
			}
			for (var index = 0; index < encounter.Foes.Count; index++)
			{
				SpawnUnit(encounter.Foes[index], encounter.Area.FoeSpawnPoints[index]);
			}
		}

		private void SpawnUnit(Unit data, Vector2Int position)
		{
			// Debug.Log($"Spawning ({data.Name}) at {position}");
			var unit = Instantiate(Game.Instance.Config.UnitPrefab);
			unit.SetGridPosition(position);
			unit.UpdateData(data);
		}
	}

	public class Battle {}
}
