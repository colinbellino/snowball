using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Snowball.Game
{
	public class SelectActionTargetState : BaseBattleState
	{
		private List<Vector3Int> _validTargets;

		public SelectActionTargetState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public override async UniTask Enter()
		{
			await base.Enter();

			_validTargets = _turn.Plan.Ability.GetTilesInRange(_turn.Unit.GridPosition, _turn.Unit, _turnManager);
			_board.HighlightTiles(_validTargets, _turn.Unit.ColorCloth);
			_ui.SetTurnUnit(_turn.Unit);

			if (_turn.Unit.Driver == Unit.Drivers.Computer)
			{
				await ShowComputerPlan();

				_machine.Fire(BattleStateMachine.Triggers.ActionTargetSelected);
			}
		}

		public override UniTask Exit()
		{
			base.Exit();

			_ui.SetTurnUnit(null);
			_ui.ClearTarget();
			_ui.HideHitRate();
			_board.ClearHighlight();

			return default;
		}

		protected override void OnCursorMove()
		{
			MoveTargetCursor(_cursorPosition);
		}

		protected override void OnConfirm()
		{
			if (_validTargets.Contains(_cursorPosition) == false)
			{
				_audio.PlaySoundEffect(_config.MenuErrorClip);
				Debug.Log("Invalid target!");
				return;
			}

			_audio.PlaySoundEffect(_config.MenuConfirmClip);
			_turn.Plan.ActionDestination = _cursorPosition;

			_machine.Fire(BattleStateMachine.Triggers.ActionTargetSelected);
		}

		protected override void OnCancel()
		{
			_audio.PlaySoundEffect(_config.MenuCancelClip);

			_machine.Fire(BattleStateMachine.Triggers.Cancelled);
		}

		private async UniTask ShowComputerPlan()
		{
			await UniTask.Delay(300);

			if (_turn.Plan.ActionDestination != _turn.Unit.GridPosition)
			{
				MoveTargetCursor(_turn.Plan.ActionDestination);

				await UniTask.Delay(800);
			}
		}

		private void MoveTargetCursor(Vector3Int cursorPosition)
		{
			var hitChance = GridHelpers.CalculateHitAccuracy(
				_turn.Unit.GridPosition,
				cursorPosition,
				_turn.Unit,
				_turnManager.BlockGrid,
				_turnManager.GetActiveUnits()
			);
			_ui.HighlightAttackTarget(_turn.Unit.GridPosition, cursorPosition, hitChance);
			_ui.HideTargetInfos();

			var target = _turnManager.SortedUnits.Find(unit => unit.GridPosition == cursorPosition);
			if (_validTargets.Contains(cursorPosition) && target != null && target != _turn.Unit)
			{
				_ui.ShowTargetInfos(target, _database.Encounters[_state.CurrentEncounterId].TeamColor);
				_ui.ShowHitRate(hitChance);
			}
			else
			{
				_ui.HideHitRate();
			}
		}
	}
}
