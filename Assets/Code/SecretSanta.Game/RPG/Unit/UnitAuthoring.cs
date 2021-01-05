using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	[CreateAssetMenu(menuName = "Secret Santa/RPG/Unit")]
	public class UnitAuthoring : ScriptableObject
	{
		public int Id;
		[SerializeField] private string _name;
		public Color Color = Color.magenta;
		public int MoveRange = 3;
		public int Health = 1;

		public string Name => _name;

		private void OnValidate()
		{
			_name = name;
		}
	}
}
