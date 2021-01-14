using Cysharp.Threading.Tasks;

namespace Snowball.Game
{
	public class EndBattleState : BaseBattleState
	{
		public EndBattleState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public override UniTask Enter()
		{
			base.Enter();

			_ui.HideAll();
			_board.ClearArea();
			_board.HideEncounter();

			foreach (var unit in _turnManager.SortedUnits)
			{
				unit.DestroyFacade();
			}

			var result = _turnManager.GetBattleResult();
			_machine.FireBattleEnded(result);

			return default;
		}
	}
}
