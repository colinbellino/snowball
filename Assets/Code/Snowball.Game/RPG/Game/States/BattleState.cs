using Cysharp.Threading.Tasks;

namespace Snowball.Game
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

		public UniTask Enter()
		{
			_turnManager = new TurnManager();
			_battleMachine = new BattleStateMachine(_turnManager);
			_battleMachine.Start();

			_battleMachine.BattleOver += OnBattleOver;

			return default;
		}

		public UniTask Exit()
		{
			_battleMachine.BattleOver -= OnBattleOver;

			return default;
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
