using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour
{
	[SerializeField] private Vector3 _scrollDirection = new Vector3(-1f, 0f, 0f);
	[SerializeField] private Color _color = new Color(0.2f, 0.6f, 0.8f);

	public Transform Root => transform;
	public Vector3 Scroll => _scrollDirection;
	public Color Color => _color;
	public Transform WinTrigger { get; private set; }
	public List<Spawner> Spawners { get; } = new List<Spawner>();
	public Vector3 StartPosition { get; private set; }
	public Tilemap ObstaclesTilemap { get; private set; }

	public void Sync()
	{
		Spawners.Clear();
		Transform winTrigger = null;
		Transform start = null;
		Tilemap obstaclesTilemap = null;

		foreach (Transform child in Root)
		{
			var spawner = child.GetComponent<Spawner>();
			if (spawner)
			{
				Spawners.Add(spawner);
				continue;
			}

			if (child.name == "Win Trigger")
			{
				winTrigger = child;
				continue;
			}

			if (child.name == "Start")
			{
				start = child;
				continue;
			}

			foreach (Transform child2 in child)
			{
				var tilemap = child2.GetComponent<Tilemap>();
				if (child2.name == "Obstacles" && tilemap)
				{
					obstaclesTilemap = tilemap;
				}
			}
		}

		Assert.IsNotNull(winTrigger, "Missing win trigger position.");
		Assert.IsNotNull(start, "Missing start position.");
		Assert.IsNotNull(obstaclesTilemap, "Missing obstacles tilemap.");
		Assert.IsTrue(Spawners.Count > 0, "Missing spawners (enemies)");

		WinTrigger = winTrigger;
		StartPosition = start.position;
		ObstaclesTilemap = obstaclesTilemap;
	}

	public static void SetTilemapColor(Tilemap tilemap, Color color)
	{
		var bounds = tilemap.cellBounds;
		// var allTiles = tilemap.GetTilesBlock(bounds);

		for (var x = bounds.x; x < bounds.x + bounds.size.x; x++)
		{
			for (var y = bounds.y; y < bounds.y + bounds.size.y; y++)
			{
				// var tile = allTiles[x + y * bounds.size.x];
				var position = new Vector3Int(x, y, 0);
				if (tilemap.HasTile(position))
				{
					tilemap.SetTileFlags(position, TileFlags.None);
					tilemap.SetColor(position, color);
				}
			}
		}
	}
}
