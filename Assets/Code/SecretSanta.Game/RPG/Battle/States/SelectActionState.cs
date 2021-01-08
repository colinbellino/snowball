using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.SecretSanta.Game.RPG
{
	public enum BattleAction { Attack, Move, Wait }

	public class SelectActionState : BaseBattleState, IState
	{
		public SelectActionState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public UniTask Enter(object[] args)
		{
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

			_ui.InitActionMenu(_turn.Unit);

			if (_turn.Unit.IsPlayerControlled)
			{
				_ui.ToggleButton(BattleAction.Move, _turn.HasMoved == false);
				_ui.ToggleButton(BattleAction.Attack, _turn.HasActed == false);
				_ui.ShowActionsMenu();
			}
			else
			{
				PlanComputerTurn();

				if (_turn.MovePath?.Count > 0)
				{
					_machine.Fire(BattleStateMachine.Triggers.MoveDestinationSelected);
				}
				if (_turn.AttackPath?.Count > 0)
				{
					_machine.Fire(BattleStateMachine.Triggers.ActionAttackSelected);
				}
				else
				{
					_machine.Fire(BattleStateMachine.Triggers.TurnEnded);
				}
			}

			return default;
		}

		public UniTask Exit()
		{
			_ui.InitActionMenu(null);
			_ui.HideActionsMenu();
			_ui.OnActionClicked -= OnActionClicked;

			return default;
		}

		public void Tick()
		{
			#if UNITY_EDITOR
			if (Keyboard.current.escapeKey.wasPressedThisFrame)
			{
				_machine.Fire(BattleStateMachine.Triggers.Loss);
			}
			#endif
		}

		private void OnActionClicked(BattleAction action)
		{
			switch (action)
			{
				case BattleAction.Attack:
					_machine.Fire(BattleStateMachine.Triggers.ActionAttackSelected);
					return;
				case BattleAction.Move:
					_machine.Fire(BattleStateMachine.Triggers.ActionMoveSelected);
					return;
				case BattleAction.Wait:
					_machine.Fire(BattleStateMachine.Triggers.TurnEnded);
					return;
			}
		}

		private void PlanComputerTurn()
		{
			if (_turn.Unit.Type == UnitType.Snowman)
			{
				return;
			}

			var foes = _turnManager.GetActiveUnits().Where(unit => unit.IsPlayerControlled).ToList();
			var randomTarget = foes[Random.Range(0, foes.Count)];
			_turn.AttackTargets = new List<Vector3Int> { randomTarget.GridPosition };
			_turn.AttackPath = new List<Vector3Int> { _turn.Unit.GridPosition, randomTarget.GridPosition};
		}

		private bool IsVictoryConditionReached()
		{
			return _turnManager.GetActiveUnits().Where(unit => unit.IsPlayerControlled == false).Count() == 0;
		}

		private bool IsDefeatConditionReached()
		{
			return _turnManager.GetActiveUnits().Where(unit => unit.IsPlayerControlled).Count() == 0;
		}
	}
}
