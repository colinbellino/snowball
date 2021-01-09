using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectActionTargetState : BaseBattleState, IState
	{
		public SelectActionTargetState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		private List<Vector3Int> _validMovePositions;
		private Vector3Int _cursorPosition;

		public async UniTask Enter(object[] args)
		{
			if (_turn.Action == Turn.Actions.Build)
			{
				_validMovePositions = GridHelpers.GetWalkableTilesInRange(_turn.Unit.GridPosition, _turn.Unit.BuildRange, _turnManager.WalkGrid, _turnManager.SortedUnits);
				_board.HighlightTiles(_validMovePositions, _turn.Unit.Color);
			}
			else
			{
				_validMovePositions = GridHelpers.GetAllTiles(_turnManager.WalkGrid);
			}

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
			_board.ClearHighlight();

			return default;
		}

		public void Tick()
		{
			if (_turn.Unit.IsPlayerControlled == false)
			{
				return;
			}

			var mousePosition = _controls.Gameplay.MousePosition.ReadValue<Vector2>();

			var mouseWorldPosition = _camera.ScreenToWorldPoint(mousePosition);
			var cursorPosition = GridHelpers.GetCursorPosition(mouseWorldPosition, _config.TilemapSize);
			if (cursorPosition != _cursorPosition)
			{
				_cursorPosition = cursorPosition;

				if (_turn.Action == Turn.Actions.Build)
				{
					_ui.HighlightBuildTarget(cursorPosition);
				}
				else
				{
					var hitChance = GridHelpers.CalculateHitAccuracy(
						_turn.Unit.GridPosition,
						cursorPosition,
						_turnManager.BlockGrid,
						_turn.Unit,
						_turnManager.SortedUnits
					);
					_ui.HighlightAttackTarget(_turn.Unit.GridPosition, cursorPosition, hitChance);
				}
			}

			var leftClick = _controls.Gameplay.LeftClick.ReadValue<float>() > 0f;
			if (leftClick)
			{
				if (_validMovePositions.Contains(cursorPosition))
				{
					_turn.ActionTargets = new List<Vector3Int> { cursorPosition };
					_turn.ActionDestination = _cursorPosition;

					_machine.Fire(BattleStateMachine.Triggers.ActionTargetSelected);
					return;
				}
				else
				{
					Debug.Log("Invalid target");
				}
			}
		}
	}
}
