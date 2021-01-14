using Cysharp.Threading.Tasks;

namespace Snowball.Game
{
	public class EndTurnState : BaseBattleState
	{
		public EndTurnState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public override async UniTask Enter()
		{
			await base.Enter();

			_machine.Fire(BattleStateMachine.Triggers.Done);
		}
	}
}
