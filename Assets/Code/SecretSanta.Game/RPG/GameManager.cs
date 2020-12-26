using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
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
			var turnEnumerator = battle.Start();

			Game.Instance.Controls.Enable();

			while (true)
			{
				turnEnumerator.MoveNext();
				var turn = turnEnumerator.Current;
				var unit = turn.Unit;

				while (turn.IsOver() == false)
				{
					if (unit.IsPlayerControlled)
					{
						var startPosition = unit.GridPosition;
						var maxDistance = 5;
						var maxClimpHeight = 1;

						var moveInput = Game.Instance.Controls.Gameplay.Move.ReadValue<Vector2>();
						var confirmInput = Game.Instance.Controls.Gameplay.Confirm.ReadValue<float>() > 0f;

						if (moveInput.magnitude > 0f)
						{
							var direction = Helpers.InputToDirection(moveInput);

							if (direction.y == 0 && direction.x != unit.Direction.x)
							{
								await unit.Turn(direction);
							}
							else
							{
								for (var y = 0; y <= maxClimpHeight; y++)
								{
									var destination = unit.GridPosition + direction + Vector3Int.up * y;
									if (Helpers.IsInRange(startPosition, destination, maxDistance) == false)
									{
										break;
									}

									if (Helpers.CanMoveTo(destination, _tilemap))
									{
										var path = Helpers.CalculatePathWithFall(unit.GridPosition, destination, _tilemap);
										await unit.MoveOnPath(path);
										turn.HasMoved = true;
										break;
									}
								}
							}
						}

						if (confirmInput)
						{
							var result = Helpers.CalculateAttackResult(unit.GridPosition, unit.Direction.x, allUnits, _tilemap);
							await unit.Attack(result);
							turn.HasActed = true;
						}
					}
					else
					{
						await UniTask.Delay(300);
						var randomDirection = Random.Range(0, 2) > 0 ? 1 : -1;
						var result = Helpers.CalculateAttackResult(unit.GridPosition, randomDirection, allUnits, _tilemap);
						await unit.Attack(result);
						turn.HasActed = true;
						turn.HasMoved = true;
					}

					await UniTask.NextFrame();
				}

				if (battle.IsBattleOver())
				{
					break;
				}
			}

			Notification.Send("BattleEnded");
			Debug.LogWarning("Battle over!");
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
