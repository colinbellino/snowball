using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectMoveDestinationState : BaseBattleState, IState
	{
		private IEnumerable<Vector3Int> _validMovePositions;

		public SelectMoveDestinationState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public async Task Enter(object[] args)
		{
			_validMovePositions = GridHelpers.GetWalkableTilesInRange(_turn.Unit.GridPosition, _turn.Unit.MoveRange, _turnManager.WalkGrid);

			_board.HighlightTiles(_validMovePositions, _turn.Unit.Color);
			_ui.InitActionMenu(_turn.Unit);
		}

		public async Task Exit()
		{
			_ui.ClearMovePath();
			_ui.InitActionMenu(null);
			_board.ClearHighlight();
		}

		public void Tick()
		{
			var mousePosition = _controls.Gameplay.MousePosition.ReadValue<Vector2>();
			var leftClick = _controls.Gameplay.LeftClick.ReadValue<float>() > 0f;

			var mouseWorldPosition = _camera.ScreenToWorldPoint(mousePosition);
			var cursorPosition = GridHelpers.GetCursorPosition(mouseWorldPosition, _config.TilemapSize);

			var path = GridHelpers.CalculatePathWithFall(
				_turn.Unit.GridPosition, cursorPosition,
				_turnManager.WalkGrid
			);

			if (cursorPosition != _turn.Unit.GridPosition && _validMovePositions.Contains(cursorPosition))
			{
				_ui.HighlightMovePath(path);
			}
			else
			{
				_ui.ClearMovePath();
			}

			if (leftClick)
			{
				if (_validMovePositions.Contains(cursorPosition))
				{
					_turn.MovePath = path;
					_machine.Fire(BattleStateMachine.Triggers.MoveDestinationSelected);
					return;
				}
			}
		}
	}
}
