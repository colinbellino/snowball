using System.Collections.Generic;
using UnityEngine;
using Grid = NesScripts.Controls.PathFind.Grid;

namespace Code.SecretSanta.Game.RPG
{
	public class Battle
	{
		public List<UnitRuntime> Units { get; private set; }
		public int TurnNumber { get; private set; }
		public Turn Turn => _turns.Current;
		public Grid WalkGrid { get; private set; }

		private IEnumerator<Turn> _turns;

		public void Start(List<UnitRuntime> units, Area area, TilesData tilesData)
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

		private bool CanTakeTurn(UnitRuntime unit) => true;
	}
}
