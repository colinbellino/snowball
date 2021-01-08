using Cysharp.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public class EndTurnState : BaseBattleState, IState
	{
		public EndTurnState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public async UniTask Enter(object[] args)
		{
			await UniTask.Delay(200);

			_machine.Fire(BattleStateMachine.Triggers.Done);
		}

		public UniTask Exit() { return default; }

		public void Tick() { }
	}
}
