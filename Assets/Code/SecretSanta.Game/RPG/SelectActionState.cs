using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public enum BattleAction { Attack, Move, Wait }

	public class SelectActionState : BaseBattleState, IState
	{
		public SelectActionState(BattleStateMachine machine) : base(machine) { }

		private BattleAction _action;

		public async Task Enter(object[] args)
		{
			if (_turn.HasActed && _turn.HasMoved)
			{
				_machine.Fire(BattleStateMachine.Triggers.ActionWaitSelected);
				return;
			}

			if (_turn.Unit.IsPlayerControlled)
			{
				_ui.ShowActions();
			}
			else
			{
				ComputerTurn();
				_machine.Fire(BattleStateMachine.Triggers.ActionAttackSelected);
			}
		}

		public async Task Exit()
		{
			_ui.HideActions();
		}

		public void Tick()
		{
			var moveInput = _controls.Gameplay.Move.ReadValue<Vector2>();
			if (moveInput.magnitude > 0f)
			{
				if (moveInput.y > 0)
				{
					_action = BattleAction.Move;
				}
				else if (moveInput.y < 0)
				{
					_action = BattleAction.Wait;
				}
			}
			else
			{
				_action = BattleAction.Attack;
			}

			_ui.SelectAction(_action);

			var confirmInput = _controls.Gameplay.Confirm.ReadValue<float>() > 0f;
			if (confirmInput)
			{
				switch (_action)
				{
					case BattleAction.Attack:
						_machine.Fire(BattleStateMachine.Triggers.ActionAttackSelected);
						return;
					case BattleAction.Move:
						_machine.Fire(BattleStateMachine.Triggers.ActionMoveSelected);
						return;
					case BattleAction.Wait:
						_machine.Fire(BattleStateMachine.Triggers.ActionWaitSelected);
						return;
				}
			}
		}

		private void ComputerTurn()
		{
			var randomTarget = _allUnits[0];
			_turn.Targets = new List<Vector3Int> { randomTarget.GridPosition };
			_turn.HasActed = true;
			_turn.HasMoved = true;
		}
	}
}
