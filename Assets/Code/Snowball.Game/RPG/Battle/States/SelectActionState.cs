using System.Collections.Generic;
using System.Linq;
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
			base.Enter();

			_ui.ShowTurnOrder();
			_ui.SetTurnOrder(_turnManager.GetTurnOrder());

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

			_ui.OnActionClicked += OnActionClicked;

			if (_turn.HasActed && _turn.HasMoved)
			{
				_machine.Fire(BattleStateMachine.Triggers.TurnEnded);
				return;
			}

			_ui.SetTurnUnit(_turn.Unit);
			await UniTask.Delay(300);

			if (_turn.Unit.Driver == Unit.Drivers.Human)
			{
				_ui.ToggleButton(BattleActions.Move, _turn.HasMoved == false);
				_ui.ToggleButton(BattleActions.Attack, _turn.HasActed == false);
				_ui.ToggleButton(BattleActions.Build, _turn.HasActed == false);
				_ui.ShowActionsMenu();
			}
			else
			{
				PlanComputerTurn();

				if (_turn.MovePath?.Count > 0)
				{
					_machine.Fire(BattleStateMachine.Triggers.MoveDestinationSelected);
				}
				else if (_turn.Action != Turn.Actions.None)
				{
					_machine.Fire(BattleStateMachine.Triggers.ActionSelected);
				}
				else
				{
					_machine.Fire(BattleStateMachine.Triggers.TurnEnded);
				}
			}

			return;
		}

		public override UniTask Exit()
		{
			base.Exit();

			_ui.SetTurnUnit(null);
			_ui.HideActionsMenu();
			_ui.OnActionClicked -= OnActionClicked;

			return default;
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
					_turn.Action = Turn.Actions.Attack;
					_machine.Fire(BattleStateMachine.Triggers.ActionSelected);
					return;
				case BattleActions.Build:
					if (_turn.HasActed) { return; }
					_turn.Action = Turn.Actions.Build;
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

		private void PlanComputerTurn()
		{
			if (_turn.Unit.Type == Unit.Types.Snowpal)
			{
				_turn.HasMoved = true;
				_turn.HasActed = true;
				_turn.Action = Turn.Actions.Melt;
				return;
			}

			var foes = _turnManager.GetActiveUnits()
				.Where(unit => unit.Alliance != _turn.Unit.Alliance && unit.Type == Unit.Types.Humanoid)
				.OrderBy(unit => (unit.GridPosition - _turn.Unit.GridPosition).magnitude)
				.ToList();
			var closestTarget = foes[0];

			_turn.Action = Turn.Actions.Attack;
			_turn.ActionTargets = new List<Vector3Int> { closestTarget.GridPosition };
			_turn.ActionDestination = closestTarget.GridPosition;
			_turn.HasMoved = true;
		}
	}
}
