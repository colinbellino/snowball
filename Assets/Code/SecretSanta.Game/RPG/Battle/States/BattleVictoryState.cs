using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
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
