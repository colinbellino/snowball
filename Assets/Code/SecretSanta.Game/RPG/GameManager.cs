using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.SecretSanta.Game.RPG
{
	public enum Notifications { BattleStarted, BattleEnded, TurnStarted, TurnEnded }

	public class GameManager : MonoBehaviour
	{
		[SerializeField] private Tilemap _tilemap;

		private void Start()
		{
			var encounter = Resources.Load<Encounter>("Encounters/Encounter1");
			StartEncounter(encounter);
		}

		private async void StartEncounter(Encounter encounter)
		{
			Helpers.LoadArea(encounter.Area, _tilemap, Game.Instance.Config.Tiles);

			var allUnits = new List<UnitComponent>();
			for (var index = 0; index < encounter.Allies.Count; index++)
			{
				allUnits.Add(SpawnUnit(encounter.Allies[index], encounter.Area.AllySpawnPoints[index], true));
			}
			for (var index = 0; index < encounter.Foes.Count; index++)
			{
				allUnits.Add(SpawnUnit(encounter.Foes[index], encounter.Area.FoeSpawnPoints[index]));
			}

			Notification.Send("BattleStarted");
			var battle = new Battle(allUnits);
			await battle.Start();
			Notification.Send("BattleEnded");
		}

		private UnitComponent SpawnUnit(Unit data, Vector2Int position, bool isPlayerControlled = false)
		{
			var unit = Instantiate(Game.Instance.Config.UnitPrefab);
			unit.SetGridPosition(position);
			unit.UpdateData(data, isPlayerControlled);
			return unit;
		}
	}
}
