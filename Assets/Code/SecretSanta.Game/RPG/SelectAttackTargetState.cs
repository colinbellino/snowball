using System.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectAttackTargetState : BaseBattleState, IState
	{
		public SelectAttackTargetState(BattleStateMachine machine) : base(machine) { }

		public async Task Enter(object[] args)
		{
			if (_turn.Unit.IsPlayerControlled == false)
			{
				_turn.AttackDirection = _turn.Unit.Direction.x > 0f ? 1 : -1;
				_machine.Fire(BattleStateMachine.Triggers.TargetSelected);
			}
		}

		public async Task Exit() { }

		public void Tick()
		{
			var playerSelectedTarget = true;
			if (playerSelectedTarget)
			{
				_turn.AttackDirection = _turn.Unit.Direction.x;
				_machine.Fire(BattleStateMachine.Triggers.TargetSelected);
			}
		}
	}
}
