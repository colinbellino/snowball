/**
 * Provide simple path-finding algorithm with tile prices support.
 * Based on code and tutorial by Sebastian Lague (https://www.youtube.com/channel/UCmtyQOKKmrMVaKuRXz02jbQ).
 *
 * Edit 02/01/2021 (Colin): Replaced Point by Vector3Int
 *
 * Author: Ronen Ness.
 * Since: 2016.
*/
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NesScripts.Controls.PathFind
{
	/// <summary>
	/// Main class to find the best path to walk from A to B.
	///
	/// Usage example:
	/// Grid grid = new Grid(width, height, tiles_costs);
	/// List<Vector3Int> path = Pathfinding.FindPath(grid, from, to);
	/// </summary>
	public class Pathfinding
	{
		/// <summary>
		/// Different ways to calculate path distance.
		/// </summary>
		public enum DistanceType
		{
			/// <summary>
			/// The "ordinary" straight-line distance between two points.
			/// </summary>
			Euclidean,

			/// <summary>
			/// Distance without diagonals, only horizontal and/or vertical path lines.
			/// </summary>
			Manhattan
		}

		/// <summary>
		/// Find a path between two points.
		/// </summary>
		/// <param name="grid">Grid to search.</param>
		/// <param name="startPos">Starting position.</param>
		/// <param name="targetPos">Ending position.</param>
		/// <param name="distance">The type of distance, Euclidean or Manhattan.</param>
		/// <param name="ignorePrices">If true, will ignore tile price (how much it "cost" to walk on).</param>
		/// <returns>List of points that represent the path to walk.</returns>
		public static List<Vector3Int> FindPath(Grid grid, Vector3Int startPos, Vector3Int targetPos, DistanceType distance = DistanceType.Euclidean, bool ignorePrices = false)
		{
			// find path
			List<Node> nodes_path = _ImpFindPath(grid, startPos, targetPos, distance, ignorePrices);

			// convert to a list of points and return
			List<Vector3Int> ret = new List<Vector3Int>();
			if (nodes_path != null)
			{
				foreach (Node node in nodes_path)
				{
					ret.Add(new Vector3Int(node.gridX, node.gridY, 0));
				}
			}
			return ret;
		}

		/// <summary>
		/// Internal function that implements the path-finding algorithm.
		/// </summary>
		/// <param name="grid">Grid to search.</param>
		/// <param name="startPos">Starting position.</param>
		/// <param name="targetPos">Ending position.</param>
		/// <param name="distance">The type of distance, Euclidean or Manhattan.</param>
		/// <param name="ignorePrices">If true, will ignore tile price (how much it "cost" to walk on).</param>
		/// <returns>List of grid nodes that represent the path to walk.</returns>
		private static List<Node> _ImpFindPath(Grid grid, Vector3Int startPos, Vector3Int targetPos, DistanceType distance = DistanceType.Euclidean, bool ignorePrices = false)
		{
			Node startNode = grid.nodes[startPos.x, startPos.y];
			Node targetNode = grid.nodes[targetPos.x, targetPos.y];

			List<Node> openSet = new List<Node>();
			HashSet<Node> closedSet = new HashSet<Node>();
			openSet.Add(startNode);

			while (openSet.Count > 0)
			{
				Node currentNode = openSet[0];
				for (int i = 1; i < openSet.Count; i++)
				{
					if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
					{
						currentNode = openSet[i];
					}
				}

				openSet.Remove(currentNode);
				closedSet.Add(currentNode);

				if (currentNode == targetNode)
				{
					return RetracePath(grid, startNode, targetNode);
				}

				foreach (Node neighbour in grid.GetNeighbours(currentNode, distance))
				{
					if (!neighbour.walkable || closedSet.Contains(neighbour))
					{
						continue;
					}

					int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) * (ignorePrices ? 1 : (int)(10.0f * neighbour.price));
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
					{
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetNode);
						neighbour.parent = currentNode;

						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
					}
				}
			}

			return null;
		}

		public static List<Node> GetTilesInRange(Node startNode, int maxDistance, Grid grid, DistanceType distanceType = DistanceType.Manhattan, bool ignorePrices = false)
		{
			var nodesInRange = new List<Node>();
			var nodesToCheck = new HashSet<Node>();
			nodesInRange.Add(startNode);

			while (nodesInRange.Count > 0)
			{
				var currentNode = nodesInRange[0];

				nodesInRange.Remove(currentNode);
				nodesToCheck.Add(currentNode);

				var neighbours = grid.GetNeighbours(currentNode, distanceType);
				foreach (var neighbour in neighbours)
				{
					if (!neighbour.walkable || nodesToCheck.Contains(neighbour))
					{
						continue;
					}

					var distance = GetDistance(startNode, neighbour);
					if (distance <= maxDistance * 10)
					{
						nodesToCheck.Add(neighbour);
						nodesInRange.Add(neighbour);
					}
				}
			}

			return nodesToCheck.ToList();
		}

		/// <summary>
		/// Retrace path between two points.
		/// </summary>
		/// <param name="grid">Grid to work on.</param>
		/// <param name="startNode">Starting node.</param>
		/// <param name="endNode">Ending (target) node.</param>
		/// <returns>Retraced path between nodes.</returns>
		private static List<Node> RetracePath(Grid grid, Node startNode, Node endNode)
		{
			List<Node> path = new List<Node>();
			Node currentNode = endNode;

			while (currentNode != startNode)
			{
				path.Add(currentNode);
				currentNode = currentNode.parent;
			}
			path.Reverse();
			return path;
		}

		/// <summary>
		/// Get distance between two nodes.
		/// </summary>
		/// <param name="nodeA">First node.</param>
		/// <param name="nodeB">Second node.</param>
		/// <returns>Distance between nodes.</returns>
		private static int GetDistance(Node nodeA, Node nodeB)
		{
			int dstX = System.Math.Abs(nodeA.gridX - nodeB.gridX);
			int dstY = System.Math.Abs(nodeA.gridY - nodeB.gridY);
			return (dstX > dstY) ?
				14 * dstY + 10 * (dstX - dstY) :
				14 * dstX + 10 * (dstY - dstX);
		}
	}

}
