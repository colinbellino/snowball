using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.SecretSanta.Game.RPG
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private Tilemap _tilemap;

		private void Start()
		{
			var encounter = new Encounter
			{
				Area = Resources.Load<Area>("Areas/BattleArea1"),
				Allies = new List<Unit>{ new Unit { Name = "Ally1" } },
				Foes = new List<Unit>{ new Unit { Name = "Foe1" } },
			};
			StartEncounter(encounter);
		}

		private void StartEncounter(Encounter encounter)
		{
			Helpers.LoadArea(encounter.Area, _tilemap, Game.Instance.Config.Tiles);

			var allyIndex = 0;
			var foeIndex = 0;
			for (var x = 0; x < encounter.Area.Size.x; x++)
			{
				for (var y = 0; y < encounter.Area.Size.y; y++)
				{
					var index = x + y * encounter.Area.Size.x;
					var tileId = encounter.Area.Tiles[index];
					var position = new Vector2Int(x, y);

					if (tileId == 2)
					{
						SpawnUnit(encounter.Allies[allyIndex], position);
						allyIndex += 1;
					}
					if (tileId == 3)
					{
						SpawnUnit(encounter.Foes[foeIndex], position);
						foeIndex += 1;
					}
				}
			}
		}

		private void SpawnUnit(Unit unit, Vector2Int position)
		{
			Debug.Log($"Spawning ({unit.Name}) at {position}");
			var instance = new GameObject();
			instance.transform.position = new Vector3(position.x, position.y);
			instance.name = $"Unit [{unit.Name}]";
		}
	}

	public class Encounter
	{
		public Area Area;
		public List<Unit> Allies;
		public List<Unit> Foes;
	}

	public class Unit
	{
		public string Name;
	}
}
