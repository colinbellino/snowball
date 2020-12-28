using System.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public class EndTurnState : BaseBattleState, IState
	{
		public EndTurnState(BattleStateMachine machine) : base(machine) { }

		public async Task Enter(object[] args)
		{
			_machine.Fire(BattleStateMachine.Triggers.Done);
		}

		public async Task Exit() { }

		public void Tick() { }
	}
}
