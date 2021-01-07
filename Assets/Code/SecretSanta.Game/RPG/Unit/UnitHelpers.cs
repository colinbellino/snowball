using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public static class UnitHelpers
	{
		public static UnitFacade SpawnUnitFacade(UnitFacade prefab, Unit unit, Vector3Int position, bool isPlayerControlled)
		{
			var facade = GameObject.Instantiate(prefab);
			unit.GridPosition = position;
			unit.Direction = isPlayerControlled ? Vector3Int.right : Vector3Int.left;
			unit.IsPlayerControlled = isPlayerControlled;
			facade.Initialize(unit);

			return facade;
		}
	}
}
