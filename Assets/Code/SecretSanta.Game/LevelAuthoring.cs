using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class LevelAuthoring : MonoBehaviour
{
	public Transform Root => transform;
	public Vector3 WinTriggerPosition { get; private set; }
	public List<Spawner> Spawners { get; private set; } = new List<Spawner>();

	public void Sync()
	{
		Spawners.Clear();
		Transform winTrigger = null;

		foreach (Transform child in Root)
		{
			var spawner = child.GetComponent<Spawner>();
			if (spawner)
			{
				Spawners.Add(spawner);
			}

			if (child.name == "Win Trigger")
			{
				winTrigger = child;
			}
		}

		WinTriggerPosition = winTrigger.position;

		Assert.IsNotNull(winTrigger);
		Assert.IsTrue(Spawners.Count > 0);
	}
}
