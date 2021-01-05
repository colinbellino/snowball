using System.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectUnitState : BaseBattleState, IState
	{
		public SelectUnitState(BattleStateMachine machine) : base(machine) { }

		public async Task Enter(object[] args)
		{
			if (IsVictoryConditionReached())
			{
				_machine.Fire(BattleStateMachine.Triggers.BattleWon);
				return;
			}

			_battle.NextTurn();

			_machine.Fire(BattleStateMachine.Triggers.UnitSelected);
		}

		public async Task Exit() { }

		public void Tick() { }

		private bool IsVictoryConditionReached()
		{
			return _battle.TurnNumber > 10;
		}
	}
}
