using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Grid = NesScripts.Controls.PathFind.Grid;

namespace Code.SecretSanta.Game.RPG
{
	public class Battle
	{
		public List<Unit> Units { get; private set; }
		public int TurnNumber { get; private set; }
		public Turn Turn => _turns.Current;
		public Grid WalkGrid { get; private set; }

		private IEnumerator<Turn> _turns;

		public void Start(List<Unit> units, Area area, TilesData tilesData)
		{
			TurnNumber = 0;
			Units = units;
			_turns = StartTurn();
			WalkGrid = Helpers.GetWalkGrid(area, tilesData);
		}

		public void NextTurn()
		{
			_turns.MoveNext();
		}

		private IEnumerator<Turn> StartTurn()
		{
			while (true)
			{
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

				// Debug.Log("round over");
			}
		}

		private bool CanTakeTurn(Unit unit) => unit.HealthCurrent > 0;

		public List<Unit> GetActiveUnits()
		{
			return Units.Where(CanTakeTurn).ToList();
		}

		public List<Unit> GetTurnOrder()
		{
			return GetActiveUnits();
		}
	}
}
