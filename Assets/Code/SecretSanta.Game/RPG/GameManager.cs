using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace Code.SecretSanta.Game.RPG
{
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
			var turn = battle.Start();

			while (true)
			{
				turn.MoveNext();
				var unit = turn.Current;

				if (unit.IsPlayerControlled)
				{
					var foes = allUnits.Where(u => u.IsPlayerControlled == false);
					var randomFoe = foes.OrderBy(qu => Guid.NewGuid()).First();

					Task task = null;
					while (task == null)
					{
						if (Keyboard.current.leftArrowKey.wasReleasedThisFrame || Keyboard.current.rightArrowKey.wasReleasedThisFrame)
						{
							var destination = unit.GridPosition + new Vector2Int(-1, 0);
							task = unit.Move(destination);
							break;
						}

						if (Keyboard.current.rightArrowKey.wasReleasedThisFrame)
						{
							var destination = unit.GridPosition + new Vector2Int(1, 0);
							task = unit.Move(destination);
							break;
						}

						if (Keyboard.current.spaceKey.wasReleasedThisFrame)
						{
							var target = randomFoe;
							task = unit.Attack(target);
							break;
						}

						await UniTask.NextFrame();
					}

					await task;
				}
				else
				{
					var allies = allUnits.Where(u => u.IsPlayerControlled == true);
					var randomAlly = allies.OrderBy(qu => Guid.NewGuid()).First();

					await UniTask.Delay(300);
					await unit.Attack(randomAlly);
				}

				if (battle.IsBattleOver())
				{
					break;
				}
			}

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
