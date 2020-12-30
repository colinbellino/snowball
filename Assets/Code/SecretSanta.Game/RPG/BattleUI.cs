using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.SecretSanta.Game.RPG
{
	public class BattleUI : MonoBehaviour
	{
		[SerializeField] private GameObject _actionsRoot;
		[SerializeField] private Button _attackButton;
		[SerializeField] private Button _moveButton;
		[SerializeField] private Button _waitButton;

		[SerializeField] private GameObject _moveCursor;
		[SerializeField] private LineRenderer _moveLine;

		private EventSystem _eventSystem;

		private void Awake()
		{
			_eventSystem = EventSystem.current;
		}

		public void ShowActions() => _actionsRoot.SetActive(true);
		public void HideActions() => _actionsRoot.SetActive(false);

		public void SelectAction(BattleAction action)
		{
			switch (action)
			{
				case BattleAction.Attack:
					_eventSystem.SetSelectedGameObject(_attackButton.gameObject);
					break;
				case BattleAction.Move:
					_eventSystem.SetSelectedGameObject(_moveButton.gameObject);
					break;
				case BattleAction.Wait:
					_eventSystem.SetSelectedGameObject(_waitButton.gameObject);
					break;
			}
		}

		public void HighlightMovePath(List<Vector3Int> path)
		{
			_moveLine.positionCount = path.Count;
			for (var pathIndex = 0; pathIndex < path.Count; pathIndex++)
			{
				var point = path[pathIndex];
				_moveLine.SetPosition(pathIndex, point);
			}

			if (path.Count > 0)
			{
				_moveCursor.transform.position = path[path.Count - 1];
			}
		}

		public void ClearMovePath()
		{
			_moveLine.positionCount = 0;
			_moveCursor.transform.position = new Vector3(999, 999, 0);
		}
	}
}
