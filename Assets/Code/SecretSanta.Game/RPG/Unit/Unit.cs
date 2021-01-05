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

		public UnitFacade Facade;

		public Unit(UnitAuthoring authoring)
		{
			Id = authoring.Id;
			Name = authoring.Name;
			Color = authoring.Color;
			MoveRange = authoring.MoveRange;
		}
	}
}
