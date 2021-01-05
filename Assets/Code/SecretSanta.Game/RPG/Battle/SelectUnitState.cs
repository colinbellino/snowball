using System.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectUnitState : BaseBattleState, IState
	{
		public SelectUnitState(BattleStateMachine machine) : base(machine) { }

		public async Task Enter(object[] args)
		{
			_battle.NextTurn();

			_machine.Fire(BattleStateMachine.Triggers.UnitSelected);
		}

		public async Task Exit() { }

		public void Tick() { }
	}
}
