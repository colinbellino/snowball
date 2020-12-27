using System.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public class EndTurnState : IState
	{
		private readonly BattleStateMachine _machine;

		public EndTurnState(BattleStateMachine machine)
		{
			_machine = machine;
		}

		public async Task Enter(object[] args)
		{
			_machine.Fire(BattleStateMachine.Triggers.Done);
		}

		public async Task Exit() { }

		public void Tick() { }
	}
}
