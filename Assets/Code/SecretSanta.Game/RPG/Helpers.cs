using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.SecretSanta.Game.RPG
{
	public static class Helpers
	{
		public static TileBase GetTile(int index, TileBase[] tiles)
		{
			return tiles[index];
		}

		public static int GetTileId(TileBase tile, TileBase[] tiles)
		{
			for (var index = 0; index < tiles.Length; index++)
			{
				if (tiles[index] == tile)
				{
					return index;
				}
			}

			return 0;
		}

		public static void LoadArea(Area area, Tilemap tilemap, TileBase[] tiles)
		{
			for (var x = 0; x < area.Size.x; x++)
			{
				for (var y = 0; y < area.Size.y; y++)
				{
					var index = x + y * area.Size.x;
					var position = new Vector2Int(x, y);

					tilemap.SetTile((Vector3Int) position, tiles[area.Tiles[index]]);
				}
			}
		}

		public static void LoadMeta(Area area, Tilemap tilemap, TileBase[] tiles)
		{
			foreach (var position in area.AllySpawnPoints)
			{
				tilemap.SetTile(new Vector3Int(position.x, position.y, 0), GetTile(2, tiles));
			}
			foreach (var position in area.FoeSpawnPoints)
			{
				tilemap.SetTile(new Vector3Int(position.x, position.y, 0), GetTile(3, tiles));
			}
		}

		public static List<Vector3Int> GetFallPath(Vector3Int start, Tilemap tilemap)
		{
			var path = new List<Vector3Int> { start };
			for (var y = start.y; y > 0; y--)
			{
				var tile = tilemap.GetTile(new Vector3Int(start.x, y, 0));
				if (tile != null)
				{
					path.Add(new Vector3Int(start.x, y + 1, 0));
					break;
				}
			}

			return path;
		}

		public static bool CanMove(Vector3Int destination, Tilemap tilemap)
		{
			var tile = tilemap.GetTile(new Vector3Int(destination.x, destination.y, 0));
			return tile == null;
		}

		public static AttackResult CalculateAttackResult(Vector3Int start, int direction, List<UnitComponent> allUnits, Tilemap tilemap)
		{
			var result = new AttackResult { Direction = direction };

			for (
				var x = start.x;
				direction > 0 ? (x <= tilemap.size.x) : (x >= 0);
				x += direction)
			{
				var position = new Vector3Int(x, start.y, 0);

				var unit = allUnits.Find(unit => unit.GridPosition == position);
				if (unit)
				{
					if (position == start)
					{
						result.Attacker = unit;
					}
					else
					{
						result.Target = unit;
						result.Destination = position;
						break;
					}
				}

				var tile = tilemap.GetTile(position);
				if (tile != null || x == tilemap.size.x || x == 0)
				{
					result.Destination = position;
					break;
				}
			}
			return result;
		}
	}

	public class AttackResult
	{
		public UnitComponent Attacker;
		public int Direction;
		public Vector3Int Destination;
		public UnitComponent Target;
	}
}
