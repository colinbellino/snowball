using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectMoveDestinationState : BaseBattleState, IState
	{
		private IEnumerable<Vector3Int> _validMovePositions;
		private Vector3Int _cursorPosition;
		private List<Vector3Int> _path;

		public SelectMoveDestinationState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public UniTask Enter(object[] args)
		{
			_validMovePositions = GridHelpers.GetWalkableTilesInRange(_turn.Unit.GridPosition, _turn.Unit.MoveRange, _turnManager.WalkGrid, _turnManager.SortedUnits);

			_board.HighlightTiles(_validMovePositions, _turn.Unit.Color);
			_ui.InitMenu(_turn.Unit);

			return default;
		}

		public UniTask Exit()
		{
			_ui.ClearMovePath();
			_ui.InitMenu(null);
			_board.ClearHighlight();

			return default;
		}

		public void Tick()
		{
			var mousePosition = _controls.Gameplay.MousePosition.ReadValue<Vector2>();

			var mouseWorldPosition = _camera.ScreenToWorldPoint(mousePosition);
			var cursorPosition = GridHelpers.GetCursorPosition(mouseWorldPosition, _config.TilemapSize);

			if (cursorPosition != _cursorPosition)
			{
				_cursorPosition = cursorPosition;
				_path = GridHelpers.CalculatePathWithFall(
					_turn.Unit.GridPosition, cursorPosition,
					_turnManager.WalkGrid
				);

				if (cursorPosition != _turn.Unit.GridPosition && _validMovePositions.Contains(cursorPosition))
				{
					_ui.HighlightMovePath(_path);
				}
				else
				{
					_ui.ClearMovePath();
				}
			}

			var leftClick = _controls.Gameplay.LeftClick.ReadValue<float>() > 0f;
			if (leftClick)
			{
				if (_validMovePositions.Contains(cursorPosition))
				{
					_turn.MovePath = _path;
					_machine.Fire(BattleStateMachine.Triggers.MoveDestinationSelected);
					return;
				}
			}
		}
	}
}
