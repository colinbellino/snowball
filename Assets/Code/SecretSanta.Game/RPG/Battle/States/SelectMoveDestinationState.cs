using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectMoveDestinationState : BaseBattleState
	{
		public SelectMoveDestinationState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		private List<Vector3Int> _path;
		private List<Vector3Int> _validMovePositions;

		public override UniTask Enter()
		{
			base.Enter();

			_validMovePositions = GridHelpers.GetWalkableTilesInRange(_turn.Unit.GridPosition, _turn.Unit.MoveRange,
				_turnManager.WalkGrid, _turnManager.SortedUnits);

			_board.HighlightTiles(_validMovePositions, _turn.Unit.Color);
			_ui.InitMenu(_turn.Unit);

			return default;
		}

		public override UniTask Exit()
		{
			base.Exit();

			_ui.ClearMovePath();
			_ui.InitMenu(null);
			_board.ClearHighlight();

			return default;
		}

		protected override void OnCursorMove()
		{
			_path = GridHelpers.CalculatePathWithFall(
				_turn.Unit.GridPosition, _cursorPosition,
				_turnManager.WalkGrid
			);

			if (_cursorPosition != _turn.Unit.GridPosition && _validMovePositions.Contains(_cursorPosition))
				_ui.HighlightMovePath(_path);
			else
				_ui.ClearMovePath();
		}

		protected override void OnConfirm()
		{
			if (_validMovePositions.Contains(_cursorPosition) == false)
			{
				Debug.Log("Invalid destination");
				return;
			}

			_turn.MovePath = _path;
			_machine.Fire(BattleStateMachine.Triggers.MoveDestinationSelected);
		}

		protected override void OnCancel()
		{
			_machine.Fire(BattleStateMachine.Triggers.Cancelled);
		}
	}
}
