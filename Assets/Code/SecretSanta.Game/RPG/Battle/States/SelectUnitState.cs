using Cysharp.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectUnitState : BaseBattleState, IState
	{
		public SelectUnitState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public UniTask Enter(object[] args)
		{
			_turnManager.NextTurn();

			_machine.Fire(BattleStateMachine.Triggers.UnitSelected);

			return default;
		}

		public UniTask Exit() { return default; }

		public void Tick() { }
	}
}
