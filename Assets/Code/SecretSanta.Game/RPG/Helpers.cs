using System.Collections.Generic;
using System.Linq;
using NesScripts.Controls.PathFind;
using UnityEngine;
using UnityEngine.Tilemaps;
using Grid = NesScripts.Controls.PathFind.Grid;

namespace Code.SecretSanta.Game.RPG
{
	public static class Helpers
	{
		public static void RenderArea(Area area, Tilemap tilemap, TilesData tilesData)
		{
			for (var x = 0; x < area.Size.x; x++)
			{
				for (var y = 0; y < area.Size.y; y++)
				{
					var index = x + y * area.Size.x;
					var position = new Vector3Int(x, y, 0);
					var tileId = area.Tiles[index];

					tilemap.SetTile(position, tilesData[tileId].Tile);
				}
			}
		}

		public static void RenderMeta(Area area, Tilemap tilemap)
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

		public static void RenderGridWalk(Grid grid, Tilemap tilemap, TileBase tile)
		{
			foreach (var node in grid.nodes)
			{
				var position = new Vector3Int(node.gridX, node.gridY, 0);
				var color = Color.clear;
				switch (node.price)
				{
					case 0.5f:
						color = Color.red; break;
					case 1f:
						color = Color.blue; break;
				}

				tilemap.SetTile(position, tile);
				tilemap.SetColor(position, color);
			}
		}

		public static int GetTileIndex(TileBase tile, TilesData tilesData)
		{
			for (var index = 0; index < tilesData.Count; index++)
			{
				if (tilesData[index].Tile == tile)
				{
					return index;
				}
			}

			return 0;
		}

		public static Grid GetWalkGrid(Area area, TilesData tilesData)
		{
			var data = new bool[area.Size.x, area.Size.y];
			for (var x = 0; x < area.Size.x; x++)
			{
				for (var y = 0; y < area.Size.y; y++)
				{
					var index = x + y * area.Size.x;
					var tileId = area.Tiles[index];
					var tileData = tilesData[tileId];

					if (tileData.Climbable)
					{
						data[x, y] = true;
						continue;
					}

					if (tileData.Blocking)
					{
						data[x, y] = false;
						continue;
					}

					if (y > 0)
					{
						var belowIndex = x + (y - 1) * area.Size.x;
						var belowTileId = area.Tiles[belowIndex];

						if (tilesData[belowTileId].Walkable)
						{
							data[x, y] = true;
						}
					}
				}
			}

			return new Grid(data);
		}

		public static List<Vector3Int> CalculatePathWithFall(Vector3Int start, Vector3Int destination, Grid walkGrid)
		{
			var path = Pathfinding.FindPath(walkGrid, start, destination);
			return path.Prepend(start).ToList();
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

		public static bool CanMoveTo(Vector3Int destination, Grid walkGrid)
		{
			return true;
			// if (tilemap.cellBounds.Contains(destination) == false)
			// {
			// 	return false;
			// }
			//
			// // TODO: DO this with walkGrid
			// var tile = tilemap.GetTile(destination);
			// return CanWalkOnTile(tile);
		}

		private static bool CanWalkOnTile(int tileId)
		{
			return Game.Instance.Config.TilesData[tileId].Walkable;
		}

		public static bool CanBlockProjectile(TileBase tile)
		{
			return false;
			// if (Equals(tile, null))
			// {
			// 	return false;
			// }
			//
			// return Game.Instance.Config.BlockingTiles.Contains(tile);
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

		public static IEnumerable<Vector3Int> GetWalkableTilesInRange(Vector3Int start, int maxDistance, Grid grid)
		{
			var startNode = grid.nodes[start.x, start.y];
			return GetNodesInRange(startNode, maxDistance, grid)
				.Where(node => node.walkable)
				.Select(node => (Vector3Int) node);
		}

		private static IEnumerable<Node> GetNodesInRange(Node startNode, int maxDistance, Grid grid)
		{
			foreach (var node in grid.nodes)
			{
				if (Pathfinding.FindPath(grid, startNode, node).Count <= maxDistance)
				{
					yield return node;
				}
			}
		}
	}

	public static class TilemapHelpers
	{
		public static void SetTiles(IEnumerable<Vector3Int> positions, Tilemap tilemap, TileBase tile, Color color)
		{
			foreach (var position in positions)
			{
				tilemap.SetTile(position, tile);
				tilemap.SetColor(position, color);
			}
		}

		public static void ClearTilemap(Tilemap tilemap)
		{
			tilemap.ClearAllTiles();
		}
	}

	public class AttackResult
	{
		public Unit Attacker;
		public List<Vector3Int> Path;
		public List<Unit> Targets;
	}
}
