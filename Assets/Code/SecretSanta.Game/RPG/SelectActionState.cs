using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectActionState : IState
	{
		private readonly BattleStateMachine _machine;

		public SelectActionState(BattleStateMachine machine)
		{
			_machine = machine;
		}

		public async Task Enter(object[] args)
		{
			var turn = Game.Instance.Battle.Turn;
			var unit = turn.Unit;

			if (turn.HasActed && turn.HasMoved)
			{
				_machine.Fire(BattleStateMachine.Triggers.Done);
				return;
			}

			if (unit.IsPlayerControlled == false)
			{
				ComputerTurn();
				_machine.Fire(BattleStateMachine.Triggers.AttackActionSelected);
			}
		}

		public async Task Exit() { }

		public void Tick()
		{
			var turn = Game.Instance.Battle.Turn;

			var moveInput = Game.Instance.Controls.Gameplay.Move.ReadValue<Vector2>();
			if (turn.HasMoved == false && moveInput.magnitude > 0f)
			{
				_machine.Fire(BattleStateMachine.Triggers.MoveActionSelected);
			}

			var confirmInput = Game.Instance.Controls.Gameplay.Confirm.ReadValue<float>() > 0f;
			if (turn.HasActed == false && confirmInput)
			{
				turn.AttackDirection = turn.Unit.Direction.x > 0f ? 1 : -1;
				_machine.Fire(BattleStateMachine.Triggers.AttackActionSelected);
			}
		}

		private static void ComputerTurn()
		{
			var turn = Game.Instance.Battle.Turn;

			var randomDirection = Random.Range(0, 2) > 0 ? 1 : -1;

			turn.AttackDirection = randomDirection;
			turn.HasActed = true;
			turn.HasMoved = true;
		}
	}
}
