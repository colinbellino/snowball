using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class Unit
	{
		public int Id;
		public string Name;
		public Color Color;
		public int MoveRange;
		public int HealthCurrent;
		public int HealthMax;
		public int ChargeTime;
		public int Speed;
		public Sprite Sprite;
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
			HealthCurrent = authoring.Health;
			HealthMax = authoring.Health;
			Speed = authoring.Speed;
			Sprite = authoring.Sprite;
		}

		public void SetFacade(UnitFacade facade)
		{
			Facade = facade;
		}

		public override string ToString() => Name;
	}
}
