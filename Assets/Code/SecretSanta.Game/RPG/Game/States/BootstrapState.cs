using Cysharp.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public class BootstrapState : IState
	{
		private readonly GameStateMachine _machine;

		public BootstrapState(GameStateMachine machine)
		{
			_machine = machine;
		}

		public UniTask Enter(object[] args)
		{
			DatabaseHelpers.LoadFromResources(Game.Instance.Database);

			_machine.Fire(GameStateMachine.Triggers.Done);

			return default;
		}

		public UniTask Exit() { return default; }

		public void Tick() { }
	}
}
