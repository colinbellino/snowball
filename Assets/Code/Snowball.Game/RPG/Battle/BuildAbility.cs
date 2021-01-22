using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Snowball.Game
{
	public class BuildAbility : IAbility
	{
		private readonly GameConfig _config;
		private readonly Database _database;

		public BuildAbility(GameConfig config, Database database)
		{
			_config = config;
			_database = database;
		}

		public bool RequiresUnitTarget => false;

		public bool IsValidTarget(Unit actor, Unit target) => true;

		public List<Vector3Int> GetAreaOfEffect(Vector3Int destination)
		{
			return new List<Vector3Int> { destination };
		}

		public List<Vector3Int> GetTilesInRange(Vector3Int origin, Unit actor, TurnManager turnManager)
		{
			return GridHelpers.GetWalkableTilesInRange(
				origin, actor, actor.BuildRange,
				turnManager.WalkGrid, turnManager.GetActiveUnits()
			);
		}

		public async UniTask Perform(TurnManager turnManager)
		{
			var actor = turnManager.Turn.Unit;
			var plan = turnManager.Turn.Plan;

			var direction = UnitHelpers.VectorToDirection(plan.ActionDestination - actor.GridPosition);

			var needsToChangeDirection = direction != actor.Direction;
			if (needsToChangeDirection)
			{
				await actor.Facade.AnimateChangeDirection(direction);
				actor.Direction = direction;
			}
			await actor.Facade.AnimateBuild(direction);

			var newUnit = new Unit(_database.Units[_config.SnowmanUnitId]);
			var facade = UnitHelpers.SpawnUnitFacade(
				_config.UnitPrefab,
				newUnit,
				plan.ActionDestination,
				Unit.Drivers.Computer,
				actor.Alliance,
				direction
			);
			newUnit.SetFacade(facade);

			await newUnit.Facade.AnimateSpawn();

			turnManager.SortedUnits.Add(newUnit);
		}
	}
}
