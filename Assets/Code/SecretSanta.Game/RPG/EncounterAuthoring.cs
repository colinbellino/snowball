using System.Collections.Generic;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	[CreateAssetMenu(menuName = "Secret Santa/RPG/Encounter")]
	public class EncounterAuthoring: ScriptableObject
	{
		public int Id;
		public Area Area;
		public List<UnitAuthoring> Foes;
	}
}
