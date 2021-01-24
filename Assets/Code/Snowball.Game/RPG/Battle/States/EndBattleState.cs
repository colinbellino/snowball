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
			_machine.FireBattleEnded(result);
		}
	}
}
