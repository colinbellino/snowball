using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	[CreateAssetMenu(menuName = "Secret Santa/RPG/Unit")]
	public class Unit : ScriptableObject
	{
		public string Name => name;
		public Color Color = Color.magenta;
	}
}
