using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace Snowball.Game
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
				if (database.Units.ContainsKey(unit.Id))
				{
					throw new DataException($"Database already contains a unit with the id '{unit.Id}'.");
				}
				database.Units.Add(unit.Id,  Object.Instantiate(unit));
			}

			database.Encounters = new Dictionary<int, EncounterAuthoring>();
			var encounters = Resources.LoadAll<EncounterAuthoring>("Encounters");
			foreach (var encounter in encounters)
			{
				if (database.Encounters.ContainsKey(encounter.Id))
				{
					throw new DataException($"Database already contains an encounter with the id '{encounter.Id}'.");
				}
				database.Encounters.Add(encounter.Id, Object.Instantiate(encounter));
			}
		}
	}
}
