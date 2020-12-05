using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.SecretSanta.Game.RPG
{
	public class AreaCreation : MonoBehaviour
	{
		[SerializeField] private Tilemap _tilemap;
		[SerializeField] private Area _area;

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
					var tileId = Helpers.GetTileId(tile, Game.Instance.Config.Tiles);

					_area.Tiles[index] = tileId;
				}
			}
		}

		public void Load()
		{
			Clear();
			Helpers.LoadArea(_area, _tilemap, Game.Instance.Config.Tiles);
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
