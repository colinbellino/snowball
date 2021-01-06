using System.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public class BootstrapState : IState
	{
		private readonly GameStateMachine _machine;

		public BootstrapState(GameStateMachine machine)
		{
			_machine = machine;
		}

		public async Task Enter(object[] args)
		{
			DatabaseHelpers.LoadFromResources(Game.Instance.Database);

			_machine.Fire(GameStateMachine.Triggers.Done);
		}

		public async Task Exit() { }

		public void Tick() { }
	}
}
