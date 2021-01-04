using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Code.SecretSanta.Game.RPG
{
	[Serializable]
	public class ActionButtons : UnitySerializedDictionary<BattleAction, Button> { }

	public class BattleUI : MonoBehaviour
	{
		[SerializeField] private GameObject _actionsRoot;
		[SerializeField] private Text _actionsTitle;
		[SerializeField] private ActionButtons _actionButtons;
		[SerializeField] private GameObject _moveCursor;
		[SerializeField] private LineRenderer _moveLine;
		[SerializeField] private GameObject _aimCursor;
		[SerializeField] private LineRenderer _aimLine;
		[SerializeField] private GameObject _currentUnitCursor;

		public event Action<BattleAction> OnActionClicked;
		private static readonly Vector3 OUT_OF_SCREEN_POSITION = new Vector3(999, 999, 0);

		private void OnEnable()
		{
			foreach (var item in _actionButtons)
			{
				item.Value.onClick.AddListener(OnBattleActionClicked(item.Key));
			}
		}

		private void OnDisable()
		{
			foreach (var item in _actionButtons)
			{
				item.Value.onClick.RemoveListener(OnBattleActionClicked(item.Key));
			}
		}

		public void SetUnit(UnitComponent unit)
		{
			if (unit == null)
			{
				_currentUnitCursor.transform.position = OUT_OF_SCREEN_POSITION;
				_actionsTitle.text = "";
				return;
			}

			_currentUnitCursor.transform.position = unit.GridPosition + Vector3Int.up;
			_actionsTitle.text = $"{unit.Data.Name}";
		}

		public void ShowActions() => _actionsRoot.SetActive(true);

		public void HideActions() => _actionsRoot.SetActive(false);

		public void ToggleButton(BattleAction action, bool value)
		{
			_actionButtons[action].interactable = value;
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
			_moveCursor.transform.position = OUT_OF_SCREEN_POSITION;
		}

		public void HighlightAimPath(List<Vector3Int> path)
		{
			_aimLine.positionCount = path.Count;
			for (var pathIndex = 0; pathIndex < path.Count; pathIndex++)
			{
				var point = path[pathIndex];
				_aimLine.SetPosition(pathIndex, point);
			}

			if (path.Count > 0)
			{
				_aimCursor.transform.position = path[path.Count - 1];
			}
		}

		public void ClearAimPath()
		{
			_aimLine.positionCount = 0;
			_aimCursor.transform.position = OUT_OF_SCREEN_POSITION;
		}

		private UnityAction OnBattleActionClicked(BattleAction action)
		{
			return () => OnActionClicked?.Invoke(action);
		}
	}
}
