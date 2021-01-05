using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class Unit
	{
		public int Id;
		public string Name;
		public Color Color;
		public int MoveRange;
		public Vector3Int GridPosition;
		public Vector3Int Direction;
		public bool IsPlayerControlled;

		public UnitFacade Facade { get; private set; }

		public Unit(UnitAuthoring authoring)
		{
			Id = authoring.Id;
			Name = authoring.Name;
			Color = authoring.Color;
			MoveRange = authoring.MoveRange;
		}

		public void SetFacade(UnitFacade facade)
		{
			Facade = facade;
		}
	}

	public static class UnitHelpers
	{
		public static void SetGridPosition(Unit unit, Vector3Int position)
		{
			unit.GridPosition = position;
		}
	}
}
