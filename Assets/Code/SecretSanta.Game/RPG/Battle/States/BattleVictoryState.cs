using System.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class BattleVictoryState : BaseBattleState, IState
	{
		public BattleVictoryState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public async Task Enter(object[] args)
		{
			var current = _config.EncountersOrder.FindIndex(id => id == _state.CurrentEncounterId);
			var next = (current + 1) % _config.EncountersOrder.Count;
			_state.CurrentEncounterId = _config.EncountersOrder[next];

			Debug.Log("Battle won \\o/");

			_machine.Fire(BattleStateMachine.Triggers.Done);
		}

		public async Task Exit() { }

		public void Tick() { }
	}
}
