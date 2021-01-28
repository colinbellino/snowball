using System.Linq;
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

			_ui.HideAll();

			var result = _turnManager.GetBattleResult();

			var encounter = _database.Encounters[_state.CurrentEncounterId];
			if (result == BattleResults.Victory && encounter.EndConversation.Length > 0)
			{
				foreach (var unit in _turnManager.SortedUnits.Where(unit => unit.Alliance == Unit.Alliances.Ally))
				{
					UnitHelpers.AnimateResurrection(unit.Facade);
				}
				await _conversation.Start(encounter.EndConversation, encounter);
			}

			_controls.Gameplay.Disable();

			_ = _audio.StopMusic();
			await _transition.StartTransition(Color.white);

			_board.ClearArea();
			_board.HideEncounter();

			foreach (var unit in _turnManager.SortedUnits)
			{
				GameObject.Destroy(unit.Facade.gameObject);
				unit.Facade = null;
			}

			// Give control of all the party to the player after the tutorial
			var justWonFirstBattle = result == BattleResults.Victory && _state.CurrentEncounterId == _config.Encounters[0];
			if (justWonFirstBattle)
			{
				_state.Guests.Clear();
			}

			_machine.FireBattleEnded(result);
		}
	}
}
