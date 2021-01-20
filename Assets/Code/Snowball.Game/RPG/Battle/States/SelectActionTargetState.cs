using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Snowball.Game
{
	public class SelectActionTargetState : BaseBattleState
	{
		private List<Vector3Int> _validMovePositions;

		public SelectActionTargetState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public override async UniTask Enter()
		{
			await base.Enter();

			if (_turn.Unit.Driver == Unit.Drivers.Computer)
			{
				ComputerTurn();
				return;
			}

			_validMovePositions = GetTilesInRange();
			_board.HighlightTiles(_validMovePositions, _turn.Unit.ColorCloth);

			_ui.SetTurnUnit(_turn.Unit);
		}

		public override UniTask Exit()
		{
			base.Exit();

			_ui.SetTurnUnit(null);
			_ui.ClearTarget();
			_board.ClearHighlight();

			return default;
		}

		protected override void OnCursorMove()
		{
			MoveTargetCursor(_cursorPosition);
		}

		protected override void OnConfirm()
		{
			if (_validMovePositions.Contains(_cursorPosition) == false)
			{
				_audio.PlaySoundEffect(_config.MenuErrorClip);
				Debug.Log("Invalid target!");
				return;
			}

			_audio.PlaySoundEffect(_config.MenuConfirmClip);
			_turn.Plan.ActionTargets = new List<Vector3Int> {_cursorPosition};
			_turn.Plan.ActionDestination = _cursorPosition;

			_machine.Fire(BattleStateMachine.Triggers.ActionTargetSelected);
		}

		protected override void OnCancel()
		{
			_audio.PlaySoundEffect(_config.MenuCancelClip);

			_machine.Fire(BattleStateMachine.Triggers.Cancelled);
		}

		private List<Vector3Int> GetTilesInRange()
		{
			if (_turn.Plan.Action == TurnActions.Build)
			{
				return GridHelpers.GetWalkableTilesInRange(
					_turn.Unit.GridPosition, _turn.Unit.BuildRange,
					_turnManager.WalkGrid, _turnManager.GetActiveUnits()
				);
			}

			return GridHelpers.GetTilesInRange(_turn.Unit.GridPosition, _turn.Unit.HitRange, _turnManager.EmptyGrid);
		}

		private async void ComputerTurn()
		{
			_validMovePositions = GetTilesInRange();
			_board.HighlightTiles(_validMovePositions, _turn.Unit.ColorCloth);

			await UniTask.Delay(300);

			if (_turn.Plan.Action == TurnActions.Attack)
			{
				MoveTargetCursor(_turn.Plan.ActionTargets[0]);
				await UniTask.Delay(300);
			}

			_machine.Fire(BattleStateMachine.Triggers.ActionTargetSelected);
		}

		private void MoveTargetCursor(Vector3Int cursorPosition)
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
}
