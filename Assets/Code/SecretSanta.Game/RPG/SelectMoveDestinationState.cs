using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NesScripts.Controls.PathFind;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectMoveDestinationState : BaseBattleState, IState
	{
		private IEnumerable<Vector3Int> _validMovePositions;
		public SelectMoveDestinationState(BattleStateMachine machine) : base(machine) { }

		public async Task Enter(object[] args)
		{
			_validMovePositions = Helpers.GetWalkableTilesInRange(_turn.Unit.GridPosition, _turn.Unit.Data.MoveRange, _battle.WalkGrid);
			TilemapHelpers.SetTiles(_validMovePositions, _highlightTilemap, _config.HighlightTile, _turn.Unit.Data.Color);
		}

		public async Task Exit()
		{
			_ui.ClearMovePath();
			TilemapHelpers.ClearTilemap(_highlightTilemap);
		}

		public void Tick()
		{
			var mousePosition = _controls.Gameplay.MousePosition.ReadValue<Vector2>();
			var leftClick = _controls.Gameplay.LeftClick.ReadValue<float>() > 0f;

			var mouseWorldPosition = _camera.ScreenToWorldPoint(mousePosition);
			var cursorPosition = Helpers.GetCursorPosition(mouseWorldPosition, _config.TilemapSize);

			var path = Helpers.CalculatePathWithFall(
				_turn.Unit.GridPosition, cursorPosition,
				_battle.WalkGrid
			);
			_ui.HighlightMovePath(path);

			if (leftClick)
			{
				if (_validMovePositions.Contains(cursorPosition))
				{
					_turn.MoveDestination = cursorPosition;
					_machine.Fire(BattleStateMachine.Triggers.MoveDestinationSelected);
					return;
				}
			}
		}
	}
}
