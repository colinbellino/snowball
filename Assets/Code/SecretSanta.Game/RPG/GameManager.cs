using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

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
				var point = encounter.Area.AllySpawnPoints[index];
				allUnits.Add(SpawnUnit(encounter.Allies[index], new Vector3Int(point.x, point.y, 0), true));
			}
			for (var index = 0; index < encounter.Foes.Count; index++)
			{
				var point = encounter.Area.FoeSpawnPoints[index];
				allUnits.Add(SpawnUnit(encounter.Foes[index], new Vector3Int(point.x, point.y, 0)));
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
					Task task = null;
					while (task == null)
					{
						if (Keyboard.current.leftArrowKey.wasReleasedThisFrame || Keyboard.current.rightArrowKey.wasReleasedThisFrame)
						{
							var direction = new Vector3Int(Keyboard.current.leftArrowKey.wasReleasedThisFrame ? -1 : 1, 0, 0);
							var destination = unit.GridPosition + direction;
							if (Helpers.CanMove(destination, _tilemap))
							{
								var path = Helpers.GetFallPath(destination, _tilemap);
								task = unit.MoveOnPath(path);
							}
						}

						if (Keyboard.current.spaceKey.wasReleasedThisFrame)
						{
							var result = Helpers.CalculateAttackResult(unit.GridPosition, 1, allUnits, _tilemap);
							task = unit.Attack(result);
							break;
						}

						await UniTask.NextFrame();
					}

					await task;
				}
				else
				{
					await UniTask.Delay(300);
					var randomDirection = Random.Range(0, 1) > 0 ? 1 : -1;
					var result = Helpers.CalculateAttackResult(unit.GridPosition, randomDirection, allUnits, _tilemap);
					await unit.Attack(result);
				}

				if (battle.IsBattleOver())
				{
					break;
				}
			}

			Notification.Send("BattleEnded");
		}

		private UnitComponent SpawnUnit(Unit data, Vector3Int position, bool isPlayerControlled = false)
		{
			var unit = Instantiate(Game.Instance.Config.UnitPrefab);
			unit.SetGridPosition(position);
			unit.UpdateData(data, isPlayerControlled);
			return unit;
		}
	}
}
