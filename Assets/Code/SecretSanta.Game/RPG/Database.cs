using System.Collections.Generic;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class Database
	{
		public Dictionary<int, UnitAuthoring> Units;
		public Dictionary<int, EncounterAuthoring> Encounters;
	}

	public static class DatabaseHelpers
	{
		public static void LoadFromResources(Database database)
		{
			database.Units = new Dictionary<int, UnitAuthoring>();
			var units = Resources.LoadAll<UnitAuthoring>("Units");
			foreach (var unit in units)
			{
				database.Units.Add(unit.Id,  Object.Instantiate(unit));
			}

			database.Encounters = new Dictionary<int, EncounterAuthoring>();
			var encounters = Resources.LoadAll<EncounterAuthoring>("Encounters");
			foreach (var encounter in encounters)
			{
				database.Encounters.Add(encounter.Id, Object.Instantiate(encounter));
			}
		}
	}
}
