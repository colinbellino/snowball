using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Snowball.Game
{
	public class EndBattleState : BaseBattleState
	{
		public EndBattleState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public override async UniTask Enter()
		{
			await base.Enter();

			_controls.Gameplay.Disable();

			_ui.HideAll();

			_ = _audio.StopMusic();
			await Game.Instance.Transition.StartTransition(Color.white);

			_board.ClearArea();
			_board.HideEncounter();

			foreach (var unit in _turnManager.SortedUnits)
			{
				GameObject.Destroy(unit.Facade.gameObject);
				unit.Facade = null;
			}

			var result = _turnManager.GetBattleResult();

			// Give control of all the party to the player after the tutorial
			var justWonFirstBattle = result == BattleResults.Victory && _state.CurrentEncounterId == _config.Encounters[0];
			if (justWonFirstBattle)
			{
				Debug.Log("z");
				_state.Guests.Clear();
			}

			_machine.FireBattleEnded(result);
		}
	}
}
