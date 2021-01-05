using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	[CreateAssetMenu(menuName = "Secret Santa/RPG/Unit")]
	public class UnitAuthoring : ScriptableObject
	{
		public int Id;
		public string Name => name;
		public Color Color = Color.magenta;
		public int MoveRange = 3;
		public int Health = 1;
	}
}
