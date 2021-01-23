using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Grid = NesScripts.Controls.PathFind.Grid;

namespace Snowball.Game
{
	public static class TilemapHelpers
	{
		public static void RenderArea(Area area, Tilemap tilemap, TilesData tilesData, bool useEditorTile = false)
		{
			for (var x = 0; x < area.Size.x; x++)
			{
				for (var y = 0; y < area.Size.y; y++)
				{
					var index = x + y * area.Size.x;
					var position = new Vector3Int(x, y, 0);
					var tileId = area.Tiles[index];

					tilemap.SetTile(position, useEditorTile ? tilesData[tileId].TileEditor : tilesData[tileId].Tile);
				}
			}
		}

		public static void RenderMeta(Area area, Tilemap tilemap, TileBase allySpawnTile, TileBase foeSpawnTile)
		{
			foreach (var position in area.AllySpawnPoints)
			{
				tilemap.SetTile(new Vector3Int(position.x, position.y, 0), allySpawnTile);
			}
			foreach (var position in area.FoeSpawnPoints)
			{
				tilemap.SetTile(new Vector3Int(position.x, position.y, 0), foeSpawnTile);
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
						color = Color.green; break;
					case 1f:
						color = Color.blue; break;
				}

				tilemap.SetTile(position, tile);
				tilemap.SetColor(position, color);
			}
		}

		public static void RenderBlockWalk(Grid grid, Tilemap tilemap, TileBase tile)
		{
			foreach (var node in grid.nodes)
			{
				var position = new Vector3Int(node.gridX, node.gridY, 0);
				tilemap.SetTile(position, tile);
				if (node.walkable)
				{
					tilemap.SetColor(position, Color.red);
				}
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
