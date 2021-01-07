using System.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class BattleDefeatState : BaseBattleState, IState
	{
		public BattleDefeatState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public async Task Enter(object[] args)
		{
			Debug.Log("Battle lost :(");

			_machine.Fire(BattleStateMachine.Triggers.Done);
		}

		public async Task Exit() { }

		public void Tick() { }
	}
}
