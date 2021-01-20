using System;
using System.Collections.Generic;
using System.Linq;
using NesScripts.Controls.PathFind;
using UnityEngine;
using Grid = NesScripts.Controls.PathFind.Grid;

namespace Snowball.Game
{
	public static class GridHelpers
	{
		public static Grid GetEmptyGrid(Area area, TilesData tilesData)
		{
			var data = new bool[area.Size.x, area.Size.y];
			for (var x = 0; x < area.Size.x; x++)
			{
				for (var y = 0; y < area.Size.y; y++)
				{
					var index = x + y * area.Size.x;
					var tileId = area.Tiles[index];
					var tileData = tilesData[tileId];

					data[x, y] = tileData.Blocking == false;
				}
			}

			return new Grid(data);
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

					// Check if the tile below is walkable (ground)
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

		public static Grid GetBlockGrid(Area area, TilesData tilesData)
		{
			var data = new bool[area.Size.x, area.Size.y];
			for (var x = 0; x < area.Size.x; x++)
			{
				for (var y = 0; y < area.Size.y; y++)
				{
					var index = x + y * area.Size.x;
					var tileId = area.Tiles[index];
					var tileData = tilesData[tileId];

					data[x, y] = tileData.Blocking;
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

		public static List<Vector3Int> GetTilesInRange(Vector3Int start, int maxDistance, Grid grid)
		{
			var startNode = grid.nodes[start.x, start.y];
			return GetNodesInRange(startNode, maxDistance, grid)
				.Select(node => (Vector3Int) node)
				.ToList();
		}

		public static List<Vector3Int> GetWalkableTilesInRange(Vector3Int start, int maxDistance, Grid grid, List<Unit> allUnits)
		{
			var startNode = grid.nodes[start.x, start.y];
			var nodes = GetNodesInRange(startNode, maxDistance, grid, Pathfinding.DistanceType.Euclidean);

			return nodes
				.Where(node => GetUnitInNode(node, allUnits) == null)
				.Select(node => (Vector3Int) node)
				.ToList();
		}

		private static Unit GetUnitInNode(Node node, List<Unit> allUnits)
		{
			return allUnits.Find(unit => unit.GridPosition.x == node.gridX && unit.GridPosition.y == node.gridY);
		}

		private static IEnumerable<Node> GetNodesInRange(Node startNode, int maxDistance, Grid grid, Pathfinding.DistanceType distanceType = Pathfinding.DistanceType.Manhattan)
		{
			return Pathfinding.GetTilesInRange(startNode, maxDistance, grid, distanceType);
		}

		public static int CalculateHitAccuracy(Vector3 origin, Vector3 destination, Grid blockGrid, Unit attacker, List<Unit> allUnits)
		{
			var hitChance = attacker.HitAccuracy;

			var direction = destination - origin;
			const int segments = 100;
			var checkedPositions = new List<Vector3Int>();

			for (var i = 1; i <= segments; i++)
			{
				var segmentDestination = origin + 1f / segments * i * direction;
				var segmentGridDestination = new Vector3Int(Mathf.RoundToInt(segmentDestination.x), Mathf.RoundToInt(segmentDestination.y), 0);

				if (checkedPositions.Contains(segmentGridDestination))
				{
					continue;
				}

				checkedPositions.Add(segmentGridDestination);

				var blockedByTile = blockGrid.nodes[segmentGridDestination.x, segmentGridDestination.y].walkable;
				if (blockedByTile)
				{
					hitChance = 0;
					break;
				}

				var unit = allUnits.Find(unit => unit.GridPosition == segmentGridDestination);
				var blockedBySnowpal = unit != null && unit.Type == Unit.Types.Snowpal && unit != attacker && segmentGridDestination != destination;
				if (blockedBySnowpal)
				{
					hitChance -= 50;
				}
			}

			if (direction.magnitude > attacker.HitRange)
			{
				hitChance = 0;
			}

			return Math.Max(0, hitChance);
		}
	}
}
