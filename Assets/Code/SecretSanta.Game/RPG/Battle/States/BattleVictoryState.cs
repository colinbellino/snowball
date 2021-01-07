using System.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class BattleVictoryState : BaseBattleState, IState
	{
		public BattleVictoryState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public async Task Enter(object[] args)
		{
			Debug.Log("Battle won \\o/");

			_machine.Fire(BattleStateMachine.Triggers.Done);
		}

		public async Task Exit() { }

		public void Tick() { }
	}
}
