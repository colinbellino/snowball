using Cysharp.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public class EndBattleState : BaseBattleState, IState
	{
		public EndBattleState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public UniTask Enter(object[] args)
		{
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

		public UniTask Exit() { return default; }

		public void Tick() { }
	}
}
