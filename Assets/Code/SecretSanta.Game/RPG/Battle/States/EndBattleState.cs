using Cysharp.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
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

			_machine.FireBattleOver();

			return default;
		}
	}
}
