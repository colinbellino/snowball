using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.SecretSanta.Game.RPG
{
	public class AreaCreation : MonoBehaviour
	{
		[SerializeField] private Tilemap _tilemap;
		[SerializeField] private Tilemap _metaTilemap;
		[SerializeField] private Area _area;

		public void Clear()
		{
			TilemapHelpers.ClearTilemap(_tilemap);
			TilemapHelpers.ClearTilemap(_metaTilemap);
		}

		public void Save()
		{
			_area.Size = new Vector2Int(32, 18);
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

		public void Load()
		{
			Clear();
			TilemapHelpers.RenderArea(_area, _tilemap, Game.Instance.Config.TilesData);
			TilemapHelpers.RenderMeta(_area, _metaTilemap);
		}

		private static int GetTileIndex(TileBase tile, TilesData tilesData)
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
	}

	[CustomEditor(typeof(AreaCreation))]
	public class StuffEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			var stuff = (AreaCreation)target;

			// if (Application.isPlaying)
			// {
			if (GUILayout.Button("Clear"))
			{
				stuff.Clear();
			}
			if (GUILayout.Button("Save"))
			{
				stuff.Save();
			}
			if (GUILayout.Button("Load"))
			{
				stuff.Load();
			}
			// }
		}
	}
}
