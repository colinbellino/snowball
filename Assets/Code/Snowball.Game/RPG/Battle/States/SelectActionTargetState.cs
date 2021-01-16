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
				_machine.Fire(BattleStateMachine.Triggers.ActionTargetSelected);
				return;
			}

			if (_turn.Action == Turn.Actions.Build)
			{
				_validMovePositions = GridHelpers.GetWalkableTilesInRange(_turn.Unit.GridPosition, _turn.Unit.BuildRange, _turnManager.WalkGrid, _turnManager.SortedUnits);
				_board.HighlightTiles(_validMovePositions, _turn.Unit.ColorCloth);
			}
			else
			{
				_validMovePositions = GridHelpers.GetAllTiles(_turnManager.WalkGrid);
			}

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
				_audio.PlaySoundEffect(_config.MenuErrorClip);
				Debug.Log("Invalid target!");
				return;
			}

			_audio.PlaySoundEffect(_config.MenuConfirmClip);
			_turn.ActionTargets = new List<Vector3Int> {_cursorPosition};
			_turn.ActionDestination = _cursorPosition;

			_machine.Fire(BattleStateMachine.Triggers.ActionTargetSelected);
		}

		protected override void OnCancel()
		{
			_audio.PlaySoundEffect(_config.MenuCancelClip);

			_machine.Fire(BattleStateMachine.Triggers.Cancelled);
		}
	}
}
