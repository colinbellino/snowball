#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Snowball.Game
{
	public class AreaCreation : MonoBehaviour
	{
		[SerializeField] private Tilemap _tilemap;
		[SerializeField] private Tilemap _metaTilemap;
		[SerializeField] private Area _area;

		private void Start()
		{
			Game.InitForDebug();
			Load();
		}

		[ButtonGroup("Actions")] [Button] [DisableInEditorMode]
		private void Clear()
		{
			TilemapHelpers.ClearTilemap(_tilemap);
			TilemapHelpers.ClearTilemap(_metaTilemap);
		}

		[ButtonGroup("Actions")] [Button] [DisableInEditorMode]
		private void Load()
		{
			Clear();
			TilemapHelpers.RenderArea(_area, _tilemap, Game.Instance.Config.TilesData, true);
			TilemapHelpers.RenderMeta(_area, _metaTilemap);
		}

		[ButtonGroup("Actions")] [Button] [DisableInEditorMode]
		private void Save()
		{
			_area.Size = Game.Instance.Config.GridSize + Game.Instance.Config.GridOffset * 2;
			_area.Tiles = new int[_area.Size.x * _area.Size.y];
			_area.AllySpawnPoints = new List<Vector2Int>();
			_area.FoeSpawnPoints = new List<Vector2Int>();
			var bounds = new BoundsInt(Vector3Int.zero, new Vector3Int(_area.Size.x, _area.Size.y, 1));

			var allTiles = _tilemap.GetTilesBlock(bounds);
			for (var x = 0; x < _area.Size.x; x++)
			{
				for (var y = 0; y < _area.Size.y; y++)
				{
					var index = x + y * _area.Size.x;
					var tileId = GetTileIndex(allTiles[index], Game.Instance.Config.TilesData);

					_area.Tiles[index] = tileId;
				}
			}

			var allMeta = _metaTilemap.GetTilesBlock(bounds);
			for (var x = 0; x < _area.Size.x; x++)
			{
				for (var y = 0; y < _area.Size.y; y++)
				{
					var index = x + y * _area.Size.x;
					var position = new Vector2Int(x, y);
					var tile = allMeta[index];

					if (tile == Game.Instance.Config.AllySpawnTile)
					{
						_area.AllySpawnPoints.Add(position);
					}
					if (tile == Game.Instance.Config.FoeSpawnTile)
					{
						_area.FoeSpawnPoints.Add(position);
					}
				}
			}

			EditorUtility.SetDirty(_area);
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
