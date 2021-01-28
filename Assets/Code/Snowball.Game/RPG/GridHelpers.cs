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
		public static bool AlwaysTrue(int x, int y, int index, Area area, TilesData tilesData) => true;

		public static bool IsEmpty(int x, int y, int index, Area area, TilesData tilesData)
		{
			var tileData = tilesData[area.Tiles[index]];
			return tileData.Blocking == false;
		}

		public static bool IsBlocking(int x, int y, int index, Area area, TilesData tilesData)
		{
			var tileData = tilesData[area.Tiles[index]];
			return tileData.Blocking;
		}

		public static bool IsWalkable(int x, int y, int index, Area area, TilesData tilesData)
		{
			var tileData = tilesData[area.Tiles[index]];
			if (tileData.Climbable)
			{
				return true;
			}

			if (tileData.Blocking)
			{
				return false;
			}

			// Check if the tile below is walkable (ground)
			if (y > 0)
			{
				var belowIndex = x + (y - 1) * area.Size.x;
				var belowTileId = area.Tiles[belowIndex];

				if (tilesData[belowTileId].Walkable)
				{
					return true;
				}
			}

			return false;
		}

		public static bool IsBuildable(int x, int y, int index, Area area, TilesData tilesData)
		{
			var tileData = tilesData[area.Tiles[index]];
			if (tileData.Climbable)
			{
				return false;
			}

			if (tileData.Blocking)
			{
				return false;
			}

			// Check if the tile below is walkable (ground)
			if (y > 0)
			{
				var belowIndex = x + (y - 1) * area.Size.x;
				var belowTileId = area.Tiles[belowIndex];

				if (tilesData[belowTileId].Walkable && tilesData[belowTileId].Climbable == false)
				{
					return true;
				}
			}

			return false;
		}

		public static Grid GenerateGrid(Area area, Vector2Int offset, TilesData tilesData, Func<int, int, int, Area, TilesData, bool> Check)
		{
			var data = new bool[area.Size.x, area.Size.y];

			for (var x = offset.x; x < area.Size.x - offset.x; x++)
			{
				for (var y = offset.y; y < area.Size.y - offset.y; y++)
				{
					var index = x + y * area.Size.x;

					data[x, y] = Check(x, y, index, area, tilesData);
				}
			}

			return new Grid(data);
		}

		public static List<Vector3Int> FindPath(Vector3Int start, Vector3Int destination, Grid walkGrid)
		{
			var path = Pathfinding.FindPath(walkGrid, start, destination);
			return path.Prepend(start).ToList();
		}

		public static Vector3Int GetCursorPosition(Vector2 cursorPosition, Vector2Int gridSize, Vector2Int offset)
		{
			var position = new Vector3Int(
				Mathf.RoundToInt(cursorPosition.x).Clamp(offset.x, gridSize.x),
				Mathf.RoundToInt(cursorPosition.y).Clamp(offset.y, gridSize.y),
				0
			);

			return position;
		}

		public static List<Vector3Int> GetTilesInRange(Vector3Int start, int maxDistance, Grid grid)
		{
			var startNode = grid.nodes[start.x, start.y];
			var nodes = Pathfinding.GetTilesInRange(startNode, maxDistance, grid);

			return nodes.Select(node => (Vector3Int) node).ToList();
		}

		public static List<Vector3Int> GetWalkableTilesInRange(Vector3Int start, Unit actor, int maxDistance, Grid grid, List<Unit> allUnits)
		{
			var startNode = grid.nodes[start.x, start.y];

			Func<Node, int> Check = (node) =>
			{
				if (node == startNode)
				{
					return 0;
				}

				var unit = GetUnitInNode(node, allUnits);
				if (unit != null)
				{
					return unit.Alliance == actor.Alliance ? 0 : -1;
				}

				return 1;
			};
			var nodes = Pathfinding.GetTilesInRange(startNode, maxDistance, grid, Pathfinding.DistanceType.Euclidean, Check);

			return nodes.Select(node => (Vector3Int) node).ToList();
		}

		public static List<Vector3Int> GetBuildTilesInRange(Vector3Int start, Unit actor, int maxDistance, Grid grid, List<Unit> allUnits)
		{
			var startNode = grid.nodes[start.x, start.y];

			Func<Node, int> Check = (node) =>
			{
				if (node == startNode)
				{
					return 0;
				}

				var unit = GetUnitInNode(node, allUnits);
				if (unit != null)
				{
					return 0;
				}

				return 1;
			};
			var nodes = Pathfinding.GetTilesInRange(startNode, maxDistance, grid, Pathfinding.DistanceType.Euclidean, Check);

			return nodes.Select(node => (Vector3Int) node).ToList();
		}

		private static Unit GetUnitInNode(Node node, List<Unit> allUnits)
		{
			return allUnits.Find(unit => unit.GridPosition.x == node.gridX && unit.GridPosition.y == node.gridY);
		}

		public static int CalculateHitAccuracy(Vector3 start, Vector3 destination, Unit attacker, Grid blockGrid, List<Unit> allUnits)
		{
			var hitChance = attacker.HitAccuracy;

			var direction = destination - start;
			const int segments = 100;
			var checkedPositions = new List<Vector3Int>();

			for (var i = 1; i <= segments; i++)
			{
				var segmentDestination = start + 1f / segments * i * direction;
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
