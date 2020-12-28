using System.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectActionState : BaseBattleState, IState
	{
		public SelectActionState(BattleStateMachine machine) : base(machine) { }

		public async Task Enter(object[] args)
		{
			if (_turn.HasActed && _turn.HasMoved)
			{
				_machine.Fire(BattleStateMachine.Triggers.Done);
				return;
			}

			if (_turn.Unit.IsPlayerControlled == false)
			{
				ComputerTurn();
				_machine.Fire(BattleStateMachine.Triggers.AttackActionSelected);
			}
		}

		public async Task Exit() { }

		public void Tick()
		{
			var moveInput = _controls.Gameplay.Move.ReadValue<Vector2>();
			if (_turn.HasMoved == false && moveInput.magnitude > 0f)
			{
				_machine.Fire(BattleStateMachine.Triggers.MoveActionSelected);
			}

			var confirmInput = _controls.Gameplay.Confirm.ReadValue<float>() > 0f;
			if (_turn.HasActed == false && confirmInput)
			{
				_machine.Fire(BattleStateMachine.Triggers.AttackActionSelected);
			}
		}

		private void ComputerTurn()
		{
			var randomDirection = Random.Range(0, 2) > 0 ? 1 : -1;

			_turn.AttackDirection = randomDirection;
			_turn.HasActed = true;
			_turn.HasMoved = true;
		}
	}
}
