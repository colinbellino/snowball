using Cysharp.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public class EndTurnState : BaseBattleState
	{
		public EndTurnState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public override async UniTask Enter()
		{
			await base.Enter();

			await UniTask.Delay(200);

			_machine.Fire(BattleStateMachine.Triggers.Done);
		}
	}
}
