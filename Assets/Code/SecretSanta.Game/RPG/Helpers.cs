using System;
using System.Collections.Generic;
using System.Linq;
using NesScripts.Controls.PathFind;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Grid = NesScripts.Controls.PathFind.Grid;

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

		public static List<Vector3Int> CalculatePathWithFall(Vector3Int start, Vector3Int destination, Tilemap tilemap, Vector2Int tilemapSize)
		{
			// FIXME: This is super slow and should be cached!
			var data = new float[tilemapSize.x, tilemapSize.y];
			for (var x = 0; x < tilemapSize.x; x++)
			{
				for (var y = 0; y < tilemapSize.y; y++)
				{
					var tile = tilemap.GetTile(new Vector3Int(x, y, 0));
					data[x, y] = CanMoveOnTile(tile) ? 1f : 0f;
				}
			}
			var grid = new Grid(data);

			var points = Pathfinding.FindPath(grid, start.ToPoint(), destination.ToPoint());

			return points
				.Select(point => new Vector3Int(point.x, point.y, 0))
				.ToList();
		}

		private static Vector3Int GetLandingPoint(Vector3Int start, Tilemap tilemap)
		{
			for (var y = start.y - 1; y > 0; y--)
			{
				var tile = tilemap.GetTile(new Vector3Int(start.x, y, 0));
				if (tile != null)
				{
					return new Vector3Int(start.x, y + 1, 0);
				}
			}

			return start;
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

		public static Vector3Int GetCursorPosition(Vector2 mousePosition, Vector2Int tilemapSize)
		{
			var position = new Vector3Int(
				Mathf.RoundToInt(mousePosition.x).Clamp(0, tilemapSize.x - 1),
				Mathf.RoundToInt(mousePosition.y).Clamp(0, tilemapSize.y - 1),
				0
			);

			return position;
		}
	}

	public class AttackResult
	{
		public UnitComponent Attacker;
		public Vector3Int Destination;
		public List<UnitComponent> Targets;
	}
}
