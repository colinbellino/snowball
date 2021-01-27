using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Snowball.Game.UnitHelpers;

namespace Snowball.Game
{
	public class BattleUI : MonoBehaviour
	{
		[Serializable]
		private class ActionButtons : UnitySerializedDictionary<BattleActions, ButtonUI> { }

		[Serializable]
		private class CursorColorsDictionary : UnitySerializedDictionary<CursorColors, Color> { }

		private enum CursorColors { Default, Success, Warning, Error }

		[Title("Menu")]
		[SerializeField][Required] private GameObject _actionsRoot;
		[SerializeField][Required] private ActionButtons _actionButtons;
		[Title("Cursors")]
		[SerializeField][Required] private SpriteRenderer _moveCursor;
		[SerializeField][Required] private LineRenderer _moveLine;
		[SerializeField][Required] private SpriteRenderer _aimCursor;
		[SerializeField][Required] private Text _aimCursorText;
		[SerializeField][Required] private LineRenderer _aimLine;
		[SerializeField][Required] private CursorColorsDictionary _cursorColors;
		[Title("Current unit")]
		[SerializeField][Required] private GameObject _currentUnitCursor;
		[Title("Infos")]
		[SerializeField][Required] private Text _hitRateText;
		[SerializeField][Required] private UnitInfosPanel _actorInfosPanel;
		[SerializeField][Required] private UnitInfosPanel _targetInfosPanel;

		public event Action<BattleActions> OnActionClicked;
		private static readonly Vector3 OUT_OF_SCREEN_POSITION = new Vector3(999, 999, 0);

		private void Awake()
		{
			HideAll();
		}

		private void OnEnable()
		{
			foreach (var item in _actionButtons)
			{
				item.Value.OnClick.AddListener(OnBattleActionClicked(item.Key));
			}
		}

		private void OnDisable()
		{
			foreach (var item in _actionButtons)
			{
				item.Value.OnClick.RemoveListener(OnBattleActionClicked(item.Key));
			}
		}

		public void HideAll()
		{
			_ = HideActionsMenu();
			ClearMovePath();
			ClearTarget();
			_ = HideActionsMenu();
			HideActorInfos();
			HideTargetInfos();
			HideHitRate();
		}

		public void SetTurnUnit(Unit unit)
		{
			if (unit == null)
			{
				_currentUnitCursor.transform.position = OUT_OF_SCREEN_POSITION;
				// _actionsTitle.text = "";
			}
			else
			{
				_currentUnitCursor.transform.position = unit.GridPosition;
				// _actionsTitle.text = $"{unit.Name}";
			}
		}

		public void ShowActionsMenu()
		{
			_actionsRoot.SetActive(true);
		}

		public UniTask HideActionsMenu()
		{
			_actionsRoot.SetActive(false);

			return default;
		}

		public void ToggleButton(BattleActions action, bool value)
		{
			_actionButtons[action].Toggle(value);
		}

		public void HighlightMovePath(List<Vector3Int> path)
		{
			_moveLine.positionCount = path.Count;
			_moveLine.startColor = _cursorColors[CursorColors.Default];
			_moveLine.endColor = _cursorColors[CursorColors.Default];
			for (var pathIndex = 0; pathIndex < path.Count; pathIndex++)
			{
				var point = path[pathIndex];
				_moveLine.SetPosition(pathIndex, point);
			}

			if (path.Count > 0)
			{
				_moveCursor.color = _cursorColors[CursorColors.Default];
				_moveCursor.transform.position = path[path.Count - 1];
			}
		}

		public void ClearMovePath()
		{
			_moveLine.positionCount = 0;
			_moveCursor.transform.position = OUT_OF_SCREEN_POSITION;
		}

		public void HighlightAttackTarget(Vector3Int origin, Vector3Int destination, int hitChance)
		{
			_aimLine.positionCount = 2;
			_aimLine.SetPosition(0, origin);
			_aimLine.SetPosition(1, destination);
			_aimCursor.transform.position = destination;
			_aimCursorText.text = $"hit: {hitChance}%\n{destination}";

			var color = hitChance switch
			{
				100 => _cursorColors[CursorColors.Success],
				0 => _cursorColors[CursorColors.Error],
				_ => _cursorColors[CursorColors.Warning]
			};

			_aimLine.startColor = color;
			_aimLine.endColor = color;
			_aimCursor.color = color;
			// _aimCursorText.color = color;
		}

		public void ClearTarget()
		{
			_aimLine.positionCount = 0;
			_aimCursor.transform.position = OUT_OF_SCREEN_POSITION;
			_aimCursorText.text = "";
		}

		public void ShowActorInfos(Unit unit, Color teamColor) => ShowInfos(_actorInfosPanel, unit, teamColor);

		public void HideActorInfos() => HideInfos(_actorInfosPanel);

		public void ShowTargetInfos(Unit unit, Color teamColor) => ShowInfos(_targetInfosPanel, unit, teamColor);

		public void HideTargetInfos() => HideInfos(_targetInfosPanel);

		public void ShowHitRate(int hitRate)
		{
			_hitRateText.text = $"> {hitRate}%";
			_hitRateText.gameObject.SetActive(true);
		}

		public void HideHitRate()
		{
			_hitRateText.gameObject.SetActive(false);
		}

		private static void ShowInfos(UnitInfosPanel infos, Unit unit, Color teamColor)
		{
			infos.InfosNameText.text = unit.Name;
			infos.InfosHealthText.text = $"{unit.HealthCurrent}/{unit.HealthMax}";
			var scale = infos.InfosHealthImage.transform.localScale;
			scale.x = (float) unit.HealthCurrent / unit.HealthMax;
			infos.InfosHealthImage.transform.localScale = scale;
			// infos.InfosBodyImage.transform.rotation = unit.Facade.BodyRenderer.transform.rotation;

			infos.InfosBodyImage.sprite = unit.Sprite;
			infos.InfosBodyImage.material = Instantiate(infos.InfosBodyImage.material);
			ApplyColors(infos.InfosBodyImage.material, unit.ColorCloth, unit.ColorHair, unit.ColorSkin);
			ApplyColors(infos.InfosLeftHandImage.material, unit.ColorCloth, unit.ColorHair, unit.ColorSkin);
			ApplyColors(infos.InfosRightHandImage.material, unit.ColorCloth, unit.ColorHair, unit.ColorSkin);

			if (unit.Alliance == Unit.Alliances.Foe)
			{
				ApplyColors(infos.InfosBodyImage.material, teamColor, unit.ColorHair, unit.ColorSkin);
			}

			infos.InfosRoot.SetActive(true);
		}

		private static void HideInfos(UnitInfosPanel infos)
		{
			infos.InfosRoot.SetActive(false);
		}

		private UnityAction OnBattleActionClicked(BattleActions action)
		{
			return () => OnActionClicked?.Invoke(action);
		}
	}
}
