using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class BattleVictoryState : BaseBattleState, IState
	{
		public BattleVictoryState(BattleStateMachine machine) : base(machine) { }

		public async Task Enter(object[] args)
		{
			_ui.HideAll();

			var current = _config.EncountersOrder.FindIndex(id => id == _state.CurrentEncounterId);
			var next = (current + 1) % _config.EncountersOrder.Count;
			_state.CurrentEncounterId = _config.EncountersOrder[next];

			TilemapHelpers.ClearTilemap(_areaTilemap);
			foreach (var unit in _battle.Units)
			{
				GameObject.Destroy(unit.Facade.gameObject);
			}

			await UniTask.Delay(1000);

			_machine.Fire(BattleStateMachine.Triggers.Done);

			Debug.Log("Battle won \\o/");
		}

		public async Task Exit() { }

		public void Tick() { }
	}
}
