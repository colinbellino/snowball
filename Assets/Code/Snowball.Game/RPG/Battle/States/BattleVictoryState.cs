using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Snowball.Game
{
	public class BattleVictoryState : BaseBattleState
	{
		public BattleVictoryState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public override UniTask Enter()
		{
			base.Enter();

			Debug.Log("Battle won \\o/");

			_machine.Fire(BattleStateMachine.Triggers.Done);

			return default;
		}
	}
}
