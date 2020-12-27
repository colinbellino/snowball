using System.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectUnitState : IState
	{
		private readonly BattleStateMachine _machine;

		public SelectUnitState(BattleStateMachine machine)
		{
			_machine = machine;
		}

		public async Task Enter(object[] args)
		{
			if (IsVictoryConditionReached())
			{
				_machine.Fire(BattleStateMachine.Triggers.BattleWon);
				return;
			}

			Game.Instance.Battle.NextTurn();

			_machine.Fire(BattleStateMachine.Triggers.UnitSelected);
		}

		public async Task Exit() { }

		public void Tick() { }

		private bool IsVictoryConditionReached()
		{
			return Game.Instance.Battle.TurnNumber > 10;
		}
	}
}
