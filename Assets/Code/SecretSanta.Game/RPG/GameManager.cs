using System.Collections.Generic;
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
			StartEncounter(Game.Instance.Config.Encounters[0]);
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

			Game.Instance.Controls.Enable();

			while (true)
			{
				turn.MoveNext();
				var unit = turn.Current;

				if (unit.IsPlayerControlled)
				{
					await PlayerTurn(unit, allUnits, _tilemap);
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
			Debug.LogWarning("Battle over!");
		}

		private static async UniTask PlayerTurn(UnitComponent unit, List<UnitComponent> allUnits, Tilemap tilemap)
		{
			var didAct = false;
			var startPosition = unit.GridPosition;

			while (didAct == false)
			{
				var moveInput = Game.Instance.Controls.Gameplay.Move.ReadValue<Vector2>();
				var confirmInput = Game.Instance.Controls.Gameplay.Confirm.ReadValue<float>() > 0f;

				if (moveInput.magnitude > 0f)
				{
					var direction = Helpers.InputToDirection(moveInput);
					var destination = unit.GridPosition + direction;

					if (direction != unit.Direction)
					{
						await unit.Turn(direction);
					}
					else
					{
						if (Helpers.IsInRange(startPosition, destination, maxDistance: 3) && Helpers.CanMoveTo(destination, tilemap))
						{
							var path = Helpers.GetFallPath(destination, tilemap);
							await unit.MoveOnPath(path);
						}
					}
				}

				if (confirmInput)
				{
					var result = Helpers.CalculateAttackResult(unit.GridPosition, 1, allUnits, tilemap);
					await unit.Attack(result);
					didAct = true;
				}

				await UniTask.NextFrame();
			}
		}

		private UnitComponent SpawnUnit(Unit data, Vector3Int position, bool isPlayerControlled = false)
		{
			var unit = Instantiate(Game.Instance.Config.UnitPrefab);
			unit.SetGridPosition(position);
			_ = unit.Turn(isPlayerControlled ? Vector3Int.right : Vector3Int.left);
			unit.UpdateData(data, isPlayerControlled);
			return unit;
		}
	}
}
