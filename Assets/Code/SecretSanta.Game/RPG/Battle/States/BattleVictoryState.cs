using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class BattleVictoryState : BaseBattleState, IState
	{
		public BattleVictoryState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public UniTask Enter(object[] args)
		{
			Debug.Log("Battle won \\o/");

			_machine.Fire(BattleStateMachine.Triggers.Done);

			return default;
		}

		public UniTask Exit() { return default; }

		public void Tick() { }
	}
}
