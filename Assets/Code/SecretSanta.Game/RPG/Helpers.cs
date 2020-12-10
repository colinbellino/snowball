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
	}
}
