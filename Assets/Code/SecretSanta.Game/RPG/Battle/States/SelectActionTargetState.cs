using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectActionTargetState : BaseBattleState, IState
	{
		private Vector3Int _cursorPosition;
		public SelectActionTargetState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public async UniTask Enter(object[] args)
		{
			_ui.InitMenu(_turn.Unit);

			if (_turn.Unit.IsPlayerControlled == false)
			{
				await UniTask.Delay(300);
				_machine.Fire(BattleStateMachine.Triggers.ActionTargetSelected);
			}
		}

		public UniTask Exit()
		{
			_ui.InitMenu(null);
			_ui.ClearAimPath();

			return default;
		}

		public void Tick()
		{
			if (_turn.Unit.IsPlayerControlled == false)
			{
				return;
			}

			var mousePosition = _controls.Gameplay.MousePosition.ReadValue<Vector2>();
			var leftClick = _controls.Gameplay.LeftClick.ReadValue<float>() > 0f;

			var mouseWorldPosition = _camera.ScreenToWorldPoint(mousePosition);
			var destination = new Vector3Int(
				Mathf.RoundToInt(mouseWorldPosition.x),
				Mathf.RoundToInt(mouseWorldPosition.y),
				0
			);
			var cursorPosition = GridHelpers.GetCursorPosition(mouseWorldPosition, _config.TilemapSize);
			if (cursorPosition != _cursorPosition)
			{
				_cursorPosition = cursorPosition;

				var hitChance = GridHelpers.CalculateHitAccuracy(
					_turn.Unit.GridPosition,
					cursorPosition,
					_turnManager.BlockGrid,
					_turn.Unit,
					_turnManager.SortedUnits
				);
				_ui.HighlightAimPath(_turn.Unit.GridPosition, cursorPosition, hitChance);
			}

			if (leftClick)
			{
				switch (_turn.Action)
				{
					case Turn.Actions.Attack:
					{
						_turn.ActionTargets = new List<Vector3Int> { destination };
						_turn.ActionDestination = _cursorPosition;
					} break;
					case Turn.Actions.Build:
					{
						_turn.ActionDestination = _cursorPosition;
					} break;
					default:
					{
						Debug.LogWarning("No action selected but we still tried to select a target for it ? We probably have a bug in our state machine.");
					} break;
				}

				_machine.Fire(BattleStateMachine.Triggers.ActionTargetSelected);
			}
		}
	}
}
