using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using Grid = NesScripts.Controls.PathFind.Grid;

namespace Code.SecretSanta.Game.RPG
{
	public class Board : MonoBehaviour
	{
		[Title("Encounter")]
		[SerializeField][Required] private GameObject _encounterRoot;
		[SerializeField][Required] private Tilemap _areaTilemap;
		[SerializeField][Required] private Tilemap _highlightTilemap;
		[SerializeField][Required] private Tilemap _gridWalkTilemap;

		[Title("Worldmap")]
		[SerializeField][Required] private GameObject _worldmapRoot;
		[SerializeField][Required] private Tilemap _worldmapTilemap;
		[SerializeField][Required] private Tilemap _pointsTilemap;

		public void Awake()
		{
			HideEncounter();
			HideWorldmap();
		}

		public void ShowEncounter() => _encounterRoot.SetActive(true);

		public void HideEncounter() => _encounterRoot.SetActive(false);

		public void ShowWorldmap() => _worldmapRoot.SetActive(true);

		public void HideWorldmap() => _worldmapRoot.SetActive(false);

		public void ClearHighlight()
		{
			TilemapHelpers.ClearTilemap(_highlightTilemap);
		}

		public void HighlightTiles(IEnumerable<Vector3Int> tiles, Color color)
		{
			TilemapHelpers.SetTiles(tiles, _highlightTilemap, Game.Instance.Config.HighlightTile, color);
		}

		public void DrawArea(Area area)
		{
			TilemapHelpers.RenderArea(area, _areaTilemap, Game.Instance.Config.TilesData);
		}

		public void DrawGridWalk(Grid walkGrid)
		{
			Helpers.RenderGridWalk(walkGrid, _gridWalkTilemap, Game.Instance.Config.EmptyTile);
		}

		public void ClearArea()
		{
			TilemapHelpers.ClearTilemap(_areaTilemap);
		}
	}
}
