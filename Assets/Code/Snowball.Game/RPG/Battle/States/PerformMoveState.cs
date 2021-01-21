using Cysharp.Threading.Tasks;

namespace Snowball.Game
{
	public class PerformMoveState : BaseBattleState
	{
		public PerformMoveState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public override async UniTask Enter()
		{
			await base.Enter();

			var path = GridHelpers.FindPath(
				_turn.Unit.GridPosition, _turn.Plan.MoveDestination,
				_turnManager.WalkGrid
			);

			await _turn.Unit.Facade.MoveOnPath(path);
			await _turn.Unit.Facade.AnimateChangeDirection(_turn.Unit.Direction);

			_turn.Unit.GridPosition = _turn.Plan.MoveDestination;
			_turn.HasMoved = true;

			_machine.Fire(BattleStateMachine.Triggers.Done);
		}
	}
}
