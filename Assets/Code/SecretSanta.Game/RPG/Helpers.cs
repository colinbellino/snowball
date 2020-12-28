using System.Collections.Generic;
using System.Linq;
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

					tilemap.SetTile((Vector3Int)position, tiles[area.Tiles[index]]);
				}
			}
		}

		public static void LoadMeta(Area area, Tilemap tilemap)
		{
			foreach (var position in area.AllySpawnPoints)
			{
				tilemap.SetTile(new Vector3Int(position.x, position.y, 0), Game.Instance.Config.AllySpawnTile);
			}
			foreach (var position in area.FoeSpawnPoints)
			{
				tilemap.SetTile(new Vector3Int(position.x, position.y, 0), Game.Instance.Config.FoeSpawnTile);
			}
		}

		public static List<Vector3Int> CalculatePathWithFall(Vector3Int start, Vector3Int destination, Tilemap tilemap)
		{
			var path = new List<Vector3Int>();

			if (start.x != destination.x)
			{
				for (var y = start.y; y <= destination.y; y++)
				{
					path.Add(new Vector3Int(start.x, y, 0));
				}
			}

			path.Add(destination);

			for (var y = destination.y - 1; y > 0; y--)
			{
				var tile = tilemap.GetTile(new Vector3Int(destination.x, y, 0));
				if (tile != null)
				{
					path.Add(new Vector3Int(destination.x, y + 1, 0));
					break;
				}
			}

			return path;
		}

		public static bool CanMoveTo(Vector3Int destination, Tilemap tilemap)
		{
			if (tilemap.cellBounds.Contains(destination) == false)
			{
				return false;
			}

			var tile = tilemap.GetTile(destination);
			return CanMoveOnTile(tile);
		}

		public static bool CanMoveOnTile(TileBase tile)
		{
			if (tile == null)
			{
				return true;
			}

			return Game.Instance.Config.WalkableTiles.Contains(tile);
		}

		public static bool CanBlockProjectile(TileBase tile)
		{
			if (tile == null)
			{
				return false;
			}

			return Game.Instance.Config.BlockingTiles.Contains(tile);
		}

		// TODO: Replace this by actual path finding
		public static bool IsInRange(Vector3Int position, Vector3Int destination, int maxDistance)
		{
			var distance = Mathf.Abs(position.x - destination.x) + Mathf.Abs(position.y - destination.y);
			return distance <= maxDistance;
		}

		public static Vector3Int InputToDirection(Vector2 moveInput)
		{
			var direction = new Vector3Int();
			if (moveInput.x != 0)
			{
				direction.x = moveInput.x > 0 ? 1 : -1;
			}
			if (moveInput.y != 0)
			{
				direction.y = moveInput.y > 0 ? 1 : -1;
			}

			return direction;
		}

		// public static AttackResult CalculateAttackResult(Vector3Int start, int direction, List<UnitComponent> allUnits, Tilemap tilemap)
		// {
		// 	var result = new AttackResult { Direction = direction };
		//
		// 	for (
		// 		var x = start.x;
		// 		direction > 0 ? (x <= tilemap.size.x) : (x >= 0);
		// 		x += direction)
		// 	{
		// 		var position = new Vector3Int(x, start.y, 0);
		//
		// 		var unit = allUnits.Find(unit => unit.GridPosition == position);
		// 		if (unit)
		// 		{
		// 			if (position == start)
		// 			{
		// 				result.Attacker = unit;
		// 			}
		// 			else
		// 			{
		// 				result.Target = unit;
		// 				result.Destination = position;
		// 				break;
		// 			}
		// 		}
		//
		// 		if (x == tilemap.size.x || x == 0)
		// 		{
		// 			result.Destination = position;
		// 			break;
		// 		}
		//
		// 		var tile = tilemap.GetTile(position);
		// 		if (Game.Instance.Config.BlockingTiles.Contains(tile))
		// 		{
		// 			result.Destination = position;
		// 			break;
		// 		}
		// 	}
		//
		// 	return result;
		// }
	}

	public class AttackResult
	{
		public UnitComponent Attacker;
		public Vector3Int Destination;
		public List<UnitComponent> Targets;
	}
}
