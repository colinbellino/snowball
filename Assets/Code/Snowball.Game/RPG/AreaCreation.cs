#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Snowball.Game
{
	public class AreaCreation : MonoBehaviour
	{
		[SerializeField] private Tilemap _tilemap;
		[SerializeField] private Tilemap _metaTilemap;

		private GameConfig _config;

		public void Init(GameConfig config)
		{
			_config = config;
		}

		public void Clear()
		{
			TilemapHelpers.ClearTilemap(_tilemap);
			TilemapHelpers.ClearTilemap(_metaTilemap);
		}

		public void Load(Area area)
		{
			Clear();

			TilemapHelpers.RenderArea(area, _tilemap, _config.TilesData, true);
			TilemapHelpers.RenderMeta(area, _metaTilemap, _config.AllySpawnTile, _config.FoeSpawnTile);
		}

		public void Save(Area area)
		{
			area.Size = _config.GridSize + _config.GridOffset * 2;
			area.Tiles = new int[area.Size.x * area.Size.y];
			area.AllySpawnPoints = new List<Vector2Int>();
			area.FoeSpawnPoints = new List<Vector2Int>();
			var bounds = new BoundsInt(Vector3Int.zero, new Vector3Int(area.Size.x, area.Size.y, 1));

			var allTiles = _tilemap.GetTilesBlock(bounds);
			for (var x = 0; x < area.Size.x; x++)
			{
				for (var y = 0; y < area.Size.y; y++)
				{
					var index = x + y * area.Size.x;
					var tileId = GetTileIndex(allTiles[index], _config.TilesData);

					area.Tiles[index] = tileId;
				}
			}

			var allMeta = _metaTilemap.GetTilesBlock(bounds);
			for (var x = 0; x < area.Size.x; x++)
			{
				for (var y = 0; y < area.Size.y; y++)
				{
					var index = x + y * area.Size.x;
					var position = new Vector2Int(x, y);
					var tile = allMeta[index];

					if (tile == _config.AllySpawnTile)
					{
						area.AllySpawnPoints.Add(position);
					}
					if (tile == _config.FoeSpawnTile)
					{
						area.FoeSpawnPoints.Add(position);
					}
				}
			}

			EditorUtility.SetDirty(area);
		}

		private static int GetTileIndex(TileBase tile, TilesData tilesData)
		{
			for (var index = 0; index < tilesData.Count; index++)
			{
				if (tilesData[index].TileEditor == tile)
				{
					return index;
				}
			}

			return 0;
		}
	}
}
#endif
