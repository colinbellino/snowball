using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class Unit
	{
		public enum Types { Humanoid, Snowpal }
		public enum Directions { Left = -1, Right = 1 }

		public int Id;
		public string Name;
		public Sprite Sprite;
		public Color Color;
		public Types Type;
		public int MoveRange;
		public int HealthCurrent;
		public int HealthMax;
		public int ChargeTime;
		public int Speed;
		public int HitAccuracy;
		public int HitRange;
		public int HitDamage;
		public Vector3Int GridPosition;
		public Directions Direction;
		public bool IsPlayerControlled;

		public UnitFacade Facade { get; private set; }

		public Unit(UnitAuthoring authoring)
		{
			Id = authoring.Id;
			Name = authoring.Name;
			Sprite = authoring.Sprite;
			Color = authoring.Color;
			Type = authoring.Type;
			MoveRange = authoring.MoveRange;
			HealthCurrent = authoring.Health;
			HealthMax = authoring.Health;
			Speed = authoring.Speed;
			HitAccuracy = authoring.HitAccuracy;
			HitRange = authoring.HitRange;
			HitDamage = authoring.HitDamage;
		}

		public void SetFacade(UnitFacade facade)
		{
			Facade = facade;
		}

		public void DestroyFacade()
		{
			GameObject.Destroy(Facade.gameObject);
			Facade = null;
		}

		public override string ToString() => Name;
	}
}
