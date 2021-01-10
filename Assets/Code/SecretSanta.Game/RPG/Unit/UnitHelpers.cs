using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public static class UnitHelpers
	{
		public static UnitFacade SpawnUnitFacade(UnitFacade prefab, Unit unit, Vector3Int position, Unit.Drivers driver, Unit.Alliances alliance, Unit.Directions direction)
		{
			var facade = Object.Instantiate(prefab);
			unit.GridPosition = position;
			unit.Direction = direction;
			unit.Driver = driver;
			unit.Alliance = alliance;
			facade.Initialize(unit);

			return facade;
		}

		public static Unit.Directions VectorToDirection(Vector3 vector)
		{
			return vector.x > 0 ? Unit.Directions.Right : Unit.Directions.Left;
		}
	}
}
