using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Snowball.Game
{
	public class Worldmap : MonoBehaviour
	{
		[SerializeField][Required] private GameObject _worldmapRoot;
		[SerializeField][Required] private List<WorldmapEncounterPoint> _encounterPoints;
		private Database _database;

		public event Action<int> EncounterClicked;

		public void Awake()
		{
			Hide();
		}

		public void Show()
		{
			_database = Game.Instance.Database;

			for (var encounterIndex = 0; encounterIndex < _encounterPoints.Count; encounterIndex++)
			{
				var encounterPoint = _encounterPoints[encounterIndex];
				encounterPoint.Clicked += OnEncounterPointClicked(encounterIndex);
			}

			_worldmapRoot.SetActive(true);
		}

		public void Hide()
		{
			_worldmapRoot.SetActive(false);
			for (var encounterIndex = 0; encounterIndex < _encounterPoints.Count; encounterIndex++)
			{
				var encounterPoint = _encounterPoints[encounterIndex];
				encounterPoint.Clicked -= OnEncounterPointClicked(encounterIndex);
			}
		}

		public void SetEncounters(List<EncounterAuthoring> encounters, GameState state)
		{
			for (var encounterIndex = 0; encounterIndex < _encounterPoints.Count; encounterIndex++)
			{
				var encounterPoint = _encounterPoints[encounterIndex];

				encounterPoint.gameObject.SetActive(encounterIndex < encounters.Count);
				if (encounterIndex < encounters.Count)
				{
					var encounter = encounters[encounterIndex];
					encounterPoint.name = encounter.Name;
					encounterPoint.SetCleared(encounter, state.EncountersDone.Contains(encounter.Id));
				}
			}
		}

		public Vector3Int GetEncounterGridPosition(int encounterIndex)
		{
			var position = _encounterPoints[encounterIndex].transform.position;
			return new Vector3Int((int) position.x, (int) position.y, 0);
		}

		private Action OnEncounterPointClicked(int encounterIndex)
		{
			return () => EncounterClicked?.Invoke(encounterIndex);
		}
	}
}
