using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Code.SecretSanta.Game.RPG
{
	[Serializable]
	public class ActionButtons : UnitySerializedDictionary<BattleAction, Button> { }

	public class BattleUI : MonoBehaviour
	{
		[Title("Turn order")]
		[SerializeField][Required] private GameObject _turnOrderRoot;
		[SerializeField][Required] private GameObject _turnOrderUnitsRoot;
		[Title("Actions")]
		[SerializeField][Required] private GameObject _actionsRoot;
		[SerializeField][Required] private Text _actionsTitle;
		[SerializeField][Required] private ActionButtons _actionButtons;
		[Title("Move")]
		[SerializeField][Required] private GameObject _moveCursor;
		[SerializeField][Required] private LineRenderer _moveLine;
		[Title("Aim")]
		[SerializeField][Required] private GameObject _aimCursor;
		[SerializeField][Required] private LineRenderer _aimLine;
		[Title("Current")]
		[SerializeField][Required] private GameObject _currentUnitCursor;

		public event Action<BattleAction> OnActionClicked;
		private static readonly Vector3 OUT_OF_SCREEN_POSITION = new Vector3(999, 999, 0);

		private void Awake()
		{
			HideAll();
		}

		private void OnEnable()
		{
			foreach (var item in _actionButtons)
			{
				item.Value.onClick.AddListener(OnBattleActionClicked(item.Key));
				item.Value.GetComponentInChildren<Text>().text = item.Key.ToString();
			}
		}

		private void OnDisable()
		{
			foreach (var item in _actionButtons)
			{
				item.Value.onClick.RemoveListener(OnBattleActionClicked(item.Key));
			}
		}

		public void HideAll()
		{
			HideActionsMenu();
			HideTurnOrder();
			ClearMovePath();
			ClearAimPath();
		}

		public void InitActionMenu(Unit unit)
		{
			if (unit == null)
			{
				_currentUnitCursor.transform.position = OUT_OF_SCREEN_POSITION;
				_actionsTitle.text = "";
				return;
			}

			_currentUnitCursor.transform.position = unit.GridPosition + Vector3Int.up;
			_actionsTitle.text = $"{unit.Name}";
		}

		public void ShowActionsMenu() => _actionsRoot.SetActive(true);

		public void HideActionsMenu() => _actionsRoot.SetActive(false);

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
		public void ShowTurnOrder() => _turnOrderRoot.SetActive(true);

		public void HideTurnOrder() => _turnOrderRoot.SetActive(false);

		public void SetTurnOrder(List<Unit> units)
		{
			foreach (Transform child in _turnOrderUnitsRoot.transform)
			{
				child.gameObject.SetActive(false);
			}

			for (var unitIndex = 0; unitIndex < units.Count; unitIndex++)
			{
				var unit = units[unitIndex];
				var child = _turnOrderUnitsRoot.transform.GetChild(unitIndex);

				var image = child.GetComponent<Image>();
				// Normally this is done by unity when we call SetColor but not for UnityEngine.UI.Image
				var materialInstance = Instantiate(image.material);
				materialInstance.SetColor("ReplacementColor0", unit.Color);
				image.material = materialInstance;
				image.gameObject.SetActive(true);
			}
		}

		private UnityAction OnBattleActionClicked(BattleAction action)
		{
			return () => OnActionClicked?.Invoke(action);
		}
	}
}
