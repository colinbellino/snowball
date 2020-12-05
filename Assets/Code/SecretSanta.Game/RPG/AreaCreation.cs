using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.SecretSanta.Game.RPG
{
	public class AreaCreation : MonoBehaviour
	{
		[SerializeField] private Tilemap _tilemap;
		
		[SerializeField] private Area _area;
		[SerializeField] private TileBase[] _tiles;

		public void Clear()
		{
			_tilemap.ClearAllTiles();
		}

		public void Save()
		{
			_area.Size = new Vector2Int(32, 18);
			_area.Tiles = new int[_area.Size.x * _area.Size.y];

			var bounds = new BoundsInt(Vector3Int.zero, new Vector3Int(_area.Size.x, _area.Size.y, 1));
			var allTiles = _tilemap.GetTilesBlock(bounds);

			for (var x = 0; x < _area.Size.x; x++)
			{
				for (var y = 0; y < _area.Size.y; y++)
				{
					var index = x + y * _area.Size.x;
					var tile = allTiles[index];

					_area.Tiles[index] = GetTileValue(tile);
				}
			}
		}

		public void Load()
		{
			Clear();

			for (var x = 0; x < _area.Size.x; x++)
			{
				for (var y = 0; y < _area.Size.y; y++)
				{
					var index = x + y * _area.Size.x;
					var position = new Vector3Int(x, y, 0);

					_tilemap.SetTile(position, _tiles[_area.Tiles[index]]);
				}
			}
		}

		private int GetTileValue(TileBase tile)
		{
			for (var index = 0; index < _tiles.Length; index++)
			{
				if (_tiles[index] == tile)
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
		}
	}
}
