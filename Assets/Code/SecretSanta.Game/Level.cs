using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Level : MonoBehaviour
{
	[SerializeField] private Vector3 _scrollDirection = new Vector3(-1f, 0f, 0f);

	public Transform Root => transform;
	public Vector3 Scroll => _scrollDirection;
	public Transform WinTrigger { get; set; }
	public List<Spawner> Spawners { get; private set; } = new List<Spawner>();
	public Vector3 StartPosition { get; set; }

	public void Sync()
	{
		Spawners.Clear();
		Transform winTrigger = null;
		Transform start = null;

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
			}

			if (child.name == "Start")
			{
				start = child;
			}
		}

		Assert.IsNotNull(winTrigger, "Missing win trigger position.");
		Assert.IsNotNull(start, "Missing start position.");
		Assert.IsTrue(Spawners.Count > 0, "Missing spawners (enemies)");

		WinTrigger = winTrigger;
		StartPosition = start.position;
	}
}
