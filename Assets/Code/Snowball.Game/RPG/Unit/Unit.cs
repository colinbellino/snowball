using UnityEngine;

namespace Snowball.Game
{
	public enum ActionPatterns { Random, Idle }
	public enum Abilities { Attack, Build, Melt }

	public class Unit
	{
		public enum Types { Humanoid, Snowpal }
		public enum Alliances { Ally, Foe }
		public enum Directions { Left = -1, Right = 1 }
		public enum Drivers { Human, Computer }

		public int Id;
		public string Name;
		public Sprite Sprite;
		public Color ColorCloth;
		public Color ColorHair;
		public Color ColorSkin;
		public Types Type;
		public int MoveRange;
		public int HealthCurrent;
		public int HealthMax;
		public int ChargeTime;
		public int Speed;
		public int HitAccuracy;
		public int HitRange;
		public int HitDamage;
		public int BuildRange = 1;
		public Vector3Int GridPosition;
		public Directions Direction;
		public Drivers Driver;
		public Alliances Alliance;
		public ActionPatterns ActionPatterns;
		public Abilities[] Abilities;

		public UnitFacade Facade { get; private set; }

		public Unit(UnitAuthoring authoring)
		{
			Id = authoring.Id;
			Name = authoring.Name;
			Sprite = authoring.Sprite;
			ColorCloth = authoring.ColorCloth;
			ColorHair = authoring.ColorHair;
			ColorSkin = authoring.ColorSkin;
			Type = authoring.Type;
			MoveRange = authoring.MoveRange;
			HealthCurrent = authoring.Health;
			HealthMax = authoring.Health;
			Speed = authoring.Speed;
			HitAccuracy = authoring.HitAccuracy;
			HitRange = authoring.HitRange;
			HitDamage = authoring.HitDamage;
			ActionPatterns = authoring.ActionPatterns;
			Abilities = authoring.Abilities;
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
