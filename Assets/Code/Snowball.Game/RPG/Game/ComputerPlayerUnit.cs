using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Snowball.Game
{
	public class ComputerPlayerUnit
	{
		public Plan CalculateBestPlan(Unit owner, TurnManager turnManager)
		{
			var plan = new Plan();
			var actionOptions = new List<ActionOption>();
			var moveOptions = GridHelpers.GetWalkableTilesInRange(
				owner.GridPosition, owner.MoveRange,
				turnManager.WalkGrid, turnManager.GetActiveUnits()
			);

			// Choose turn action.
			if (owner.Type == Unit.Types.Snowpal)
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
					var targetOptions = GridHelpers.GetTilesInRange(moveOption, owner.HitRange, turnManager.EmptyGrid);
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
						if (actionOption.AreaTargets.Contains(moveOption) == false)
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
					var score = CalculateScore(option, owner, turnManager);

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
					plan.Action = TurnActions.None;
				}
				else
				{
					var bestOption = bestOptions[Random.Range(0, bestOptions.Count - 1)];

					plan.ActionDestination = bestOption.ActionTarget;
					plan.NeedsToMove = true;
					plan.MoveDestination = bestOption.MoveTarget;
				}
			}

			if (plan.Action == TurnActions.None)
			{
				var foes = turnManager.GetActiveUnits()
					.Where(unit => unit.Alliance != owner.Alliance && unit.Type == Unit.Types.Humanoid)
					.OrderBy(unit => (unit.GridPosition - unit.GridPosition).magnitude)
					.ToList();
				var nearestFoe = foes[0];

				if (nearestFoe != null)
				{
					var walkPath = GridHelpers.FindPath(owner.GridPosition, nearestFoe.GridPosition, turnManager.WalkGrid);
					walkPath.Reverse();
					foreach (var position in walkPath)
					{
						if (moveOptions.Contains(position))
						{
							plan.Action = TurnActions.Wait;
							plan.NeedsToMove = true;
							plan.MoveDestination = position;
							break;
						}
					}
				}
			}

			return plan;
		}

		private static int CalculateScore(ActionOption option, Unit owner, TurnManager turnManager)
		{
			var score = 0;
			var activeUnits = turnManager.GetActiveUnits();

			foreach (var areaTarget in option.AreaTargets)
			{
				var targetUnit = activeUnits.Find(unit => unit.GridPosition == areaTarget);

				if (IsValidTarget(owner, targetUnit))
				{
					var hitChance = GridHelpers.CalculateHitAccuracy(
						option.MoveTarget,
						targetUnit.GridPosition,
						owner,
						turnManager.BlockGrid,
						activeUnits
					);

					if (hitChance > 75)
					{
						score += 1;
					}
					else
					{
						score -= 1;
					}
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
		private static bool IsValidTarget(Unit owner, Unit target)
		{
			return target != null && target.HealthCurrent > 0 && target.Alliance != owner.Alliance;
		}
	}
}
