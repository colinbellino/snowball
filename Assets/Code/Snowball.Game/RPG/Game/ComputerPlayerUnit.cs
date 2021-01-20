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
							actionOption.AreaTargets = new List<Vector3Int> { moveOption };
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

			if (actionOptions.Count == 0)
			{
				return plan;
			}

			var bestOption = PickBestOption(actionOptions);

			plan.ActionDestination = bestOption.ActionTarget;
			plan.MoveDestination = bestOption.MoveTarget;

			return plan;
		}

		private static ActionOption PickBestOption(List<ActionOption> options)
		{
			return options[UnityEngine.Random.Range(0, options.Count - 1)];
		}
	}
}
