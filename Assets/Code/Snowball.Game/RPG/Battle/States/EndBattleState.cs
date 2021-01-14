using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Snowball.Game
{
	public class EndBattleState : BaseBattleState
	{
		public EndBattleState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public override async UniTask Enter()
		{
			base.Enter();

			_ui.HideAll();
			_board.ClearArea();
			_board.HideEncounter();

			foreach (var unit in _turnManager.SortedUnits)
			{
				unit.DestroyFacade();
			}

			await Game.Instance.Transition.StartTransition(Color.white);

			var result = _turnManager.GetBattleResult();
			_machine.FireBattleEnded(result);
		}
	}
}
