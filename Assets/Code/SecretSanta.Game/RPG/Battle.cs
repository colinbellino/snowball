using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.SecretSanta.Game.RPG
{
	public class Battle
	{
		private readonly List<UnitComponent> _units;
		private int _turnNumber;

		public Battle(List<UnitComponent> units)
		{
			_units = units;
			_turnNumber = 0;
		}

		/*
		while round not over
		    notify(roundBegan)
		    sort units by speed
		    foreach unit
		        if canTakeTurn(unit)
		            notify(turnBegan)
		            unitTurn(unit)
		            reduceCT(unit)
		            notify(turnEnded)
		    notify(roundEnded)
		*/
		public async Task Start()
		{
			while (true)
			{
				// TODO: Sort by speed
				var units = _units;
				foreach (var unit in units)
				{
					if (CanTakeTurn(unit))
					{
						Debug.Log($"Turn [{_turnNumber}]: Started");
						Notification.Send("TurnStarted");
						// ChangeTurn(unit);

						if (unit.IsPlayerControlled)
						{
							await PlayerTurn(unit);
						}
						else
						{
							Debug.Log($"Turn [{_turnNumber}]: Computer...");
							await UniTask.Delay(500);
							await unit.Attack(units[0]);
						}

						_turnNumber += 1;

						// ConsumeCT();

						Notification.Send("TurnEnded");

						if (IsBattleOver())
						{
							return;
						}
					}
				}
			}
		}

		private async Task PlayerTurn(UnitComponent unit)
		{
			Debug.Log($"Turn [{_turnNumber}]: Player...");

			var foes = _units.Where(u => u.IsPlayerControlled == false);
			var randomFoe = foes.OrderBy(qu => Guid.NewGuid()).First();

			Task task = null;
			while (task == null)
			{
				await UniTask.NextFrame();

				if (Keyboard.current.leftArrowKey.wasReleasedThisFrame)
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
			}

			await task;
		}

		private bool IsBattleOver()
		{
			return _turnNumber > 5;
		}

		private bool CanTakeTurn(UnitComponent unit) => true;
	}
}
