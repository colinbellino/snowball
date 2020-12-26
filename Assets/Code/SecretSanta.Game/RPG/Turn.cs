namespace Code.SecretSanta.Game.RPG
{
	public class Turn
	{
		public UnitComponent Unit;
		public bool HasMoved;
		public bool HasActed;

		public bool IsOver() => HasMoved && HasActed;
	}
}
