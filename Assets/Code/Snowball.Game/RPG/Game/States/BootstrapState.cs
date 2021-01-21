using Cysharp.Threading.Tasks;

namespace Snowball.Game
{
	public class BootstrapState : IState
	{
		private readonly GameStateMachine _machine;

		public BootstrapState(GameStateMachine machine)
		{
			_machine = machine;
		}

		public UniTask Enter()
		{
			DatabaseHelpers.LoadFromResources(Game.Instance.Database);
			DatabaseHelpers.PrepareAbilities(Game.Instance.Database, Game.Instance.Config, Game.Instance.Spawner);

			_machine.Fire(GameStateMachine.Triggers.Done);

			return default;
		}

		public UniTask Exit() { return default; }

		public void Tick() { }
	}
}
