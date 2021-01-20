using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Snowball.Game
{
	public enum BattleActions { Move, Attack, Build, Wait }

	public class SelectActionState : BaseBattleState
	{
		public SelectActionState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public override async UniTask Enter()
		{
			await base.Enter();

			var battleResult = _turnManager.GetBattleResult();
			if (battleResult == BattleResults.Victory)
			{
				_machine.Fire(BattleStateMachine.Triggers.Victory);
				return;
			}
			if (battleResult == BattleResults.Defeat)
			{
				_machine.Fire(BattleStateMachine.Triggers.Defeat);
				return;
			}

			if (_turn.HasActed && _turn.HasMoved)
			{
				_machine.Fire(BattleStateMachine.Triggers.TurnEnded);
				return;
			}

			_ui.SetTurnUnit(_turn.Unit);
			_ui.SetTurnOrder(_turnManager.GetTurnOrder());
			_ui.OnActionClicked += OnActionClicked;

			if (_turn.Unit.Driver == Unit.Drivers.Computer)
			{
				if (_turn.Plan == null)
				{
					_turn.Plan = _cpu.CalculateBestPlan(_turn.Unit, _turnManager);
				}

				if (_turn.Plan.MoveDestination != _turn.Unit.GridPosition)
				{
					_machine.Fire(BattleStateMachine.Triggers.MoveSelected);
				}
				else if (_turn.Plan.Action != TurnActions.None)
				{
					_machine.Fire(BattleStateMachine.Triggers.ActionSelected);
				}
				else
				{
					_machine.Fire(BattleStateMachine.Triggers.TurnEnded);
				}

				return;
			}

			if (_turn.Plan == null)
			{
				_turn.Plan = new Plan();
			}
			_ui.ToggleButton(BattleActions.Move, _turn.HasMoved == false);
			_ui.ToggleButton(BattleActions.Attack, _turn.HasActed == false);
			_ui.ToggleButton(BattleActions.Build, _turn.HasActed == false);
			_ui.ShowActionsMenu();
		}

		public override async UniTask Exit()
		{
			await base.Exit();

			_ui.SetTurnUnit(null);
			await _ui.HideActionsMenu();
			_ui.OnActionClicked -= OnActionClicked;
		}

		protected override void OnCancel()
		{
			#if UNITY_EDITOR
			if (Keyboard.current.escapeKey.wasPressedThisFrame)
			{
				Debug.Log("DEBUG: Abandoning fight");
				_machine.Fire(BattleStateMachine.Triggers.Defeat);
			}
			#endif
		}

		// TODO: Replace this with OnConfirm.
		// This means we have to update the current action selected in the UI (OnActionChanged) and have OnConfirm simply use _currentAction
		private void OnActionClicked(BattleActions action)
		{
			_audio.PlaySoundEffect(_config.MenuConfirmClip);

			switch (action)
			{
				case BattleActions.Move:
					if (_turn.HasMoved) { return; }
					_machine.Fire(BattleStateMachine.Triggers.MoveSelected);
					return;
				case BattleActions.Attack:
					if (_turn.HasActed) { return; }
					_turn.Plan.Action = TurnActions.Attack;
					_machine.Fire(BattleStateMachine.Triggers.ActionSelected);
					return;
				case BattleActions.Build:
					if (_turn.HasActed) { return; }
					_turn.Plan.Action = TurnActions.Build;
					_machine.Fire(BattleStateMachine.Triggers.ActionSelected);
					return;
				case BattleActions.Wait:
					_machine.Fire(BattleStateMachine.Triggers.TurnEnded);
					return;
				default:
					Debug.LogError("Action not handled.");
					return;
			}
		}
	}
}
