using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.SecretSanta.Game.RPG
{
	public class Worldmap : MonoBehaviour
	{
		[SerializeField][Required] private GameObject _worldmapRoot;
		[SerializeField][Required] private List<WorldmapEncounterPoint> _encounterPoints;

		public event Action<int> EncounterClicked;

		public void Awake()
		{
			HideWorldmap();
		}

		public void OnEnable()
		{
			for (var encounterIndex = 0; encounterIndex < _encounterPoints.Count; encounterIndex++)
			{
				var encounterPoint = _encounterPoints[encounterIndex];
				encounterPoint.Clicked += OnEncounterPointClicked(encounterIndex);
			}
		}

		public void OnDisable()
		{
			for (var encounterIndex = 0; encounterIndex < _encounterPoints.Count; encounterIndex++)
			{
				var encounterPoint = _encounterPoints[encounterIndex];
				encounterPoint.Clicked -= OnEncounterPointClicked(encounterIndex);
			}
		}

		public void ShowWorldmap() => _worldmapRoot.SetActive(true);

		public void HideWorldmap() => _worldmapRoot.SetActive(false);

		public void SetEncounters(List<int> encounters)
		{
			for (var encounterIndex = 0; encounterIndex < _encounterPoints.Count; encounterIndex++)
			{
				var encounterPoint = _encounterPoints[encounterIndex];
				encounterPoint.gameObject.SetActive(encounterIndex < encounters.Count);
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
