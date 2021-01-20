using System.Collections.Generic;
using UnityEngine;

namespace Snowball.Game
{
	public class ComputerPlayerUnit
	{
		public Plan CalculateBestPlan(Unit unit, TurnManager turnManager)
		{
			var plan = new Plan();
			var actionOptions = new List<ActionOption>();
			var moveOptions = GridHelpers.GetWalkableTilesInRange(
				unit.GridPosition, unit.MoveRange,
				turnManager.WalkGrid, turnManager.GetActiveUnits()
			);

			// Choose turn action.
			if (unit.Type == Unit.Types.Snowpal)
			{
				plan.Action = TurnActions.Melt;
			}
			else
			{
				plan.Action = TurnActions.Attack;
			}

			var isMoveDependent = plan.Action == TurnActions.Attack;
			if (isMoveDependent)
			{
				foreach (var moveOption in moveOptions)
				{
					// TODO: get this from ability range
					var targetOptions = GridHelpers.GetTilesInRange(unit.GridPosition, unit.HitRange, turnManager.EmptyGrid);
					var actionsMap = new Dictionary<Vector3Int, ActionOption>();

					// Loop on the target options.
					foreach (var actionTarget in targetOptions)
					{
						ActionOption actionOption;

						if (actionsMap.ContainsKey(actionTarget))
						{
							actionOption = actionsMap[actionTarget];
						}
						else
						{
							actionOption = new ActionOption();
							actionOption.ActionTarget = actionTarget;
							actionOption.AreaTargets = new List<Vector3Int> { actionTarget };
							actionsMap.Add(actionTarget, actionOption);
						}

						// Don't allow moving to a tile that would negatively affect the caster.
						if (actionOption.AreaTargets.Contains(unit.GridPosition) == false)
						{
							actionOption.MoveTarget = moveOption;
						}

						actionOptions.Add(actionOption);
					}
				}
			}

			{
				var bestOptions = new List<ActionOption>();
				var bestScore = 1;

				foreach (var option in actionOptions)
				{
					var score = CalculateScore(option, unit, turnManager.SortedUnits);

					if (score > bestScore)
					{
						bestScore = score;
						bestOptions.Clear();
						bestOptions.Add(option);
					}
					else if (score == bestScore)
					{
						bestOptions.Add(option);
					}
				}

				if (bestOptions.Count == 0)
				{
					// TODO: Move towards target if can't act from current position.
					plan.Action = TurnActions.Wait;
					return plan;
				}

				var bestOption = bestOptions[Random.Range(0, bestOptions.Count - 1)];

				plan.ActionDestination = bestOption.ActionTarget;
				plan.MoveDestination = bestOption.MoveTarget;
			}

			return plan;
		}

		private static int CalculateScore(ActionOption option, Unit unit, List<Unit> units)
		{
			var score = 0;

			foreach (var areaTarget in option.AreaTargets)
			{
				var targetUnit = units.Find(u => u.GridPosition == areaTarget);
				if (IsValidTarget(unit, targetUnit))
				{
					score += 1;
				}
				else
				{
					score -= 1;
				}
			}

			if (option.AreaTargets.Contains(option.MoveTarget))
			{
				score += 1;
			}

			return score;
		}

		// TODO: use ability target for this
		private static bool IsValidTarget(Unit unit, Unit target)
		{
			return target != null && target.HealthCurrent > 0 && target.Alliance != unit.Alliance;
		}
	}
}
