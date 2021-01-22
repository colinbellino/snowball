using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Snowball.Game
{
	public class ComputerPlayerUnit
	{
		private readonly Database _database;

		public ComputerPlayerUnit(Database database, GameConfig config, StuffSpawner spawner)
		{
			_database = database;
		}

		public Plan CalculateBestPlan(Unit actor, TurnManager turnManager)
		{
			var plan = new Plan();
			var actionOptions = new List<ActionOption>();
			var moveOptions = GridHelpers.GetWalkableTilesInRange(
				actor.GridPosition, actor, actor.MoveRange,
				turnManager.WalkGrid, turnManager.GetActiveUnits()
			);
			moveOptions.Add(actor.GridPosition);

			plan.Ability = ChooseAbility(actor);

			if (plan.Ability != null)
			{
				// This works only for abilities that are movement dependent.
				foreach (var moveOption in moveOptions)
				{
					var targetOptions = plan.Ability.GetTilesInRange(moveOption, actor, turnManager);
					var actionsMap = new Dictionary<Vector3Int, ActionOption>();

					// Loop on the target options.
					foreach (var targetOption in targetOptions)
					{
						ActionOption actionOption;

						if (actionsMap.ContainsKey(targetOption))
						{
							actionOption = actionsMap[targetOption];
						}
						else
						{
							actionOption = new ActionOption();
							actionOption.ActionTarget = targetOption;
							actionOption.AreaTargets = plan.Ability.GetAreaOfEffect(targetOption);
							actionsMap.Add(targetOption, actionOption);
						}

						actionOption.MoveTarget = moveOption;
						actionOption.NeedsToMove = moveOption != actor.GridPosition;
						actionOptions.Add(actionOption);
					}
				}

				// Try to find the best option for the ability.
				{
					var bestOptions = new List<ActionOption>();
					var bestScore = 1;

					foreach (var option in actionOptions)
					{
						var score = CalculateScore(option, plan.Ability, actor, turnManager);

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
						plan.Ability = null;
					}
					else
					{
						var bestOption = bestOptions[Random.Range(0, bestOptions.Count - 1)];

						plan.ActionDestination = bestOption.ActionTarget;
						plan.NeedsToMove = bestOption.NeedsToMove;
						plan.MoveDestination = bestOption.MoveTarget;
					}
				}
			}

			// If we have no ability (no best plan found), run towards the nearest foe.
			if (plan.Ability == null)
			{
				var foes = turnManager.GetActiveUnits()
					.Where(unit => unit.Alliance != actor.Alliance && unit.Type == Unit.Types.Humanoid)
					.OrderBy(unit => (unit.GridPosition - unit.GridPosition).magnitude)
					.ToList();
				var nearestFoe = foes[0];

				if (nearestFoe != null)
				{
					var walkPath = GridHelpers.FindPath(actor.GridPosition, nearestFoe.GridPosition, turnManager.WalkGrid);
					walkPath.Reverse();

					foreach (var position in walkPath)
					{
						if (moveOptions.Contains(position))
						{
							plan.NeedsToMove = position != actor.GridPosition;
							plan.MoveDestination = position;
							break;
						}
					}
				}
			}

			return plan;
		}

		private IAbility ChooseAbility(Unit actor)
		{
			return actor.ActionPatterns switch
			{
				ActionPatterns.Random => _database.Abilities[actor.Abilities[Random.Range(0, actor.Abilities.Length)]],
				ActionPatterns.Idle => null,
				_ => null
			};
		}

		private static int CalculateScore(ActionOption option, IAbility ability, Unit actor, TurnManager turnManager)
		{
			var score = 0;
			var activeUnits = turnManager.GetActiveUnits();

			foreach (var areaTarget in option.AreaTargets)
			{
				var targetUnit = activeUnits.Find(unit => unit.GridPosition == areaTarget);

				if (ability.RequiresUnitTarget)
				{
					if (ability.IsValidTarget(actor, targetUnit))
					{
						var hitChance = GridHelpers.CalculateHitAccuracy(
							option.MoveTarget,
							targetUnit.GridPosition,
							actor,
							turnManager.BlockGrid,
							activeUnits
						);

						if (hitChance > 0)
						{
							score += 1;
						}

						// Bonus score for high precision shots
						if (hitChance > 75)
						{
							score += 1;
						}
					}
					else
					{
						score -= 1;
					}
				}
				else
				{
					score += 1;
				}
			}

			if (option.AreaTargets.Contains(option.MoveTarget))
			{
				score += 1;
			}

			return score;
		}
	}
}
