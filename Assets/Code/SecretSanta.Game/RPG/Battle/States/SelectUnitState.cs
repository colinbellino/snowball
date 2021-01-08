using System.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectUnitState : BaseBattleState, IState
	{
		public SelectUnitState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public async Task Enter(object[] args)
		{
			_turnManager.NextTurn();

			_machine.Fire(BattleStateMachine.Triggers.UnitSelected);
		}

		public async Task Exit() { }

		public void Tick() { }
	}
}
