using System.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public class BattleState : IState
	{
		private readonly GameStateMachine _machine;

		private BattleStateMachine _battleMachine;
		private TurnManager _turnManager;

		public BattleState(GameStateMachine machine)
		{
			_machine = machine;
		}

		public async Task Enter(object[] args)
		{
			_turnManager = new TurnManager();
			_battleMachine = new BattleStateMachine(_turnManager);

			_battleMachine.BattleOver += OnBattleOver;
		}

		public async Task Exit()
		{
			_battleMachine.BattleOver -= OnBattleOver;
		}

		public void Tick()
		{
			_battleMachine?.Tick();
		}

		private void OnBattleOver()
		{
			_machine.Fire(GameStateMachine.Triggers.StartWorldmap);
		}
	}
}
