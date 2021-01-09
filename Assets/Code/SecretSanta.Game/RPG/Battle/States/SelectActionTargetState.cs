using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectActionTargetState : BaseBattleState
	{
		private List<Vector3Int> _validMovePositions;

		public SelectActionTargetState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager)
		{
		}

		public override async UniTask Enter()
		{
			await base.Enter();

			if (_turn.Action == Turn.Actions.Build)
			{
				_validMovePositions = GridHelpers.GetWalkableTilesInRange(_turn.Unit.GridPosition,
					_turn.Unit.BuildRange, _turnManager.WalkGrid, _turnManager.SortedUnits);
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

		public override UniTask Exit()
		{
			base.Exit();

			_ui.InitMenu(null);
			_ui.ClearAimPath();
			_board.ClearHighlight();

			return default;
		}

		protected override void OnCursorMove()
		{
			if (_turn.Action == Turn.Actions.Build)
			{
				_ui.HighlightBuildTarget(_cursorPosition);
			}
			else
			{
				var hitChance = GridHelpers.CalculateHitAccuracy(
					_turn.Unit.GridPosition,
					_cursorPosition,
					_turnManager.BlockGrid,
					_turn.Unit,
					_turnManager.SortedUnits
				);
				_ui.HighlightAttackTarget(_turn.Unit.GridPosition, _cursorPosition, hitChance);
			}
		}

		protected override void OnConfirm()
		{
			if (_validMovePositions.Contains(_cursorPosition) == false)
			{
				Debug.Log("Invalid target");
				return;
			}

			_turn.ActionTargets = new List<Vector3Int> {_cursorPosition};
			_turn.ActionDestination = _cursorPosition;

			_machine.Fire(BattleStateMachine.Triggers.ActionTargetSelected);
		}

		protected override void OnCancel()
		{
			_machine.Fire(BattleStateMachine.Triggers.Cancelled);
		}
	}
}
