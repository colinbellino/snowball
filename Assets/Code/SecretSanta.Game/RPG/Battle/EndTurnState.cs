using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public class EndTurnState : BaseBattleState, IState
	{
		public EndTurnState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public async Task Enter(object[] args)
		{
			await UniTask.Delay(200);

			_machine.Fire(BattleStateMachine.Triggers.Done);
		}

		public async Task Exit() { }

		public void Tick() { }
	}
}
