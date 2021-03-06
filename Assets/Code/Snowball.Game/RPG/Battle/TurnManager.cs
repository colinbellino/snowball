using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Grid = NesScripts.Controls.PathFind.Grid;

namespace Snowball.Game
{
	public enum BattleResults { None, Defeat, Victory }

	public class TurnManager
	{
		private const int _turnActivation = 100;
		private const int _turnCost = 100;
		private const int _moveCost = 20;
		private const int _actionCost = 20;

		private IEnumerator<Turn> _turns;
		private List<Unit> _units { get;  set; }
		private int _turnNumber;

		public Turn Turn => _turns?.Current;
		public Grid DefaultGrid { get; private set; }
		public Grid EmptyGrid { get; private set; }
		public Grid BlockGrid { get; private set; }
		public Grid WalkGrid { get; private set; }
		public Grid BuildGrid { get; private set; }
		public List<Unit> SortedUnits { get; private set; } = new List<Unit>();

		public void Start(List<Unit> units, Area area, Vector2Int gridOffset, TilesData tilesData)
		{
			_turnNumber = 0;
			_units = units;
			_turns = StartTurn();
			SortedUnits = new List<Unit>(_units);
			DefaultGrid = GridHelpers.GenerateGrid(area, gridOffset, tilesData, GridHelpers.AlwaysTrue);
			EmptyGrid = GridHelpers.GenerateGrid(area, gridOffset, tilesData, GridHelpers.IsEmpty);
			BlockGrid = GridHelpers.GenerateGrid(area, gridOffset, tilesData, GridHelpers.IsBlocking);
			WalkGrid = GridHelpers.GenerateGrid(area, gridOffset, tilesData, GridHelpers.IsWalkable);
			BuildGrid = GridHelpers.GenerateGrid(area, gridOffset, tilesData, GridHelpers.IsBuildable);
		}

		public void NextTurn()
		{
			_turns.MoveNext();
		}

		public BattleResults GetBattleResult()
		{
			if (IsVictoryConditionReached())
			{
				return BattleResults.Victory;
			}

			if (IsDefeatConditionReached())
			{
				return BattleResults.Defeat;
			}

			return BattleResults.None;
		}

		private bool IsVictoryConditionReached()
		{
			return GetActiveUnits().Where(unit => unit.Alliance == Unit.Alliances.Foe && unit.Type == Unit.Types.Humanoid).Count() == 0;
		}

		private bool IsDefeatConditionReached()
		{
			return GetActiveUnits().Where(unit => unit.Alliance == Unit.Alliances.Ally && unit.Type == Unit.Types.Humanoid).Count() == 0;
		}

		private IEnumerator<Turn> StartTurn()
		{
			foreach (var unit in _units)
			{
				unit.ChargeTime = 0;
			}

			while (true)
			{
				foreach (var unit in SortedUnits)
				{
					unit.ChargeTime += unit.Speed;
				}
				SortedUnits.Sort(CompareChargeTime);

				// Yield an empty turn just so we can give the hand to the rest of the program for debugging purposes.
				yield return null;

				for (var unitIndex = SortedUnits.Count - 1; unitIndex >= 0; --unitIndex)
				{
					var unit = SortedUnits[unitIndex];

					if (CanTakeTurn(unit))
					{
						Debug.Log($"Turn [{_turnNumber}]: Started ({unit})");
						Notification.Send("TurnStarted");

						var turn = new Turn { Unit = unit };
						yield return turn;

						_turnNumber += 1;

						ConsumeChargeTime(turn);

						Notification.Send("TurnEnded");
					}
				}

				// Debug.Log("round over");
			}
		}

		private void ConsumeChargeTime(Turn turn)
		{
			var cost = _turnCost;
			// if (turn.HasMoved)
			// {
			// 	cost += _moveCost;
			// }
			// if (turn.HasActed)
			// {
			// 	cost += _actionCost;
			// }

			turn.Unit.ChargeTime -= cost;
		}

		private bool CanTakeTurn(Unit unit)
		{
			return IsActive(unit) && unit.ChargeTime >= _turnActivation;
		}

		private bool IsActive(Unit unit)
		{
			return unit.HealthCurrent > 0;
		}

		public List<Unit> GetActiveUnits()
		{
			return SortedUnits.Where(IsActive).ToList();
		}

		public List<Unit> GetTurnOrder()
		{
			// this.SortedUnits is updated only once per round (x turns), so for an accurate turn order we need to recalculate it
			var sortedUnits = new List<Unit>(GetActiveUnits());
			sortedUnits.Sort(CompareChargeTime);
			return sortedUnits;
		}

		private Comparison<Unit> CompareChargeTime = (a, b) => a.ChargeTime.CompareTo(b.ChargeTime);
	}
}
