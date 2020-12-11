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
		public IEnumerator<UnitComponent> Start()
		{
			while (true)
			{
				// TODO: Sort by speed
				var units = _units;
				foreach (var unit in units)
				{
					if (CanTakeTurn(unit))
					{
						Debug.Log($"Turn [{_turnNumber}]: Started ({unit})");
						Notification.Send("TurnStarted");

						yield return unit;

						_turnNumber += 1;

						// ConsumeCT();

						Notification.Send("TurnEnded");
					}
				}
			}
		}

		public bool IsBattleOver()
		{
			return _turnNumber > 5;
		}

		private bool CanTakeTurn(UnitComponent unit) => true;
	}
}
