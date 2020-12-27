using System.Collections.Generic;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class Battle
	{
		public List<UnitComponent> Units { get; private set; }
		public int TurnNumber { get; private set; }
		public Turn Turn => _turns.Current;

		private IEnumerator<Turn> _turns;

		public void Start(List<UnitComponent> units)
		{
			TurnNumber = 0;
			Units = units;
			_turns = StartTurn();
		}

		public void NextTurn()
		{
			_turns.MoveNext();
		}

		private IEnumerator<Turn> StartTurn()
		{
			while (true)
			{
				// TODO: Sort by speed
				foreach (var unit in Units)
				{
					if (CanTakeTurn(unit))
					{
						Debug.Log($"Turn [{TurnNumber}]: Started ({unit})");
						Notification.Send("TurnStarted");

						var turn = new Turn { Unit = unit, InitialPosition = unit.GridPosition };
						yield return turn;

						TurnNumber += 1;

						// ConsumeCT();

						Notification.Send("TurnEnded");
					}
				}
			}
		}

		private bool CanTakeTurn(UnitComponent unit) => true;
	}
}
