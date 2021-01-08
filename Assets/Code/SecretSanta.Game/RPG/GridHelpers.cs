using System.Collections.Generic;
using System.Linq;
using NesScripts.Controls.PathFind;
using UnityEngine;
using UnityEngine.Tilemaps;
using Grid = NesScripts.Controls.PathFind.Grid;

namespace Code.SecretSanta.Game.RPG
{
	public static class GridHelpers
	{
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

		public static Vector3Int GetCursorPosition(Vector2 cursorPosition, Vector2Int areaSize)
		{
			var position = new Vector3Int(
				Mathf.RoundToInt(cursorPosition.x).Clamp(0, areaSize.x - 1),
				Mathf.RoundToInt(cursorPosition.y).Clamp(0, areaSize.y - 1),
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
}
