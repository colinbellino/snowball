using Cysharp.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectUnitState : BaseBattleState
	{
		public SelectUnitState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public override UniTask Enter()
		{
			base.Enter();

			_turnManager.NextTurn();

			_machine.Fire(BattleStateMachine.Triggers.UnitSelected);

			return default;
		}
	}
}
