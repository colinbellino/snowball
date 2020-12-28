using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectActionState : BaseBattleState, IState
	{
		public SelectActionState(BattleStateMachine machine) : base(machine) { }

		private int _action;

		public async Task Enter(object[] args)
		{
			_action = _turn.HasMoved ? 1 : 0;

			if (_turn.HasActed && _turn.HasMoved)
			{
				_machine.Fire(BattleStateMachine.Triggers.ActionWaitSelected);
				return;
			}

			if (_turn.Unit.IsPlayerControlled == false)
			{
				ComputerTurn();
				_machine.Fire(BattleStateMachine.Triggers.ActionAttackSelected);
			}
		}

		public async Task Exit() { }

		public void Tick()
		{
			var moveInput = _controls.Gameplay.Move.ReadValue<Vector2>();
			if (moveInput.magnitude > 0f)
			{
				if (_turn.HasActed == false && moveInput.x > 0)
				{
					_action = 1;
				}
				else if (_turn.HasMoved == false && moveInput.x < 0)
				{
					_action = 0;
				}
				else if (moveInput.y > 0)
				{
					_action = 2;
				}
			}

			switch (_action)
			{
				case 0:
					_turn.Unit.SetDebugText("< Move <");
					break;
				case 1:
					_turn.Unit.SetDebugText("> Attack >");
					break;
				case 2:
					_turn.Unit.SetDebugText("^ Wait ^");
					break;
			}

			var confirmInput = _controls.Gameplay.Confirm.ReadValue<float>() > 0f;
			if (confirmInput)
			{
				switch (_action)
				{
					case 0:
						_machine.Fire(BattleStateMachine.Triggers.ActionMoveSelected);
						return;
					case 1:
						_machine.Fire(BattleStateMachine.Triggers.ActionAttackSelected);
						return;
					case 2:
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
