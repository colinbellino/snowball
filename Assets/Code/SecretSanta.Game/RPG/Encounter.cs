using System.Collections.Generic;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	[CreateAssetMenu(menuName = "Secret Santa/RPG/Encounter")]
	public class Encounter: ScriptableObject
	{
		public Area Area;
		public List<Unit> Allies;
		public List<Unit> Foes;
	}
}
