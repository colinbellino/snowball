using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.SecretSanta.Game.RPG
{
	public enum BattleActions { Move, Attack, Build, Wait }

	public class SelectActionState : BaseBattleState
	{
		public SelectActionState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public override UniTask Enter()
		{
			base.Enter();

			_ui.ShowTurnOrder();
			_ui.SetTurnOrder(_turnManager.GetTurnOrder());

			if (IsVictoryConditionReached())
			{
				_machine.Fire(BattleStateMachine.Triggers.Victory);
				return default;
			}
			if (IsDefeatConditionReached())
			{
				_machine.Fire(BattleStateMachine.Triggers.Loss);
				return default;
			}

			_ui.OnActionClicked += OnActionClicked;

			if (_turn.HasActed && _turn.HasMoved)
			{
				_machine.Fire(BattleStateMachine.Triggers.TurnEnded);
				return default;
			}

			_ui.SetTurnUnit(_turn.Unit);

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

			return default;
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
				_machine.Fire(BattleStateMachine.Triggers.Loss);
			}
			#endif
		}

		private void OnActionClicked(BattleActions action)
		{
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

		private bool IsVictoryConditionReached()
		{
			return _turnManager.GetActiveUnits().Where(unit => unit.Alliance == Unit.Alliances.Foe && unit.Type == Unit.Types.Humanoid).Count() == 0;
		}

		private bool IsDefeatConditionReached()
		{
			return _turnManager.GetActiveUnits().Where(unit => unit.Alliance == Unit.Alliances.Ally && unit.Type == Unit.Types.Humanoid).Count() == 0;
		}
	}
}
