using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using Grid = NesScripts.Controls.PathFind.Grid;

namespace Snowball.Game
{
	public class Board : MonoBehaviour
	{
		[SerializeField][Required] private GameObject _encounterRoot;
		[SerializeField][Required] private Tilemap _areaTilemap;
		[SerializeField][Required] private Tilemap _highlightTilemap;
		[SerializeField][Required] private Tilemap _walkGridTilemap;
		[SerializeField][Required] private Tilemap _blockGridTilemap;

		public void Awake()
		{
			HideEncounter();
		}

		public void ShowEncounter() => _encounterRoot.SetActive(true);

		public void HideEncounter() => _encounterRoot.SetActive(false);

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

		public void DrawGridWalk(Grid grid)
		{
			TilemapHelpers.RenderGridWalk(grid, _walkGridTilemap, Game.Instance.Config.DebugTile);
		}

		public void DrawBlockWalk(Grid grid)
		{
			TilemapHelpers.RenderBlockWalk(grid, _blockGridTilemap, Game.Instance.Config.DebugTile);
		}

		public void ClearArea()
		{
			TilemapHelpers.ClearTilemap(_areaTilemap);
		}
	}
}
